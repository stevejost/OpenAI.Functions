using Microsoft.Extensions.Configuration;
using OpenAI.API;
using OpenAI.API.Model;
using OpenAI.Functions;

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

            var mapper = new GptFunctionMapper();
            

            var baseChatGptReq = new ChatRequest()
            {
                Model = "gpt-3.5-turbo-16k",
                Functions = mapper.GetFunctionsFromType(typeof(ExampleLogicClass))
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
                    if(!string.IsNullOrEmpty(choice.Message.Content))
                    {
                        System.Console.WriteLine($"{choice.Message.Role}: {choice.Message.Content}");
                    }
                    
                    if(choice.Message.FunctionCall != null)
                    {
                        var response = mapper.GetChatResponseFromMethod(choice.Message.FunctionCall.Name, choice.Message.FunctionCall.Arguments);
                        System.Console.WriteLine($"Function Response ({choice.Message.FunctionCall.Name}): {response}"); 
                    }
                    
                }
                System.Console.WriteLine();
                baseChatGptReq.Messages.Clear();
                // TODO: Process user input here.
                
            }
        }
    }
}