using WarmPack.Web.Nancy.Models.Jwt;

namespace WarmPack.Web.Nancy.Jwt
{
    public interface IRefreshTokenManager
    {
        void Save(TokenItemModel token);
        TokenItemModel Find(string refreshToken);
    }
}