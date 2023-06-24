using System.ComponentModel;


namespace ChatGptDekstop.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string propname)
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propname));
        }
    }
}
