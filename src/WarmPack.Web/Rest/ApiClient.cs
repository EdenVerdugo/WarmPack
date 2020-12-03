using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WarmPack.Web.Rest
{
    public class ApiClient
    {
        private string _UrlBase;
        private Action<HttpRequestHeaders> _ActionHeaders;
        private Action<HttpContentHeaders> _ContentHeaders;

        public ApiClient(string urlBase)
        {
            this._UrlBase = urlBase;
        }

        private byte[] GetJsonBytes(object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var bytes = Encoding.UTF8.GetBytes(json);

            return bytes;
        }

        private ByteArrayContent GetByteContent(object data)
        {
            var content = new ByteArrayContent(GetJsonBytes(data));

            if (_ContentHeaders == null)
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            else
            {
                _ContentHeaders.Invoke(content.Headers);
            }

            return content;
        }

        public void SetRequestHeaders(Action<HttpRequestHeaders> actionHeaders)
        {
            _ActionHeaders = actionHeaders;
        }

        public void SetContentHeaders(Action<HttpContentHeaders> contentHeaders)
        {
            _ContentHeaders = contentHeaders;
        }

        private HttpClient GetHttpClient()
        {
            var client = new HttpClient();

            _ActionHeaders?.Invoke(client.DefaultRequestHeaders);

            return client;
        }

        public async Task<ApiClientResult> Get(string route)
        {
            using (var client = GetHttpClient())
            {
                var response = await client.GetAsync($"{_UrlBase}{route}");

                var json = await response.Content.ReadAsStringAsync();

                return new ApiClientResult(response, json);
            }
        }

        public static async Task<ApiClientResult> Get(string route, Action<HttpRequestHeaders> actionHeaders = null, Action<HttpContentHeaders> contentHeaders = null)
        {
            var client = new ApiClient("");

            if (actionHeaders != null)
            {
                client.SetRequestHeaders(actionHeaders);
            }

            if (contentHeaders != null)
            {
                client.SetContentHeaders(contentHeaders);
            }

            return await client.Get(route);
        }

        public async Task<ApiClientResult> Post(string route, object data)
        {
            using (var client = GetHttpClient())
            {
                using (var content = GetByteContent(data))
                {
                    var response = await client.PostAsync($"{_UrlBase}{route}", content);

                    var json = await response.Content.ReadAsStringAsync();

                    return new ApiClientResult(response, json); ;
                }
            }
        }

        public async Task<ApiClientResult<T>> Post<T>(string route, object data)
        {
            using (var client = GetHttpClient())
            {
                using (var content = GetByteContent(data))
                {
                    var response = await client.PostAsync($"{_UrlBase}{route}", content);

                    var json = await response.Content.ReadAsStringAsync();

                    return new ApiClientResult<T>(response, json); ;
                }
            }
        }

        public async Task<ApiClientResult> PostFormUrlEncoded(string route, IEnumerable<KeyValuePair<string, string>> data)
        {
            using (var cliente = GetHttpClient())
            {
                using (var content = new FormUrlEncodedContent(data))
                {
                    content.Headers.Clear();
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");


                    var response = await cliente.PostAsync($"{_UrlBase}{route}", content);

                    var r = response.Content?.ReadAsStringAsync();

                    return new ApiClientResult(response, r.Result);
                }
            }
        }

        public async Task<ApiClientResult> PostMultipartFormData(string route, params FormDataContent[] formDataContents)
        {
            using (var cliente = GetHttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {                    
                    foreach(var c in formDataContents)
                    {
                        if (String.IsNullOrEmpty(c.Name))
                        {
                            content.Add(c.Content, c.Name);
                        }
                        else
                        {
                            content.Add(c.Content, c.Name, c.FileName);
                        }                        
                    }

                    var response = await cliente.PostAsync($"{_UrlBase}{route}", content);

                    var r = response.Content?.ReadAsStringAsync();

                    return new ApiClientResult(response, r.Result);
                }
            }
        }

        public async Task<ApiClientResult> PostMultipartFormData(string route, Action<MultipartFormDataContent> content)
        {
            using(var cliente = GetHttpClient())
            {
                using(var formDataContent = new MultipartFormDataContent())
                {
                    content(formDataContent);

                    var response = await cliente.PostAsync($"{_UrlBase}{route}", formDataContent);

                    var r = response.Content?.ReadAsStringAsync();

                    return new ApiClientResult(response, r.Result);
                }
            }
        }

        public static async Task<ApiClientResult> Post(string route, object data, Action<HttpRequestHeaders> actionHeaders = null, Action<HttpContentHeaders> contentHeaders = null)
        {
            var client = new ApiClient("");

            if (actionHeaders != null)
            {
                client.SetRequestHeaders(actionHeaders);
            }

            if (contentHeaders != null)
            {
                client.SetContentHeaders(contentHeaders);
            }

            return await client.Post(route, data);
        }

        public static async Task<ApiClientResult<T>> Post<T>(string route, object data, Action<HttpRequestHeaders> actionHeaders = null, Action<HttpContentHeaders> contentHeaders = null)
        {
            var client = new ApiClient("");

            if (actionHeaders != null)
            {
                client.SetRequestHeaders(actionHeaders);
            }

            if (contentHeaders != null)
            {
                client.SetContentHeaders(contentHeaders);
            }

            return await client.Post<T>(route, data);
        }

        public async Task<ApiClientResult> Delete(string route)
        {
            using (var client = GetHttpClient())
            {
                var response = await client.DeleteAsync($"{_UrlBase}{route}");

                var json = await response.Content.ReadAsStringAsync();

                return new ApiClientResult(response, json);
            }
        }

        public static async Task<ApiClientResult> Delete(string route, Action<HttpRequestHeaders> actionHeaders = null, Action<HttpContentHeaders> contentHeaders = null)
        {
            var client = new ApiClient("");

            if (actionHeaders != null)
            {
                client.SetRequestHeaders(actionHeaders);
            }

            if (contentHeaders != null)
            {
                client.SetContentHeaders(contentHeaders);
            }

            return await client.Delete(route);
        }
        public async Task<ApiClientResult> Put(string route, object data)
        {
            using (var client = GetHttpClient())
            {
                using (var content = GetByteContent(data))
                {
                    var response = await client.PutAsync($"{_UrlBase}{route}", content);

                    var json = await response.Content.ReadAsStringAsync();

                    return new ApiClientResult(response, json);
                }
            }
        }

        public static async Task<ApiClientResult> Put(string route, object data, Action<HttpRequestHeaders> actionHeaders = null, Action<HttpContentHeaders> contentHeaders = null)
        {
            var client = new ApiClient("");

            if (actionHeaders != null)
            {
                client.SetRequestHeaders(actionHeaders);
            }

            if (contentHeaders != null)
            {
                client.SetContentHeaders(contentHeaders);
            }

            return await client.Put(route, data);
        }
    }
}
