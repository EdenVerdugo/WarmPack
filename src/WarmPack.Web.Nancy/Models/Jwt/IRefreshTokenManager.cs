using WarmPack.Web.Nancy.Models.Jwt;

namespace WarmPack.Web.Nancy.Jwt
{
    public interface IRefreshTokenManager
    {
        object Save(ITokenResponseModel token);
        object Delete(object param);
        ITokenResponseModel Find(object param);
    }
}