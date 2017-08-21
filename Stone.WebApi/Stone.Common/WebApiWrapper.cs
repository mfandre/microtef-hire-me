using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace Stone.Common
{
    public class WebApiWrapper<T>
    {
        HttpClient client = new HttpClient();

        public WebApiWrapper(string webApiUrl)
        {

            client.BaseAddress = new Uri(webApiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<T> Get(string url)
        {
            T o = default(T);
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                o = await response.Content.ReadAsAsync<T>();
            }
            return o;
        }

        public async Task<T> CreateAsync(string url, object o)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(url, o);
            response.EnsureSuccessStatusCode();

            T result = default(T);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<T>();
            }
            return result;
        }

        public async Task<T> UpdateAsync(string url, object o)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(url, o);
            response.EnsureSuccessStatusCode();

            T result = default(T);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<T>();
            }
            return result;
        }

        public async Task<T> DeleteAsync(string url, string id)
        {
            HttpResponseMessage response = await client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();

            T result = default(T);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<T>();
            }
            return result;
        }
    }
}