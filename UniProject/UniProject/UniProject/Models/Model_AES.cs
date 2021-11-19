using PCLCrypto;
using System;

namespace PED_Gen_2_Debug_App.Models
{
    public static class Model_AES
    {
        public static byte[] AES_Encrypt(string data, out byte[] keyArr)
        {
            byte[] cipher = null;
            keyArr = new byte[16]; //128 bit key will be generated

            try
            {
                var provider = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);            
                keyArr = WinRTCrypto.CryptographicBuffer.GenerateRandom(16);

                // The IV may be null, but supplying a random IV increases security.
                // The IV is not a secret like the key is.
                // You can transmit the IV (w/o encryption) alongside the ciphertext.
                //var iv = WinRTCrypto.CryptographicBuffer.GenerateRandom(provider.BlockLength);

                cipher = WinRTCrypto.CryptographicEngine.Encrypt(provider.CreateSymmetricKey(keyArr), System.Text.Encoding.ASCII.GetBytes(data));
            }
            catch(Exception ex)
            {
                // Do nothing
            }
            return cipher;
        }

        public static byte[] AES_Decrypt(byte[] cipherArr, byte[] keyArr)
        {
            byte[] plainArr = null;

            try
            {
                var provider = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);

                // The IV may be null, but supplying a random IV increases security.
                // The IV is not a secret like the key is.
                // You can transmit the IV (w/o encryption) alongside the ciphertext.
                //var iv = WinRTCrypto.CryptographicBuffer.GenerateRandom(provider.BlockLength);

                plainArr = WinRTCrypto.CryptographicEngine.Decrypt(provider.CreateSymmetricKey(keyArr), cipherArr);
            }
            catch (Exception ex)
            {
                // Do nothing
            }
            return plainArr;
        }
    }
}
