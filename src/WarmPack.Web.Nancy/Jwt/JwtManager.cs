using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WarmPack.Web.Nancy.Models.Jwt;
using WarmPack.Web.Nancy.Models.Security;

namespace WarmPack.Web.Nancy.Jwt
{
    public static class JwtManager
    {
        // modificar la clave de seguridad del token
        public static string Key = "D1f4rM3r01!!";

        // modificar el emisor del token si es necesario
        public static string ValidIssuer = "http://www.difarmer.com";

        // modificar las audencias del token
        public static List<String> ValidAudiences = new List<String> { "Sistema Agentes", "Pagina Web" };        
        
        public static SymmetricSecurityKey SigningKey()
        {
            var keyByteArray = Encoding.ASCII.GetBytes(Convert.ToBase64String(Encoding.ASCII.GetBytes(Key)));

            return new SymmetricSecurityKey(keyByteArray);
        }

        //public static List<RefreshTokenItemModel> RefreshTokens = new List<RefreshTokenItemModel>();
        public static Action<List<Claim>> JwtExtraClaims;

        public static TokenResponseModel GetJwt(UserModel usuario)
        {
            var now = DateTime.Now;
            const int expireMinutes = 518400;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.User),
                new Claim("id", usuario.Id.ToString()),                
                new Claim("user", usuario.User),
                new Claim("name", usuario.Name),                
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
            };

            JwtExtraClaims(claims);

            var jwt = new JwtSecurityToken(
                issuer: ValidIssuer,
                audience: ValidAudiences?.FirstOrDefault(),
                claims: claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromMinutes(expireMinutes)),
                signingCredentials: new SigningCredentials(SigningKey(), SecurityAlgorithms.HmacSha256)
            );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new TokenResponseModel
            {
                AccessToken = encodedJwt,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                RefreshToken = Guid.NewGuid().ToString()
            };


            //RefreshTokenRepository.Guardar(new RefreshTokenItemModel() { Usuario = usuario, Uid = response.RefreshToken });
            //Globales.RefreshTokens.RemoveAll(p => p.Usuario?.IdUsuario == usuario.IdUsuario);
            //Globales.RefreshTokens.Add(new RefreshTokenItemModel() { Usuario = usuario, Uid = response.RefreshToken });

            return response;
        }
    }
}
