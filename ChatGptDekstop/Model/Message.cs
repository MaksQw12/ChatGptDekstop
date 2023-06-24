using ChatGptDekstop.ViewModel;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ChatGptDekstop.Model
{
    public class Message
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = "";
        [JsonPropertyName("content")]
        public string Content { get; set; } = "";
        public override string ToString()
        {
            return Content;
        }

       
    }
}
