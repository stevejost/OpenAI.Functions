namespace OpenAI.Functions.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class GptFunctionParameterAttribute : Attribute
    {
        private string _description;

        public GptFunctionParameterAttribute(string description)
        {
            _description = description;
        }

        public string GetDescription()
        {


            return _description;
        }
    }
}