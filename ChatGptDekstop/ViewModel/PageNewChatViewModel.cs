using ChatGptDekstop.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Net.Http.Json;
using System.IO;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;

namespace ChatGptDekstop.ViewModel
{
    public class PageNewChatViewModel : BaseViewModel
    {
        private List<Message> temporaryMessages = new List<Message>();
        public Chat Chat { get; }
        private bool isGenerating = false;
        private readonly string url = "https://api.openai.com/v1/chat/completions";
        private ObservableCollection<Message> _messages;
        public ObservableCollection<Message> Messagess
        {
            get => _messages;
            set
            {
                _messages = value;
                OnPropertyChanged(nameof(Messagess));
            }
        }

        private string inputUser;
        public string InputUser
        {
            get => inputUser;
            set
            {
                inputUser = value;
                OnPropertyChanged(nameof(InputUser));
            }
        }
        public ICommand Response { get; }

 
        public PageNewChatViewModel(Chat chat)
        {
            Chat = chat;
            Messagess = new ObservableCollection<Message>();
            Response = new RelayCommand(async () => await ResponseData());
        }

        public void LoadMessages()
        {
            foreach (var message in temporaryMessages)
            {
                Messagess.Add(message);
            }
        }

        private string GetApiKey()
        {
            using StreamReader reader = new StreamReader("resourse.json");
            string jsonResult = reader.ReadToEnd();
            var result = JsonConvert.DeserializeObject<JObject>(jsonResult);
            return result.GetValue("apiKey").ToString();
        }
 

        public async Task ResponseData()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetApiKey()}");

            if (isGenerating || string.IsNullOrWhiteSpace(InputUser))
                return;

            isGenerating = true;
            var userMessages = SplitMessage(InputUser);
            InputUser = "";

            await GenerateResponses(userMessages, httpClient);


            await Task.Delay(100);
            isGenerating = false;
        }

        private async Task GenerateResponses(List<string> messages, HttpClient httpClient)
        {
            foreach (var userMessage in messages)
            {
                Messagess.Add(new Message { Role = "user", Content = userMessage });
                OnPropertyChanged(nameof(Messagess));

                var requestData = new Request
                {
                    ModelId = "gpt-3.5-turbo",
                    Messages = new ObservableCollection<Message>(Messagess.ToList())
                };

                var response = await httpClient.PostAsJsonAsync(url, requestData);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Проблема сервера", "Ошибка");
                    return;
                }

                var responseData = await response.Content.ReadFromJsonAsync<ResponseData>();

                if (responseData?.Choices?.Count > 0)
                {
                    var choice = responseData.Choices[0];
                    var responseMessage = choice.Message;

                    var responseText = responseMessage.Content.Trim();

                    Messagess.Add(responseMessage);
                    OnPropertyChanged(nameof(Messagess));
                }
            }
        }


        public static List<string> SplitMessage(string userInput)
        {
            const int MaxMessageLength = 4096;

            if (userInput.Length <= MaxMessageLength)
                return new List<string> { userInput };

            var messages = new List<string>();
            var currentMessage = "";

            foreach (var character in userInput)
            {
                if (currentMessage.Length + 1 <= MaxMessageLength)
                {
                    currentMessage += character;
                }
                else
                {
                    messages.Add(currentMessage);
                    currentMessage = character.ToString();
                }
            }

            if (!string.IsNullOrEmpty(currentMessage))
                messages.Add(currentMessage);

            return messages;
        }
    }
}
