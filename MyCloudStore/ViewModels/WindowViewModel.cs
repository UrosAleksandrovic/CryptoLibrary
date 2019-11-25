using MyCloudStore.Models;
using System.ComponentModel;
using System.Windows;

namespace MyCloudStore.ViewModels
{
    /// <summary>
    /// View model class for main window
    /// </summary>
    public class WindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Private members

        /// <summary>
        /// The window this view model controls
        /// </summary>
        private Window _window;

        /// <summary>
        /// Current page showed in main window
        /// </summary>
        private ApplicationPage _currentPage;


        #endregion

        #region Public properties

        public ApplicationPage CurrentPage
        {
            get => _currentPage;

            set
            {
                if (value == _currentPage)
                    return;
                _currentPage= value;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("CurrentPage"));
            }
        }
        #endregion

        #region Constructor

        /// <summary>
        /// Main constructor of Window view model
        /// </summary>
        /// <param name="Window"></param>
        public WindowViewModel(Window Window)
        {
            _window = Window;
        }


        #endregion
    }
}
