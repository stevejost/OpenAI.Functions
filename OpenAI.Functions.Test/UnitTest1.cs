using FluentAssertions;
using OpenAI.Functions.Attributes;

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

        class GetCurrentWeatherTest
        {
            [GptFunction("Get the current weather in a given location")]
            public int GetCurrentWeather([GptFunctionParameter("The city and state, e.g. San Francisco, CA")]string location)
            {
                return 100;
            }
        }
    }
}