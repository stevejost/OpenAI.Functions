using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OpenAI.API.Model;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OpenAI.API
{
    public class ApiClient
    {
        const string DEFAULT_BASE_URL = "https://api.openai.com/v1/";

        private readonly string _apiKey;
        private readonly RestClient _client;
        private readonly string _baseUrl;

        public ApiClient(string apiKey)
        {
            this._apiKey = apiKey;
            this._baseUrl = DEFAULT_BASE_URL;

            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            this._client = new RestClient(_baseUrl, configureSerialization: s=>s.UseNewtonsoftJson(new Newtonsoft.Json.JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                NullValueHandling = NullValueHandling.Ignore
            }));            
            
        }

        public ApiClient(string apiKey, string baseUrl)
        {
            this._apiKey = apiKey;
            this._baseUrl = baseUrl;

            this._client = new RestClient(_baseUrl);
        }

        public async Task<ChatResponse?> SendChatRequestAsync(ChatRequest chatRequest)
        {
            var req = new RestRequest("chat/completions");
            req.AddBody(chatRequest);            
            req.AddHeader("Content-Type", "application/json");
            req.AddHeader("Authorization", $"Bearer {this._apiKey}");

            //var testRequest = JsonConvert.SerializeObject(chatRequest);

            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            var testRequest = JsonConvert.SerializeObject(chatRequest, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                
            });

            var response = await this._client.PostAsync<ChatResponse>(req);

            if(response != null)
            {
                return response;
            }

            return null;
        }
        
    }
}