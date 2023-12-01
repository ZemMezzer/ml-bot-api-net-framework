using System;
using System.Net.Http;
using System.Threading.Tasks;
using MLBotApiNetFramework.Api.TextGeneration.Data;
using Newtonsoft.Json.Linq;

namespace MLBotApiNetFramework.Api.TextGeneration
{
    public class TextGenerationWebRequest
    {
        private readonly HttpClient _client = new();

        public TextGenerationWebRequest()
        {
            _client.Timeout = new TimeSpan(0, 10, 0);
        }
        
        public async Task<string> Send(ApiRequest request)
        {
            using (HttpContent content = new StringContent(request.RequestData.ToJson()))
            {
                var response = await _client.PostAsync(request.Bot.Config.ApiUrl, content);

                if (!response.IsSuccessStatusCode)
                    return string.Empty;

                var jsonTask = response.Content.ReadAsStringAsync();
                JObject result = JObject.Parse(jsonTask.Result);

                response.Dispose();
                
                var parsedResult = result.Root["results"].First["history"]["visible"]?.Last.Last.ToString();

                var formattedResult = System.Net.WebUtility.HtmlDecode(parsedResult);
                return formattedResult;
            }
        }
    }
}