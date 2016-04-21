using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Controls;
using MahApps.Metro.Controls;

namespace Staffinfo.Desktop.View
{
    /// <summary>
    /// Interaction logic for EmployeeView.xaml
    /// </summary>
    public partial class EmployeeView : MetroWindow
    {
        public EmployeeView()
        {
            InitializeComponent();
        }



        /// <summary>
        /// TODO: говнокод FOUND!!! перенести эту обработку во view model
        /// Event handler for when columns are added to the data grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AutoGenerationColumnExecute(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

            string displayName = GetPropertyDisplayName(e.PropertyDescriptor);
            if (!string.IsNullOrEmpty(displayName))
            {
                e.Column.Header = displayName;
            }

        }

        /// <summary>
        /// Gets the Display Name for the property descriptor passed in
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        public static string GetPropertyDisplayName(object descriptor)
        {

            PropertyDescriptor pd = descriptor as PropertyDescriptor;
            if (pd != null)
            {
                // Check for DisplayName attribute and set the column header accordingly
                DisplayNameAttribute displayName = pd.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute;
                if (displayName != null && displayName != DisplayNameAttribute.Default)
                {
                    return displayName.DisplayName;
                }

            }
            else
            {
                PropertyInfo pi = descriptor as PropertyInfo;
                if (pi != null)
                {
                    // Check for DisplayName attribute and set the column header accordingly
                    Object[] attributes = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                    for (int i = 0; i < attributes.Length; ++i)
                    {
                        DisplayNameAttribute displayName = attributes[i] as DisplayNameAttribute;
                        if (displayName != null && displayName != DisplayNameAttribute.Default)
                        {
                            return displayName.DisplayName;
                        }
                    }
                }
            }
            return null;
        }

    }
}
