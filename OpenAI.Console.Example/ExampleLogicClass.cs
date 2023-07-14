using OpenAI.Functions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Console.Example
{
    [GptClass]
    public class ExampleLogicClass
    {
        [GptFunction("Returns information about given ip addresses")]
        public static string GetIpInformation(IpInformationReq request)
        {
              return $"The IP address {request.IpAddress} has the following information: The ip is magical!";
        }

        [GptFunction("Returns current weather given a location")]
        public static string GetWeatherInformation(WeatherInformationReq request)
        {
            return $"The weather at {request.Location} is going to be a megastorm with a heat wave, coupled with massive flooding and torrential rain.  \nIt will be the end of the world.";
        }
    }
}
