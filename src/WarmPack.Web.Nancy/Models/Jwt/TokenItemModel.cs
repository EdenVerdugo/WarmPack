using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarmPack.Web.Nancy.Models.Security;

namespace WarmPack.Web.Nancy.Models.Jwt
{
    public class TokenItemModel : TokenResponseModel
    {        
        public UserModel User { get; set; }

        public TokenItemModel()
        {

        }

        public TokenItemModel(UserModel user, string accessToken, string refreshToken, DateTime expiresAt)
        {
            User = user;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            ExpiresAt = expiresAt;
        }
    }
}
