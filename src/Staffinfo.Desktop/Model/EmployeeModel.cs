using System;
using System.Windows.Media.Imaging;

namespace Staffinfo.Desktop.Model
{
    /// <summary>
    /// Набор полей, описывающих служащго
    /// P.S. вряд ли можно назвать "моделью" =)
    /// </summary>
    public class EmployeeModel: BaseModel
    {
        #region Constructor
        public EmployeeModel()
        {
        }
        #endregion

        #region Properties
                
        /// <summary>
        /// Имя служащего
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество служащего
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Личный номер
        /// </summary>
        public string PersonalNumber { get; set; }

        /// <summary>
        /// Фамилия служащего
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Должность служащего
        /// </summary>
        public long? PostId { get; set; }

        /// <summary>
        /// Звание служащего
        /// </summary>
        public long? RankId { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime? BornDate { get; set; }

        /// <summary>
        /// Дата начала службы в МЧС
        /// </summary>
        public DateTime? JobStartDate { get; set; }

        /// <summary>
        /// Адрес TODO!!!: избыточность адреса
        /// </summary>
        public string Address { get; set; }
        
        /// <summary>
        /// Город
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Улица
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Номер дома
        /// </summary>
        public string House { get; set; }

        /// <summary>
        /// Номер квартиры
        /// </summary>
        public string Flat { get; set; }

        /// <summary>
        /// Паспортные данные TODO!!!:избыточность...или нет=)
        /// </summary>
        public string Pasport { get; set; }

        /// <summary>
        /// Серия паспорта
        /// </summary>
        public string PasportSeries { get; set; }

        /// <summary>
        /// Организация, выдавшая паспорт
        /// </summary>
        public string PasportOrganizationUnit { get; set; }

        /// <summary>
        /// Номер паспорта
        /// </summary>
        public string PasportNumber { get; set; }

        /// <summary>
        /// Номер мобильного телефона
        /// </summary>
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// Номер домашнего телефона
        /// </summary>
        public string HomePhoneNumber { get; set; }

        /// <summary>
        /// Фотография служащего
        /// </summary>
        public BitmapImage Photo { get; set; }
        
        public bool IsPensioner { get; set; }
                
        #endregion

    }
}
