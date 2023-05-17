namespace UnitGPT.Services.OpenAI.Models
{
    internal class Response
    {
        public string id { get; set; }
        public string _object { get; set; }
        public int created { get; set; }
        public Choice[] choices { get; set; }
        public Usage usage { get; set; }
    }

}
