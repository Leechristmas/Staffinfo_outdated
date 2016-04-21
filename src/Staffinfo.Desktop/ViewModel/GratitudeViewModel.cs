using System.ComponentModel;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// view model благодарности
    /// </summary>
    public class GratitudeViewModel
    {
        /// <summary>
        /// модель
        /// </summary>
        private GratitudeModel _gratitude;

        public GratitudeViewModel(GratitudeModel gratitude)
        {
            _gratitude = gratitude;
        }

        /// <summary>
        /// Дата вынесения благодарности
        /// </summary>
        [DisplayName(@"Дата вынесения благодарности")]
        public string GratitudeDate => _gratitude.GratitudeDate.ToString("d");

        /// <summary>
        /// Описание
        /// </summary>
        [DisplayName(@"Описание")]
        public string Description => _gratitude.Description;

        /// <summary>
        /// Возвращает модель
        /// </summary>
        /// <returns></returns>
        public GratitudeModel GetModel()
        {
            return _gratitude;
        }
    }
}