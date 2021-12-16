using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarmPack.Web.Nancy.Models.Jwt
{
    public class TokenResponseModel : ITokenResponseModel
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string RefreshToken { get; set; }
        public string AccessId { get; set; }
    }

    public interface ITokenResponseModel
    {
        string AccessToken { get; set; }
        DateTime ExpiresAt { get; set; }
        string RefreshToken { get; set; }
        string AccessId { get; set; }
    }
}
