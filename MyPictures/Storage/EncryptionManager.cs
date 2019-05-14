using System;
using System.IO;
using System.Security.Cryptography;

namespace MyPictures.Storage
{
    class EncryptionManager
    {
        protected byte[] key, vector;

        protected SymmetricAlgorithm algorithm;

        public EncryptionManager()
        {
            // Create new Rijndae manager instance.
            this.algorithm = new RijndaelManaged();

            // Load in credentails from settings. 
            this.LoadCredentails();
        }

        protected void LoadCredentails()
        {
            // Read in key, value pair byte array from settings.
            this.key = System.Text.Encoding.ASCII.GetBytes(Properties.Settings.Default.EncryptionKey);
            this.vector = System.Text.Encoding.ASCII.GetBytes(Properties.Settings.Default.EncryptionVector);

            // Check if manager key or vector is invalid.
            if (this.key == null || this.key.Length == 0 || this.vector == null || this.vector.Length == 0)
            {
                // Generate new key, vector pair.
                this.GenerateCredentials();
            } else {
                // Set credentails on manager.
                this.algorithm.Key = this.key;
                this.algorithm.IV = this.vector;
            }
        }

        protected void GenerateCredentials()
        {
            // Generate new key and vector.
            this.algorithm.GenerateKey();
            this.algorithm.GenerateIV();

            // Save algorithm credentials on object.
            this.key = this.algorithm.Key;
            this.vector = this.algorithm.IV;

            // Save generated key and vector to settings.
            Properties.Settings.Default.EncryptionKey = System.Text.Encoding.ASCII.GetString(this.key);
            Properties.Settings.Default.EncryptionVector = System.Text.Encoding.ASCII.GetString(this.vector);
            Properties.Settings.Default.Save();
        }

        public Stream Encrypt(Stream message)
        {
            // Create byte array and insert stream contents.
            byte[] data = new byte[Convert.ToInt32(message.Length)];
            message.Read(data, 0, Convert.ToInt32(message.Length));

            // Encrypt the byte array contents.
            data = this.EncryptBytes(data);

            // Convert the byte array to a stream.
            return new MemoryStream(data);
        }

        public byte[] EncryptBytes(byte[] message)
        {
            using (var stream = new MemoryStream())
            using (var encryptor = this.algorithm.CreateEncryptor())
            using (var encrypt = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
            {
                encrypt.Write(message, 0, Convert.ToInt32(message.Length));
                encrypt.FlushFinalBlock();

                return stream.ToArray();
            }
        }

        public Stream Decrypt(Stream message)
        {
            // Create byte array and insert stream contents.
            byte[] data = new byte[Convert.ToInt32(message.Length)];
            message.Read(data, 0, Convert.ToInt32(message.Length));

            // Decrypt the byte array contents.
            data = this.DecryptBytes(data);

            // Convert the byte array to a stream.
            return new MemoryStream(data);
        }

        public byte[] DecryptBytes(byte[] message)
        {
            using (var stream = new MemoryStream())
            using (var decrypter = this.algorithm.CreateDecryptor())
            using (var decrypt = new CryptoStream(stream, decrypter, CryptoStreamMode.Write))
            {
                decrypt.Write(message, 0, Convert.ToInt32(message.Length));
                decrypt.FlushFinalBlock();

                return stream.ToArray();
            }
        }
    }
}
