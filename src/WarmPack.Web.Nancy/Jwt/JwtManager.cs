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
        /// <summary>
        /// Esta es la clave de seguridad del token usar 12 caracteres. default => W4rmP4ck#13!
        /// </summary>
        public static string Key { get; set; } = "W4rmP4ck#13!";

        // modificar el emisor del token si es necesario. 
        /// <summary>
        /// Esta es la entidad que generó el token. default => http://www.warmpack.com
        /// </summary>
        public static string ValidIssuer { get; set; } = "http://www.warmpack.com";

        // modificar las audencias del token
        /// <summary>
        /// Lista de audiencias para el token. default => Pagina web
        /// </summary>
        public static List<String> ValidAudiences = new List<String> { "Pagina web" };        
        
        public static SymmetricSecurityKey SigningKey()
        {
            var keyByteArray = Encoding.ASCII.GetBytes(Convert.ToBase64String(Encoding.ASCII.GetBytes(Key)));

            return new SymmetricSecurityKey(keyByteArray);
        }
        
        /// <summary>
        /// Los datos extras que se necesiten guardar para el usuario van aqui
        /// </summary>
        public static Action<List<Claim>> JwtExtraClaims { get; set; }

        public static IRefreshTokenManager RefreshTokenManager { get; set; }

        public static TokenResponseModel GetJwt(UserModel usuario)
        {
            var utc0 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var now = DateTime.Now;
            const int expireMinutes = 518400;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.User),
                new Claim("id", usuario.Id.ToString()),                
                new Claim("user", usuario.User),
                new Claim("name", usuario.Name),                
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, ((int)now.Subtract(utc0).TotalSeconds).ToString(), ClaimValueTypes.Integer64)
            };

            JwtExtraClaims?.Invoke(claims);

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

            RefreshTokenManager?.Save(new TokenItemModel(usuario, response.AccessToken, response.RefreshToken, response.ExpiresAt));

            return response;
        }
    }
}
