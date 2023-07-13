using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Functions.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class GptFunctionAttribute : Attribute
    {
        private string _description;
        

        public GptFunctionAttribute(string description)
        {
            _description = description;
        }

        public string GetDescription()
        {

            
            return _description;
        }
    }
}
