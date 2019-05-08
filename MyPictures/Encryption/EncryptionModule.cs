using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyPictures.Encryption
{
    class EncryptionModule
    {
        private byte[] key, iv;

        public EncryptionModule()
        {
            // If Rijndael key have already been generated
            if (RijndaelExistance())
            {
                // Load key into class variables
                LoadRijndael();
            }
            else
            {
                // Generate new Rijndael key
                CreateRijndael();
                // Save to settings
                SaveRijndael();
            }
        }

        private void SaveRijndael()
        {
            // Cast the key and iv to strings and insert into settings
            Properties.Settings.Default.RijndaelKey = System.Text.Encoding.ASCII.GetString(key);
            Properties.Settings.Default.RijndaelIV = System.Text.Encoding.ASCII.GetString(iv);

            // Save settings file
            Properties.Settings.Default.Save();
        }

        private void LoadRijndael()
        {
            // Load into workspace and cast to byte arrays
            key = System.Text.Encoding.ASCII.GetBytes(Properties.Settings.Default.RijndaelKey);
            iv = System.Text.Encoding.ASCII.GetBytes(Properties.Settings.Default.RijndaelIV);
        }

        private bool RijndaelExistance()
        {
            // Load key from settings
            String LoadedKey = Properties.Settings.Default.RijndaelKey;

            // If key loaded does not exist no Rijndael exists
            if (LoadedKey == null || LoadedKey == "") return false;

            // Load initialization vector
            String LoadedIV = Properties.Settings.Default.RijndaelKey;
            if (LoadedIV == null || LoadedIV == "") return false;

            return true;
        }

        private void CreateRijndael()
        {
            // Create a Managed Rijndael to generate key and initialization vector
            using (var rijndael = new RijndaelManaged())
            {
                // Generate and store key+IV in class vars
                rijndael.GenerateKey();
                rijndael.GenerateIV();
                key = rijndael.Key;
                iv = rijndael.IV;
            }
        }

        public byte[] EncryptBytes(byte[] message)
        {
            // Create Rijndael object and load vars
            var rijndael = new RijndaelManaged();
            {
                rijndael.Key = key;
                rijndael.IV = iv;
            }

            // return if message is empty
            if ((message == null) || (message.Length == 0)) return message;

            // Throw error if n
            //if (rijndael == null) throw new ArgumentNullException("alg");
            
            // Create streams and encrypters
            using (var stream = new MemoryStream())
            using (var encryptor = rijndael.CreateEncryptor())
            using (var encrypt = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
            {
                encrypt.Write(message, 0, message.Length);
                encrypt.FlushFinalBlock();
                return stream.ToArray();
            }
        }

        public byte[] DecryptBytes(byte[] message)
        {
            // Create Rijndael object and load vars
            var rijndael = new RijndaelManaged();
            {
                rijndael.Key = key;
                rijndael.IV = iv;
            }

            if ((message == null) || (message.Length == 0)) return message;

            // Create streams and encrypters
            using (var stream = new MemoryStream())
            using (var decryptor = rijndael.CreateDecryptor())
            using (var encrypt = new CryptoStream(stream, decryptor, CryptoStreamMode.Write))
            {
                encrypt.Write(message, 0, message.Length);
                encrypt.FlushFinalBlock();
                return stream.ToArray();
            }
        }

    }
}