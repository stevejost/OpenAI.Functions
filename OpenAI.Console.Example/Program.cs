using Microsoft.Extensions.Configuration;
using OpenAI.API;
using OpenAI.API.Model;

namespace OpenAI.Console.Example
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<Program>();
            var config = builder.Build();

            var section = config.GetSection("OpenAI");
            var aiConfig = section.Get<OpenAIConfiguration>();

            var client = new ApiClient(aiConfig.ApiKey);
            string userInput = string.Empty;

            var baseChatGptReq = new ChatRequest()
            {
                Model = "gpt-3.5-turbo-16k"                
            };

            while (userInput.ToLower() != "bye")
            {
                
                System.Console.Write("Please enter a line of chat (or 'bye' to stop): ");
                userInput = System.Console.ReadLine();

                if(userInput.ToLower() == "bye")
                {
                    break;
                }

                baseChatGptReq.Messages.Add(new ChatMessage() { Role = "user", Content = userInput });
                var chatResponse = await client.SendChatRequestAsync(baseChatGptReq);

                foreach(var choice in chatResponse.Choices)
                {
                    System.Console.WriteLine($"{choice.Message.Role}: {choice.Message.Content}");
                }
                // TODO: Process user input here.
                
            }
        }
    }
}