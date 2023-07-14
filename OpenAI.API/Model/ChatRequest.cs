using OpenAI.Functions.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.API.Model
{
    public class ChatRequest
    {
        public string Model { get; set; }
        public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
        public List<GptFunction> Functions { get; set; }
        
    }
}
