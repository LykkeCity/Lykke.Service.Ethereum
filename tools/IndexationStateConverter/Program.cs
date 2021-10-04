using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.EthereumWorker.AzureRepositories;
using Lykke.Service.EthereumWorker.Core.Domain;
using Lykke.SettingsReader.ReloadingManager;
using MessagePack;
using Newtonsoft.Json;

namespace IndexationStateConverter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Usage: dotnet IndexationStateEditor.dll read|write <bil-ethereum-worker-azure-storage-connection-string> ");

            if (args.Length != 2)
            {
                Console.WriteLine("Invalid parameters count");
                return;
            }

            if (args[0] == "read")
            {
                var connectionString = ConstantReloadingManager.From(args[1]);
                var repository = BlockchainIndexationStateRepository.Create(connectionString);

                await Read(repository);
            }
            else if (args[0] == "write")
            {
                var connectionString = ConstantReloadingManager.From(args[1]);
                var repository = BlockchainIndexationStateRepository.Create(connectionString);

                await Write(repository);
            }
            else if (args[0] == "read-file")
            {
                var sourceFilePath = args[1];

                await ReadFile(sourceFilePath);
            }
            else if (args[0] == "write-file")
            {
                var destinationFilePath = args[1];

                await WriteFile(destinationFilePath);
            }
            else
            {
                Console.WriteLine($"Unknown command: {args[0]}");
            }
        }

        private static async Task Read(BlockchainIndexationStateRepository repository)
        {
            Console.WriteLine("Reading the state from BLOB...");

            var state = await repository.GetOrCreateAsync();

            Console.WriteLine("Serializing the state to json...");

            var json = JsonConvert.SerializeObject(state.AsEnumerable(), Formatting.Indented);

            Console.WriteLine("Serializing the state json to the 'state.json' file...");

            await File.WriteAllTextAsync("state.json", json);
        }

        private static async Task ReadFile(string filePath)
        {
            Console.WriteLine($"Reading the state from the file {filePath}...");

            using var stream = File.OpenRead(filePath);

            var state = BlockchainIndexationState.Restore
            (
                await MessagePackSerializer.DeserializeAsync<IEnumerable<BlocksIntervalIndexationState>>(stream)
            );

            Console.WriteLine("Serializing the state to json...");

            var json = JsonConvert.SerializeObject(state.AsEnumerable(), Formatting.Indented);

            Console.WriteLine("Serializing the state json to the 'state.json' file...");

            await File.WriteAllTextAsync("state.json", json);

            stream.Close();
        }

        private static async Task Write(BlockchainIndexationStateRepository repository)
        {
            Console.WriteLine("Reading the state from the 'state.json' file...");

            var json = await File.ReadAllTextAsync("state.json");

            Console.WriteLine("Deserializing the state from json...");

            var intervals = JsonConvert.DeserializeObject<IEnumerable<BlocksIntervalIndexationState>>(json);
            var state = BlockchainIndexationState.Restore(intervals);

            var x = state.GetNonIndexedBlockNumbers();
            
            Console.WriteLine("Saving the state to BLOB...");

            await repository.UpdateAsync(state);
        }

        private static async Task WriteFile(string destinationFile)
        {
            Console.WriteLine("Reading the state from the 'state.json' file...");

            var json = await File.ReadAllTextAsync("state.json");

            Console.WriteLine("Deserializing the state from json...");

            var intervals = JsonConvert.DeserializeObject<IEnumerable<BlocksIntervalIndexationState>>(json);
            var state = BlockchainIndexationState.Restore(intervals);

            Console.WriteLine($"Saving the state to the file {destinationFile}...");

            using var serializationStream = new MemoryStream();
            await MessagePackSerializer.SerializeAsync(serializationStream, (IEnumerable<BlocksIntervalIndexationState>)state);

            serializationStream.Position = 0;

            using var writeStream = File.OpenWrite(destinationFile);

            await serializationStream.CopyToAsync(writeStream);

            writeStream.Close();
        }
    }
}
