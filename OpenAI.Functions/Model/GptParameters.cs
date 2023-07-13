using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Functions.Model
{
    public class GptParameters
    {
        public string Type => "object";
        public Dictionary<string, GptParameterProperties> Properties { get; set; } = new Dictionary<string, GptParameterProperties>();
    }
}
