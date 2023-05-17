namespace UnitGPT.Services.OpenAI.Models
{
    public class Error
    {
        public string message { get; set; }
        public string type { get; set; }
        public object param { get; set; }
        public object code { get; set; }
    }
}
