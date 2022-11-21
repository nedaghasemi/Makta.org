using System.Collections.Generic;

namespace Common
{
    public class SiteSettings
    {
        public CommonSetting CommonSetting { get; set; }
        public EmailSetting EmailSettings { get; set; }
        public IdentitySetting IdentitySetting { get; set; }
    }

    public class CommonSetting
    {
        public string ProductName { get; set; }
        public string Domain { get; set; }
        public string Url => $"https://{Domain}";
        public string Email { get; set; }
        public string Logo { get; set; }
        public string FavIconFolderName { get; set; }
        public string AdminEmail { get; set; }
        public string DefaultAdminPassword { get; set; }
        public string HomePic { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Linkedin { get; set; }
        public string ChatHubUrl { get; set; }
        public string NotificationHubUrl { get; set; }
        public string ProtectionSecretKey { get; set; }
        public string TermsandConditions { get; set; }
    }

    public class IdentitySetting
    {
        public bool PasswordRequireDigit { get; set; }
        public int PasswordRequiredLength { get; set; }
        public bool PasswordRequireNonAlphanumic { get; set; }
        public bool PasswordRequireUppercase { get; set; }
        public bool PasswordRequireLowercase { get; set; }
        public bool RequireUniqueEmail { get; set; }
    }

    public class EmailSetting
    {
        public string SettingName { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
