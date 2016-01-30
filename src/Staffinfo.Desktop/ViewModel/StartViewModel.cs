using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Staffinfo.Desktop.Data;
using Staffinfo.Desktop.Data.DataTableProviders;
using Staffinfo.Desktop.View;
using System;
using System.Windows;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class StartViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public StartViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            try
            {
                DataSingleton.Instance.DataInitialize();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #region GoToAllEmployeesView command
        private RelayCommand _goToAllEmployeesView;

        public RelayCommand GoToAllEmployeesView => _goToAllEmployeesView ?? (_goToAllEmployeesView = new RelayCommand(GoToAllEmployeesViewExecute));

        private void GoToAllEmployeesViewExecute()
        {
            var allEmployeesView = new AllEmployeesView();
            allEmployeesView.Show();
        }
        #endregion

        /// <summary>
        /// TODO: нужно использовать DialogCloser
        /// </summary>
        #region CloseApp command

        private RelayCommand _closeApp;

        public RelayCommand CloseApp
        {
            get { return _closeApp ?? (_closeApp = new RelayCommand(CloseAppExecute)); }
        }

        private void CloseAppExecute()
        {

        }

        #endregion

    }
}