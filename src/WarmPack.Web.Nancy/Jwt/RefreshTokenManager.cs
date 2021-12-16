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
        private Func<object, ITokenResponseModel> _Find;
        private Func<object> _Save;
        private Func<object> _Delete;

        public DefaultRefreshTokenManager()
        {

        }

        public DefaultRefreshTokenManager(Func<object, ITokenResponseModel> find, Func<object> save, Func<object> delete)
        {
            _Find = find;
            _Save = save;
            _Delete = delete;
        }

        public object Delete(object deleteObj)
        {
            return _Delete?.Invoke();
        }

        public ITokenResponseModel Find(object findObj)
        {         
            if(_Find != null)
            {
                return _Find.Invoke(findObj);
            }

            return new TokenItemModel();
        }

        public object Save(ITokenResponseModel token)
        {
            return _Save?.Invoke();            
        }
    }
}
