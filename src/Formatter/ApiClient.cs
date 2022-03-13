using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Klaasie.Sf
{
    public class Response
    {
        public string? Result { get; set; }
        public string? Error { get; set; }

        public static implicit operator Task<object>(Response v)
        {
            throw new NotImplementedException();
        }
    }

    public class ApiClient
    {
        static readonly HttpClient client;

        static ApiClient()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri("https://sqlformat.org/api/v1/")
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static async Task<Response> Format(string sql)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("reindent", "1"),
                new KeyValuePair<string, string>("sql", sql)
            });

            HttpResponseMessage responseMessage = await client.PostAsync("format", formContent);

            Response response = new(); 
            if (responseMessage.IsSuccessStatusCode)
            {
                #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                response = await responseMessage.Content.ReadFromJsonAsync<Response>();
                #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            } else
            {
                response.Error = responseMessage.ReasonPhrase;
            }

            #pragma warning disable CS8603 // Possible null reference return.
            return response;
            #pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
