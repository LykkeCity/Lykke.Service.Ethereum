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

        public static byte[] GetAddress(
            byte[] publicKey)
        {
            return Keccak256.Sum
            (
                publicKey.Slice(1)   
            ).Slice(12, 32);
        }
        
        public static async Task<byte[]> GetAddressAsync(
            byte[] publicKey)
        {
            return (await Keccak256.SumAsync
            (
                publicKey.Slice(1)   
            )).Slice(12, 32);
        }
        
        public static byte[] GetPublicKey(
            byte[] privateKey)
        {
            var ecKey = new ECKey(privateKey, true);

            return ecKey.GetPubKey(false);
        }

        public static byte[] RecoverAddress(
            byte[] messageHash,
            byte[] signature)
        {
            return GetAddress
            (
                RecoverPublicKey
                (
                    messageHash: messageHash,
                    signature: signature
                )
            );
        }
        
        public static byte[] RecoverPublicKey(
            byte[] messageHash,
            byte[] signature)
        {
            var (r, s, recId) = DeconstructSignature(signature);
            var ecdsaSignature = new ECDSASignature(r, s);
            var ecKey = ECKey.RecoverFromSignature(recId, ecdsaSignature, messageHash, false);

            return ecKey.GetPubKey(false);
        }
        
        public static byte[] Sign(
            byte[] messageHash,
            byte[] privateKey)
        {
            var ecKey = new ECKey(privateKey, true);
            var ecdsaSignature = ecKey.Sign(messageHash);

            return ConstructSignature
            (
                r: ecdsaSignature.R,
                s: ecdsaSignature.S,
                recId: CalculateRecId(ecKey, ecdsaSignature, messageHash)
            );
        }

        private static int CalculateRecId(
            ECKey ecKey,
            ECDSASignature ecdsaSignature,
            byte[] messageHash)
        {
            var publicKey = ecKey.GetPubKey(false);
            var recId = 0;

            while (recId <= 3)
            {
                var recoveredEcKey = ECKey.RecoverFromSignature(recId, ecdsaSignature, messageHash, false);

                if (recoveredEcKey?.GetPubKey(false).SequenceEqual(publicKey) == true)
                {
                    return recId;
                }
                
                recId++;
            }
            
            throw new Exception("Can not calculate RecId. This should never have happened.");
        }
        
        private static byte[] ConstructSignature(
            BigInteger r,
            BigInteger s,
            int recId)
        {
            var signature = new byte[65];

            signature[64] = (byte) recId;
            
            var rBytes = r.ToByteArrayUnsigned();
            var sBytes = s.ToByteArrayUnsigned();
            
            Buffer.BlockCopy(rBytes, 0, signature,  0, 32);
            Buffer.BlockCopy(sBytes, 0, signature, 32, 32);
            
            return signature;
        }
        
        private static (BigInteger R, BigInteger S, int RecId) DeconstructSignature(
            byte[] signature)
        {
            var r = new BigInteger
            (
                sign: 1,
                bytes: signature.Slice(0, 32)
            );
            
            var s = new BigInteger
            (
                sign: 1,
                bytes: signature.Slice(32, 64)
            );
            
            var recId = signature[64];

            return (r, s, recId);
        }
    }
}
