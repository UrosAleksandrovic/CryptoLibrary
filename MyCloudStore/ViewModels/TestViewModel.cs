using System.ComponentModel;

/// <summary>
/// Test primer za ViewModel
/// </summary>
namespace MyCloudStore.ViewModels
{
    public class TestViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string mTest;
        public string Test
        {
            get => mTest;
            set
            {
                if (mTest == null)
                    return;

                mTest = value;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Test"));
            }
        }
    }
}
