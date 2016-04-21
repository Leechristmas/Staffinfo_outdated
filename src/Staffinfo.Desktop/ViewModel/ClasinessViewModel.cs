using System;
using System.ComponentModel;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.ViewModel
{
    /// <summary>
    /// View model для классности
    /// </summary>
    public class ClasinessViewModel
    {
        private ClasinessModel _clasiness;

        public ClasinessViewModel(ClasinessModel clasiness)
        {
            _clasiness = clasiness;
        }

        /// <summary>
        /// Номер приказа
        /// </summary>
        [DisplayName(@"Номер приказа")]
        public int OrderNumber => _clasiness.OrderNumber;

        /// <summary>
        /// Дата
        /// </summary>
        [DisplayName(@"Дата")]
        public string ClasinessDate => _clasiness.ClasinessDate.ToString("d");

        /// <summary>
        /// Уровень классности
        /// </summary>
        [DisplayName(@"Степень классности")]
        public int ClasinessLevel => _clasiness.ClasinessLevel;

        /// <summary>
        /// Описание
        /// </summary>
        [DisplayName(@"Описание")]
        public string Description => _clasiness.Description;

        /// <summary>
        /// Возвращает модель
        /// </summary>
        /// <returns></returns>
        public ClasinessModel GetModel()
        {
            return _clasiness;
        }

    }
}