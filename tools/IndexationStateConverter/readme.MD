Converts BIL Ethereum worker indexation state .bin file to .json and vice versa.

Usage:

```
dotnet build
cd .\bin\Debug\netcoreapp2.2\ 
dotnet IndexationStateConverter.dll [Command] <options>
```

Available commands:

- ```read``` - reads the ".bin" file from BLOB, converts it to json and saves it to the "state.json" file`. Command options are:
  - "<EthereumJobAzureStorageConnectionString>"
- ```write``` - reads the "state.json" file, converts it to the binary format and saves it to the ".bin" file in BLOB. Command options are:
  - "<EthereumJobAzureStorageConnectionString>"
- ```read-file``` - reads the ".bin" file from specified file, converts it to json and saves it to the "state.json" file`. Command options are:
  - "<binFilePath>"
- ```write``` - reads the "state.json" file, converts it to the binary format and saves it to the specified ".bin" file. Command options are:
  - "<binFilePath>"