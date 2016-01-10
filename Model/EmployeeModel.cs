using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staffinfo.Model
{
    /// <summary>
    /// Набор полей, описывающих служащго
    /// P.S. вряд ли можно назвать "моделью" =)
    /// </summary>
    public class EmployeeModel
    {
        #region Constructor
        public EmployeeModel()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="firstName">Имя</param>
        /// <param name="middleName">Отчетство</param>
        /// <param name="lastName">Фамилия</param>
        /// <param name="personalNumber"></param>
        /// <param name="post">Должность</param>
        /// <param name="rank">Звание</param>
        /// <param name="bornDate">Дата рождения</param>
        /// <param name="jobStartDate">Дата начала работы в МЧС</param>
        /// <param name="service">Службы</param>
        /// <param name="address">Адрес</param>
        /// <param name="pasport">Паспортные данные</param>
        /// <param name="mobilePhoneNumber">Мобильный телефон</param>
        /// <param name="homePhoneNumber">Домашний телефон</param>
        public EmployeeModel(
            long id,
            string firstName,
            string middleName,
            string lastName,
            string personalNumber,
            long? post,
            long? rank,
            DateTime? bornDate,
            DateTime? jobStartDate,
            string address,
            string pasport,
            string mobilePhoneNumber)
        {
            Id = id;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            PersonalNumber = personalNumber;
            Post = post;
            Rank = rank;
            BornDate = bornDate;
            JobStartDate = jobStartDate;
            Address = address;
            Pasport = pasport;
            MobilePhoneNumber = mobilePhoneNumber;
        }
        #endregion

        #region Properties

        public long Id { get; set; }
        
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
        public long? Post { get; set; }

        /// <summary>
        /// Звание служащего
        /// </summary>
        public long? Rank { get; set; }

        /// <summary>
        /// Служба
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime? BornDate { get; set; }

        /// <summary>
        /// Дата начала службы в МЧС
        /// </summary>
        public DateTime? JobStartDate { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Выслуга лет
        /// </summary>
        public string TotalExpirienceTimeByYear { get; set; }

        /// <summary>
        /// Выслуга до службы в МЧС
        /// </summary>
        public string MilitaryExpirience { get; set; }

        /// <summary>
        /// Паспортные данные
        /// </summary>
        public string Pasport { get; set; }

        /// <summary>
        /// Номер мобильного телефона
        /// </summary>
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// Номер домашнего телефона
        /// </summary>
        public string HomePhoneNumber { get; set; }
                
        #endregion

    }
}
