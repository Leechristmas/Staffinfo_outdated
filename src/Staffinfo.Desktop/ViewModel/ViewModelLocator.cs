/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Staffinfo"
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
            SimpleIoc.Default.Register<EmployeeViewModel>();
            SimpleIoc.Default.Register<EmployeesViewModel>();
            SimpleIoc.Default.Register<AddNewEmployeeViewModel>();
        }

        public StartViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<StartViewModel>();
            }
        }
        
        public EmployeeViewModel Employee
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EmployeeViewModel>();
            }
        }

        public EmployeesViewModel Employees
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EmployeesViewModel>();
                
            }
        }

        public AddNewEmployeeViewModel AddEmployee
        {
            get { return ServiceLocator.Current.GetInstance<AddNewEmployeeViewModel>(); }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}