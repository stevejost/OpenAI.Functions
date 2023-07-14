using OpenAI.Functions.Attributes;
using OpenAI.Functions.Model;

namespace OpenAI.Console.Example
{
    public class IpInformationReq : GptRequestArguments
    {
        [GptParameter("IP address to look up information on")]
        public string IpAddress { get; set; }
    }
}