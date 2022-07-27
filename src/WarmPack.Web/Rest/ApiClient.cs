using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

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

                var rou = _UrlBase + Uri.EscapeUriString(route);
                var response = await client.GetAsync(new Uri(rou));

                var json = await response.Content.ReadAsStringAsync();                

                return new ApiClientResult(response, json);
            }
        }

        public async Task<ApiClientResult<T>> Get<T>(string route)
        {
            using (var client = GetHttpClient())
            {

                var rou = _UrlBase + Uri.EscapeUriString(route);
                var response = await client.GetAsync(new Uri(rou));

                var json = await response.Content.ReadAsStringAsync();

                return new ApiClientResult<T>(response, json);
            }
        }

        public ApiClientResult GetSync(string route)
        {
            ApiClientResult result = null;
            AutoResetEvent waitHandle = new AutoResetEvent(false);

            Task.Run(async () =>
            {
                result = await Get(route);

                waitHandle.Set();
            });

            waitHandle.WaitOne();

            return result;
        }

        public ApiClientResult GetSync(string route, Action<HttpRequestHeaders> actionHeaders = null, Action<HttpContentHeaders> contentHeaders = null)
        {
            ApiClientResult result = null;
            AutoResetEvent waitHandle = new AutoResetEvent(false);

            Task.Run(async () =>
            {
                result = await Get(route, actionHeaders, contentHeaders);

                waitHandle.Set();
            });

            waitHandle.WaitOne();

            return result;
        }

        public ApiClientResult<T> GetSync<T>(string route)
        {
            ApiClientResult<T> result = null;
            AutoResetEvent waitHandle = new AutoResetEvent(false);

            Task.Run(async () =>
            {
                result = await Get<T>(route);

                waitHandle.Set();
            });

            waitHandle.WaitOne();

            return result;
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

        public static async Task<ApiClientResult<T>> Get<T>(string route, Action<HttpRequestHeaders> actionHeaders = null, Action<HttpContentHeaders> contentHeaders = null)
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

            return await client.Get<T>(route);
        }

        public ApiClientResult<T> GetSync<T>(string route, Action<HttpRequestHeaders> actionHeaders = null, Action<HttpContentHeaders> contentHeaders = null)
        {
            ApiClientResult<T> result = null;
            AutoResetEvent waitHandle = new AutoResetEvent(false);

            Task.Run(async () =>
            {
                result = await Get<T>(route, actionHeaders, contentHeaders);

                waitHandle.Set();
            });

            waitHandle.WaitOne();

            return result;
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

        public ApiClientResult PostSync(string route, object data)
        {
            ApiClientResult result = null;
            AutoResetEvent waitHandle = new AutoResetEvent(false);

            Task.Run(async () =>
            {
                result = await Post(route, data);

                waitHandle.Set();
            });

            waitHandle.WaitOne();

            return result;
        }

        public ApiClientResult<T> PostSync<T>(string route, object data)
        {
            ApiClientResult<T> result = null;
            AutoResetEvent waitHandle = new AutoResetEvent(false);

            Task.Run(async () =>
            {
                result = await Post<T>(route, data);

                waitHandle.Set();
            });

            waitHandle.WaitOne();

            return result;
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

        public async Task<ApiClientResult<T>> PostFormUrlEncoded<T>(string route, IEnumerable<KeyValuePair<string, string>> data)
        {
            using (var cliente = GetHttpClient())
            {
                using (var content = new FormUrlEncodedContent(data))
                {
                    content.Headers.Clear();
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");


                    var response = await cliente.PostAsync($"{_UrlBase}{route}", content);

                    var r = response.Content?.ReadAsStringAsync();

                    return new ApiClientResult<T>(response, r.Result);
                }
            }
        }

        public ApiClientResult PostFormUrlEncodedSync(string route, IEnumerable<KeyValuePair<string, string>> data)
        {
            ApiClientResult result = null;
            AutoResetEvent waitHandle = new AutoResetEvent(false);

            Task.Run(async () =>
            {
                result = await PostFormUrlEncoded(route, data);

                waitHandle.Set();
            });

            waitHandle.WaitOne();

            return result;
        }

        public ApiClientResult<T> PostFormUrlEncodedSync<T>(string route, IEnumerable<KeyValuePair<string, string>> data)
        {
            ApiClientResult<T> result = null;
            AutoResetEvent waitHandle = new AutoResetEvent(false);

            Task.Run(async () =>
            {
                result = await PostFormUrlEncoded<T>(route, data);

                waitHandle.Set();
            });

            waitHandle.WaitOne();

            return result;
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

        public ApiClientResult<T> PostSync<T>(string route, object data, Action<HttpRequestHeaders> actionHeaders = null, Action<HttpContentHeaders> contentHeaders = null)
        {
            ApiClientResult<T> result = null;
            AutoResetEvent waitHandle = new AutoResetEvent(false);

            Task.Run(async () =>
            {
                result = await Post<T>(route, data, actionHeaders, contentHeaders);

                waitHandle.Set();
            });

            waitHandle.WaitOne();

            return result;
        }

        public ApiClientResult PostSync(string route, object data, Action<HttpRequestHeaders> actionHeaders = null, Action<HttpContentHeaders> contentHeaders = null)
        {
            ApiClientResult result = null;
            AutoResetEvent waitHandle = new AutoResetEvent(false);

            Task.Run(async () =>
            {
                result = await Post(route, data, actionHeaders, contentHeaders);

                waitHandle.Set();
            });

            waitHandle.WaitOne();

            return result;
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

        public ApiClientResult DeleteSync(string route)
        {
            ApiClientResult result = null;
            AutoResetEvent waitHandle = new AutoResetEvent(false);

            Task.Run(async () =>
            {
                result = await Delete(route);

                waitHandle.Set();
            });

            waitHandle.WaitOne();

            return result;
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

        public ApiClientResult PutSync(string route, object data)
        {
            ApiClientResult result = null;
            AutoResetEvent waitHandle = new AutoResetEvent(false);

            Task.Run(async () =>
            {
                result = await Put(route, data);

                waitHandle.Set();
            });

            waitHandle.WaitOne();

            return result;
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
