namespace OpenAI.API.Model
{
    public class ChatMessage
    {
        public string Role { get; set; }
        public string Content { get; set; } 
        public ChatFunctionCall FunctionCall { get; set; }
    }
}