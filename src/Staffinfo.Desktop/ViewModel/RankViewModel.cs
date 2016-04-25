﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using Staffinfo.Desktop.Annotations;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    public class RankViewModel: INotifyPropertyChanged
    {
        private bool _isSelected;

        public RankViewModel(RankModel rankModel)
        {
            RankTitle = rankModel.RankTitle;
            RankId = rankModel.Id;
        } 

        public long? RankId { get; set; }
        public string RankTitle { get; set; }

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