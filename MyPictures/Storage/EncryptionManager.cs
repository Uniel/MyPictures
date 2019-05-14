using System.IO;
using System.Security.Cryptography;

namespace MyPictures.Encryption
{
    class EncryptionManager
    {
        private byte[] key, iv;

        public EncryptionManager()
        {
            // Load existing Rijndael key or create new.
            if (this.RijndaelExistance())
            {
                this.LoadRijndael();
            } else {
                this.CreateRijndael();
            }
        }

        private void SaveRijndael()
        {
            // Cast the key and iv to strings and insert into settings.
            Properties.Settings.Default.RijndaelKey = System.Text.Encoding.ASCII.GetString(key);
            Properties.Settings.Default.RijndaelIV = System.Text.Encoding.ASCII.GetString(iv);

            // Save settings file to storage.
            Properties.Settings.Default.Save();
        }

        private void LoadRijndael()
        {
            // Load keys into workspace and cast to byte arrays.
            key = System.Text.Encoding.ASCII.GetBytes(Properties.Settings.Default.RijndaelKey);
            iv = System.Text.Encoding.ASCII.GetBytes(Properties.Settings.Default.RijndaelIV);
        }

        private bool RijndaelExistance()
        {
            // Load key from setting properties.
            string LoadedKey = Properties.Settings.Default.RijndaelKey;

            // Return out if keys was not loaded in.
            if (LoadedKey == null || LoadedKey == "") return false;

            // Load initialization vector from settings.
            string LoadedIV = Properties.Settings.Default.RijndaelKey;
            if (LoadedIV == null || LoadedIV == "") return false;

            // Return key exists.
            return true;
        }

        private void CreateRijndael()
        {
            // Create manager and generate key and initialization vector.
            using (var rijndael = new RijndaelManaged())
            {
                // Generate and store key+IV in class variables.
                rijndael.GenerateKey();
                rijndael.GenerateIV();
                key = rijndael.Key;
                iv = rijndael.IV;
            }

            // Save generated details to storage.
            this.SaveRijndael();
        }

        public byte[] EncryptBytes(byte[] message)
        {
            // Return out if message is empty.
            if ((message == null) || (message.Length == 0))
            {
                return message;
            }

            // Create manager and set keys.
            var rijndael = new RijndaelManaged();
            {
                rijndael.Key = key;
                rijndael.IV = iv;
            }

            // Create streams and encrypters.
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
            // Return out if message is empty.
            if ((message == null) || (message.Length == 0))
            {
                return message;
            }

            // Create manager and set keys.
            var rijndael = new RijndaelManaged();
            {
                rijndael.Key = key;
                rijndael.IV = iv;
            }

            // Create streams and encrypters.
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
