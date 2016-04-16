/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Staffinfo.Desktop"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<StartViewModel>();
            SimpleIoc.Default.Register<AllEmployeesViewModel>();
            SimpleIoc.Default.Register<AddNewEmployeeViewModel>();
            SimpleIoc.Default.Register<ReportsViewModel>();
            SimpleIoc.Default.Register<AddNewMilitaryUnitViewModel>();
            SimpleIoc.Default.Register<AddNewEducationalInstitutionViewModel>();
            SimpleIoc.Default.Register<AddNewSpecialityViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
        }

        /// <summary>
        /// ���������
        /// </summary>
        public SettingsViewModel Settings => ServiceLocator.Current.GetInstance<SettingsViewModel>();

        /// <summary>
        /// ������
        /// </summary>
        public ReportsViewModel Reports => ServiceLocator.Current.GetInstance<ReportsViewModel>();

        /// <summary>
        /// ���������� ����������
        /// </summary>
        public AddNewEmployeeViewModel AddNewEmploye => ServiceLocator.Current.GetInstance<AddNewEmployeeViewModel>();

        /// <summary>
        /// ��� ����������
        /// </summary>
        public AllEmployeesViewModel AllEmployee => ServiceLocator.Current.GetInstance<AllEmployeesViewModel>();

        /// <summary>
        /// Start-view
        /// </summary>
        public StartViewModel Main => ServiceLocator.Current.GetInstance<StartViewModel>();

        /// <summary>
        /// ���������� �������� �����
        /// </summary>
        public AddNewMilitaryUnitViewModel AddMilitaryUnit
            => ServiceLocator.Current.GetInstance<AddNewMilitaryUnitViewModel>();

        /// <summary>
        /// ���������� �������� ���������
        /// </summary>
        public AddNewEducationalInstitutionViewModel AddNewEducationalUnit
            => ServiceLocator.Current.GetInstance<AddNewEducationalInstitutionViewModel>();

        /// <summary>
        /// ���������� �������������
        /// </summary>
        public AddNewSpecialityViewModel AddNewSpeciality
            => ServiceLocator.Current.GetInstance<AddNewSpecialityViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}