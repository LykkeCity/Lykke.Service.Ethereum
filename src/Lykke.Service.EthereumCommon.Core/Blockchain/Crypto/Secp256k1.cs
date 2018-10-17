using System;
using System.Linq;
using System.Threading.Tasks;
using Nethereum.Signer.Crypto;
using Nethereum.Util;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Lykke.Service.Ethereum.Core.Blockchain.Crypto
{
    public static class Secp256K1
    {
        private static readonly SecureRandom SecureRandom = new SecureRandom();


        public static byte[] GeneratePrivateKey()
        {
            while (true)
            {
                var generator = new ECKeyPairGenerator("EC");
                var generatorInitParams = new KeyGenerationParameters(SecureRandom, 256);

                generator.Init(generatorInitParams);

                var keyPair = generator.GenerateKeyPair();
                var privateBytes = ((ECPrivateKeyParameters) keyPair.Private).D.ToByteArray();

                if (privateBytes.Length == 32)
                {
                    return privateBytes;
                }
            }
        }

        public static byte[] GetPublicKey(
            byte[] privateKey)
        {
            var ecKey = new ECKey(privateKey, true);

            return ecKey.GetPubKey(false);
        }
    }
}
