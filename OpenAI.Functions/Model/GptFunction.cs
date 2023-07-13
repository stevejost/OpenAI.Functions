using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Functions.Model
{
    public class GptFunction
    {        
        public string Name { get; set; }
        public GptParameters Parameters { get; set; } = new GptParameters();
        public List<string> Required { get; set; }
        public string Description { get; internal set; }
    }
}
