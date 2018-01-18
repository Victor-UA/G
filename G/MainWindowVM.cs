using Prism.Commands;
using Prism.Mvvm;

namespace G
{    
    public class MainWindowVM : BindableBase
    {
        /// <summary>
        /// Initializes a new instance of the MainWindowViewModel class.
        /// </summary>
        public MainWindowVM()
        {
            Player_Move = new DelegateCommand<byte>(
                direction => { });
        }

        public DelegateCommand<byte> Player_Move;
    }
}