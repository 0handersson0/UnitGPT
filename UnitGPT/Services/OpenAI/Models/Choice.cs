namespace UnitGPT.Services.OpenAI.Models
{
    internal class Choice
    {
        public int index { get; set; }
        public Message message { get; set; }
        public string finish_reason { get; set; }
    }
}
