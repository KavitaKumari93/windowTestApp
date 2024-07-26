using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FF.Cockpit.Common
{
    public static class CryptoString
    {
        #region FotoFinder-Schlüssel
        //======================================================================
        public static byte[] FFKey = { 1,  2,  3,  4,  5,  6,  7,  8,
                                       9, 10, 11, 12, 13, 14, 15, 16,
                                      17, 18, 19, 20, 21, 22, 23, 24,
                                      25, 26, 27, 28, 29, 30, 31, 32};
        public static byte[] FFIV = { 1,  2,  3,  4,  5,  6,  7,  8,
                                      9, 10, 11, 12, 13, 14, 15, 16};
        //======================================================================
        #endregion

        #region Felder
        //======================================================================
        private static byte[] savedKey = null;
        private static byte[] savedIV = null;
        //======================================================================
        #endregion

        #region Eigenschaften
        //======================================================================
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public static byte[] Key
        {
            get { return savedKey; }
            set { savedKey = value; }
        }
        //----------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the IV.
        /// </summary>
        /// <value>The IV.</value>
        public static byte[] IV
        {
            get { return savedIV; }
            set { savedIV = value; }
        }
        //======================================================================
        #endregion

        #region Methoden (private)
        //======================================================================
        /// <summary>
        /// Rds the generate secret key.
        /// </summary>
        /// <param name="rdProvider">The rd provider.</param>
        private static void RdGenerateSecretKey(RijndaelManaged rdProvider)
        {
            if (savedKey == null)
            {
                rdProvider.KeySize = 256;
                rdProvider.GenerateKey();
                savedKey = rdProvider.Key;
            }
        }
        //----------------------------------------------------------------------
        /// <summary>
        /// Rds the generate secret init vector.
        /// </summary>
        /// <param name="rdProvider">The rd provider.</param>
        private static void RdGenerateSecretInitVector(RijndaelManaged rdProvider)
        {
            if (savedIV == null)
            {
                rdProvider.GenerateIV();
                savedIV = rdProvider.IV;
            }
        }
        //======================================================================
        #endregion

        #region Methoden (public)
        //======================================================================
        /// <summary>
        /// Encrypts the specified original STR.
        /// </summary>
        /// <param name="originalStr">The original STR.</param>
        /// <returns></returns>
        public static string Encrypt(string originalStr)
        {
            // Encode data string to be stored in memory
            byte[] originalStrAsBytes = Encoding.ASCII.GetBytes(originalStr);
            byte[] originalBytes = { };

            // Create MemoryStream to contain output
            MemoryStream memStream = new MemoryStream(originalStrAsBytes.Length);

            RijndaelManaged rijndael = new RijndaelManaged();

            // Generate and save secret key and init vector
            RdGenerateSecretKey(rijndael);
            RdGenerateSecretInitVector(rijndael);

            if (savedKey == null || savedIV == null)
            {
                throw (new NullReferenceException("savedKey and savedIV must be non-null."));
            }

            // Create encryptor, and stream objects
            ICryptoTransform rdTransform = rijndael.CreateEncryptor((byte[])savedKey.
                                Clone(), (byte[])savedIV.Clone());
            CryptoStream cryptoStream = new CryptoStream(memStream, rdTransform,
                                CryptoStreamMode.Write);

            // Write encrypted data to the MemoryStream
            cryptoStream.Write(originalStrAsBytes, 0, originalStrAsBytes.Length);
            cryptoStream.FlushFinalBlock();
            originalBytes = memStream.ToArray();

            // Release all resources
            memStream.Close();
            cryptoStream.Close();
            rdTransform.Dispose();
            rijndael.Clear();

            // Convert encrypted string
            string encryptedStr = Convert.ToBase64String(originalBytes);
            return (encryptedStr);
        }
        //----------------------------------------------------------------------
        /// <summary>
        /// Decrypts the specified encrypted STR.
        /// </summary>
        /// <param name="encryptedStr">The encrypted STR.</param>
        /// <returns></returns>
        public static string Decrypt(string encryptedStr)
        {
            // Unconvert encrypted string
            byte[] encryptedStrAsBytes = Convert.FromBase64String(encryptedStr);
            byte[] initialText = new Byte[encryptedStrAsBytes.Length];

            RijndaelManaged rijndael = new RijndaelManaged();
            MemoryStream memStream = new MemoryStream(encryptedStrAsBytes);

            if (savedKey == null || savedIV == null)
            {
                throw (new NullReferenceException("savedKey and savedIV must be non-null."));
            }

            // Create decryptor, and stream objects
            ICryptoTransform rdTransform = rijndael.CreateDecryptor((byte[])savedKey.
                                Clone(), (byte[])savedIV.Clone());
            CryptoStream cryptoStream = new CryptoStream(memStream, rdTransform,
                                CryptoStreamMode.Read);

            // Read in decrypted string as a byte[]
            cryptoStream.Read(initialText, 0, initialText.Length);

            // Release all resources
            memStream.Close();
            cryptoStream.Close();
            rdTransform.Dispose();
            rijndael.Clear();

            // Convert byte[] to string
            int count = 0;
            for (count = 0; count < initialText.Length; count++)
            {
                if (initialText[count] == 0)
                {
                    break;
                }
            }
            string decryptedStr = Encoding.ASCII.GetString(initialText, 0, count);
            return (decryptedStr);
        }
        //======================================================================
        #endregion
    }
}
