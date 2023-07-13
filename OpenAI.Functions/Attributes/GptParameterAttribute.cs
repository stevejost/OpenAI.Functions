using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Functions.Attributes
{
    public class GptParameterAttribute : Attribute
    {
        private string _description;

        public GptParameterAttribute(string description)
        {
            _description = description;
        }

        public string GetDescription()
        {


            return _description;
        }
    }
}
