using System.Collections.ObjectModel;
using System.Linq;
using Staffinfo.Desktop.Data.DataTableProviders;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// Все пользователи
    /// </summary>
    public class AllUsersViewModel
    {
        public AllUsersViewModel()
        {
            using (var prvdr = new UserTableProvider())
            {
                UserList = new ObservableCollectionViewModel<UserViewModel>(
                    new ObservableCollection<UserViewModel>(prvdr
                    .Select()
                    .Select(user => new UserViewModel(user))));
            }
        }

        public /*readonly*/ ObservableCollectionViewModel<UserViewModel> UserList { get; set; }
    }
}