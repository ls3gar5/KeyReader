using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holistor.KeyReader.Dto
{
    class TokenResult
    {
        public string AccessToken { get; set; }
       
        public string EncryptedAccessToken { get; set; }
       
        public int? ExpireInSeconds { get; set; }
       
        public bool? ShouldResetPassword { get; set; }
        
        public string PasswordResetCode { get; set; }
       
        public bool? RequiresTwoFactorVerification { get; set; }
        
        public string TwoFactorAuthProviders { get; set; }
        
        public int? UserId { get; set; }
       
        public string TwoFactorRememberClientToken { get; set; }
       
        public string ReturnUrl { get; set; }

    }
}
