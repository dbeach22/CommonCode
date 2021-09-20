using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FatHead.PropertyChanged
{
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// This method is called by the Set accessor of each property.
        /// </summary>
        /// <param name="propertyName">The CallerMemberName attribute that is applied to the optional propertyName</param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
