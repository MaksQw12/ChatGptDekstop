using ChatGptDekstop.Model;
using ChatGptDekstop.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChatGptDekstop.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        private Dictionary<string, PageNewChatViewModel> chatViewModels = new Dictionary<string, PageNewChatViewModel>();
        private Chat _selectedChat;
        public Chat SelectedChat
        {
            get { return _selectedChat; }
            set
            {
                _selectedChat = value;
                OnPropertyChanged(nameof(SelectedChat));
            }
        }
        private ObservableCollection<Chat> _chats;
        public ObservableCollection<Chat> Chats
        {
            get { return _chats; }
            set
            {
                _chats = value;
                OnPropertyChanged(nameof(Chats));
            }
        }
        private Page page;
        public Page Page
        {
            get => page;
            set
            {
                page = value;
                OnPropertyChanged(nameof(page));
            }
        }
        public ICommand CreateNewChatCommand { get; }
        public ICommand OpenPageCommand { get; }

        public MainWindowViewModel()
        {
            NextPage();
            Chats = new ObservableCollection<Chat>();
            CreateNewChatCommand = new RelayCommand(CreateNewChat);
            OpenPageCommand = new RelayCommand(OpenNewChat);
        }

       
        private async Task CreateNewChat()
        {

          var _newChat = new Chat
            {
                Id = Guid.NewGuid().ToString(),
                Name = "New Chat"
            };
            Chats.Add(_newChat);

            var chatPageViewModel = new PageNewChatViewModel(_newChat);
            chatPageViewModel.LoadMessages(); 
             var chatPage = new PageNewChat();
            chatPage.DataContext = chatPageViewModel;

            var mainWindow = Application.Current.MainWindow as MainWindow;
            var frame = mainWindow.ContentFrame;
            frame.Content = chatPage;
        }

        private async Task OpenNewChat()
        {
            if (SelectedChat != null)
            {
                if (!chatViewModels.ContainsKey(SelectedChat.Id))
                {
                    var chatPageViewModel = new PageNewChatViewModel(SelectedChat);
                    chatViewModels.Add(SelectedChat.Id, chatPageViewModel);
                }

                var chatPage = new PageNewChat();
                chatPage.DataContext = chatViewModels[SelectedChat.Id];

                var mainWindow = Application.Current.MainWindow as MainWindow;
                var frame = mainWindow.ContentFrame;
                frame.Content = chatPage;
            }
        }

        public void NextPage()
        {
            Page = new MainPage();
        }
    }
}
