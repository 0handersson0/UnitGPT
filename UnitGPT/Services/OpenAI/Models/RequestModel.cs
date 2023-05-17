namespace UnitGPT.Services.OpenAI.Models
{
    internal class RequestModel
    {
        public string model { get; set; }
        public Message[] messages { get; set; }
    }
}
