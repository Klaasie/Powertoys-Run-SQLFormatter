using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Headers;

namespace Klaasie.Sf
{
    public class Response
    {
        public string? Result { get; set; }
    }

    public class ApiClient
    {
        static readonly HttpClient client;

        static ApiClient()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://sqlformat.org/api/v1/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static async Task<Response> Format(string sql)
        {

            var query = new Dictionary<string, string>()
            {
                ["reindent"] = "1",
                ["sql"] = sql
            };

            Response response = new();

            var uri = QueryHelpers.AddQueryString("format", query);
            HttpResponseMessage responseMessage = await client.GetAsync(uri);

            if (responseMessage.IsSuccessStatusCode)
            {
                response = await responseMessage.Content.ReadAsAsync<Response>();
            }

            return response;
        }
    }
}
