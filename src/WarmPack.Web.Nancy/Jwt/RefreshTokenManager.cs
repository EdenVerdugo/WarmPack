using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarmPack.Web.Nancy.Models.Jwt;

namespace WarmPack.Web.Nancy.Jwt
{
    public class RefreshTokenManager : IRefreshTokenManager
    {
        public TokenItemModel Find(string refreshToken)
        {            
            return new TokenItemModel();
        }

        public void Save(TokenItemModel token)
        {
            
        }
    }
}
