using System.Text;

namespace User.Core
{
    internal class PasswordService
    {
        public PasswordService()
        {
        }

        public (string encryptedPassword, string slat) Encrypt(string password)
        {
            var slat = Guid.NewGuid().ToString("N");
            var slatPassword = BuildSlatPassword(slat, password);
            return (slatPassword, slat);
        }

        public bool CheckPassword(string plaintextPassword, string slat, string encryptedPassword)
        {
            var slatPassword = BuildSlatPassword(slat, plaintextPassword);
            return slatPassword == encryptedPassword;
        }

        private static string BuildSlatPassword(string slat, string password)
        {
            var strBuilder = new StringBuilder(slat[..16]);
            strBuilder.Append(password);
            strBuilder.Append(slat[16..]);

            var slatPassword = strBuilder.ToString();
            var slatPasswordBytes = Encoding.UTF8.GetBytes(slatPassword);
            var hashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(slatPasswordBytes);

            return Convert.ToBase64String(hashBytes);
        }
    }
}
