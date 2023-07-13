using FluentAssertions;
using Newtonsoft.Json;
using OpenAI.Functions.Attributes;
using OpenAI.Functions.Model;

namespace OpenAI.Functions.Test
{
    public class FunctionParserTests
    {
        [Fact]
        public void GptFunctionsExist()
        {
            var mapper = new GptFunctionMapper();
            var wcTest = new GetCurrentWeatherTest();

            var functions = mapper.GetFunctionsFromType(wcTest.GetType());

            functions.Count.Should().Be(1);
        }

        [Fact]
        public void GptFunctionsCanBeSerialized()
        {
            var mapper = new GptFunctionMapper();
            var wcTest = new GetCurrentWeatherTest();

            var functions = mapper.GetFunctionJsonFromType(wcTest.GetType());
            
            functions.Should().NotBeNullOrEmpty();
        }
       
        [Fact]
        public void GptChatResponseDecodesProperly()
        {
            var mapper = new GptFunctionMapper();
            var wcTest = new GetCurrentWeatherTest();
            var wcType = typeof(GetCurrentWeatherTest);
            string json = "{\n  \"Location\": \"Sacramento, CA\"\n}";
            var result = mapper.GetChatResponseFromMethod(wcType, "GetCurrentWeather", json);   
           
            result.Should().Be("The current temperature in Sacramento, CA is 100");
        }

        [Fact]
        public void GptChatResponseDecodesProperlyWithoutType()
        {
            var mapper = new GptFunctionMapper();            
            string json = "{\n  \"Location\": \"Sacramento, CA\"\n}";
            var type = mapper.FindTypeByMethodName("GetCurrentWeather");
            type.Should().Be(typeof(GetCurrentWeatherTest));

            var result = mapper.GetChatResponseFromMethod(type, "GetCurrentWeather", json);

            result.Should().Be("The current temperature in Sacramento, CA is 100");
        }

        [GptClass]
        class GetCurrentWeatherTest
        {           
            [GptFunction("Get the current weather in a given location")]
            public static string GetCurrentWeather(GetWeatherArgs args)
            {
                int tempValue = 100;

                return $"The current temperature in {args.Location} is {tempValue}";              
            }
        }

        class GetWeatherArgs : GptRequestArguments
        {
            [GptParameter("The city and state, e.g. San Francisco, CA")]
            public string Location { get; set; }
        }
    }
}