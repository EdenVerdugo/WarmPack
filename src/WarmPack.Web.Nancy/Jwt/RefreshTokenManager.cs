using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarmPack.Web.Nancy.Models.Jwt;

namespace WarmPack.Web.Nancy.Jwt
{
    public class DefaultRefreshTokenManager : IRefreshTokenManager
    {
        private Func<string, TokenItemModel> _Find;
        private Action _Save;
        private Action _Delete;

        public DefaultRefreshTokenManager()
        {

        }

        public DefaultRefreshTokenManager(Func<string, TokenItemModel> find, Action save, Action delete)
        {
            _Find = find;
            _Save = save;
            _Delete = delete;
        }

        public void Delete(string refreshToken)
        {
            _Delete?.Invoke();
        }

        public TokenItemModel Find(string refreshToken)
        {         
            if(_Find != null)
            {
                return _Find.Invoke(refreshToken);
            }

            return new TokenItemModel();
        }

        public void Save(TokenItemModel token)
        {
            _Save?.Invoke();            
        }
    }
}
