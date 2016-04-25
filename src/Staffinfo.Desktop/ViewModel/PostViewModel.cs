using System.ComponentModel;
using System.Runtime.CompilerServices;
using Staffinfo.Desktop.Annotations;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    public class PostViewModel: INotifyPropertyChanged
    {
        private bool _isSelected;

        public PostViewModel(PostModel postModel)
        {
            PostTitle = postModel.PostTitle;
            PostId = postModel.Id;
            ServiceId = postModel.ServiceId;
        }

        public long? ServiceId { get; set; }
        public long? PostId { get; set; }
        public string PostTitle { get; set; }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}