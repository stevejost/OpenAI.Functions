using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Functions.Model
{
    public class GptFunctionCall
    {
        public string Name { get; set; }
        public string Arguments { get; set; }
    }
}
