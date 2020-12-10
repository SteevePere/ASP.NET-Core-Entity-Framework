using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace App.Services
{

    public class HttpService
    {

        private HttpClient _httpClient;
        private HttpContext _context;


        public HttpService(IHttpContextAccessor httpContextAccessor)
        {
            _context = httpContextAccessor.HttpContext;
            _httpClient = new HttpClient();

            var baseAddress = new Uri(Environment.GetEnvironmentVariable("DOCNET_API_URL"));
            _httpClient.BaseAddress = baseAddress;
        }


        private void SetBearer()
        {
            try
            {
                var accessToken = _context.Session.GetString("accessToken");

                if (accessToken != null)
                {
                    _httpClient.DefaultRequestHeaders.Authorization 
                        = new AuthenticationHeaderValue("Bearer", accessToken);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        public bool isBearer()
        {
            var accessToken = _context.Session.GetString("accessToken");
            if (accessToken != null) return true;
            return false;
        }
        

        public async Task<string> Get(string url)
        {
            SetBearer();
            var response = await this._httpClient.GetStringAsync(url);
            return response;
        }
        

        public async Task<HttpResponseMessage> Post(string url, string body)
        {
            SetBearer();
            var response = await this._httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
            return response;
        }
        

        public async Task<HttpResponseMessage> Put(string url, string body)
        {
            SetBearer();
            var response = await this._httpClient.PutAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
            return response;
        }

        public async Task<string> Delete(string url)
        {
            SetBearer();
            var response = await this._httpClient.DeleteAsync(url);
            return response.ToString();   
        }
        
        public async Task<HttpResponseMessage> GetRequest(string url)
        {
            SetBearer();
            var response = await this._httpClient.GetAsync(url);
            return response;
        }
    }
}