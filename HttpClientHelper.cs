using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace nUnitPlaywrightAPI
{
    internal class HttpClientHelper
    {
        private readonly HttpClient _client;

        public HttpClientHelper()
        {
            _client = new HttpClient();
        }

#pragma warning disable CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
        public async Task<HttpResponseMessage> MakeRequestAsync(string url, HttpMethod method = null, HttpContent content = null)
#pragma warning restore CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
        {
            var request = new HttpRequestMessage(method ?? HttpMethod.Get, url) { Content = content };
            return await _client.SendAsync(request);
        }
    }
}
