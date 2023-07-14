using OpenAI.Functions.Attributes;
using OpenAI.Functions.Model;

namespace OpenAI.Console.Example
{
    [GptClass]
    public class WeatherInformationReq : GptRequestArguments
    {
        [GptParameter("The location to get the weather for")]
        public string Location { get; set; }
    }
}