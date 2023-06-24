using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace ChatGptDekstop.Model
{
    public class Request
    {
        [JsonPropertyName("model")]
        public string ModelId { get; set; } = "";
        [JsonPropertyName("messages")]
        public ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>();
    }
}
