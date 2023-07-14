using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.API.Model
{
    public class ChatResponse
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public long Created { get; set; }
        public string Model { get; set; }
        public List<ChatResponseChoices> Choices { get; set; }
        public ChatUsage Usage { get; set; }

    }
}
