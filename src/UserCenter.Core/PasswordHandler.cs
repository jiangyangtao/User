using System.Text;
using UserCenter.Model;
using Yangtao.Hosting.Extensions;

namespace UserCenter.Core
{
    internal class PasswordHandler : IDisposable
    {
        /// <summary>
        /// 明文密码
        /// </summary>
        private readonly string PlaintextPassword;


        private PasswordHandler(string plaintextPassword)
        {
            PlaintextPassword = plaintextPassword;
        }

        private PasswordHandler(string slat, string encryptedPassword, string plaintextPassword) : this(plaintextPassword)
        {
            Slat = slat;
            EncryptedPassword = encryptedPassword;
        }

        /// <summary>
        /// 加密密码
        /// </summary>
        public string EncryptedPassword { private set; get; }

        /// <summary>
        /// 盐
        /// </summary>
        private string Slat { set; get; }

        /// <summary>
        /// 密码盐
        /// </summary>
        public string PasswordSlat
        {
            get
            {
                if (Slat.IsNullOrEmpty()) Slat = Guid.NewGuid().ToString("N");

                return Slat;
            }
        }

        public static PasswordHandler CreateHandler(string plaintextPassword) => new(plaintextPassword);

        public static PasswordHandler CreateHandler(User user, string plaintextPassword) => new(user.Salt, user.Password, plaintextPassword);

        public string Encrypt() => BuildSlatPassword(PlaintextPassword);

        /// <summary>
        /// 加密新密码
        /// </summary>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public string EncryptNewPassword(string newPassword) => BuildSlatPassword(newPassword);

        /// <summary>
        /// 密码比对
        /// </summary>
        /// <returns>True 则比对成功，false 则比对失败</returns>
        public bool PasswordComparison()
        {
            var slatPassword = BuildSlatPassword(PlaintextPassword);
            return slatPassword == EncryptedPassword;
        }

        private string BuildSlatPassword(string password)
        {
            var strBuilder = new StringBuilder(PasswordSlat[..16]);
            strBuilder.Append(password);
            strBuilder.Append(PasswordSlat[16..]);

            var slatPassword = strBuilder.ToString();
            var slatPasswordBytes = Encoding.UTF8.GetBytes(slatPassword);
            var sha512 = System.Security.Cryptography.SHA512.Create();
            var hashBytes = sha512.ComputeHash(slatPasswordBytes);

            return Convert.ToBase64String(hashBytes);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
