namespace OpenAI.API.Model
{
    public class ChatResponseChoices
    {
        public int Index { get; set; }
        public ChatMessage Message { get; set; }
        public string FinishReason { get; set; }
    }
}