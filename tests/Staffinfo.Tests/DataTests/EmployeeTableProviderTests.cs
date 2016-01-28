using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staffinfo.Desktop.Data.DataTableProviders;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Tests
{
    [TestClass]
    public class EmployeeTableProviderTests
    {
        [TestMethod]
        public void AddNewEmployeeIsSuccess()
        {
            using (var ePrvdr = new EmployeeTableProvider())
            {
                var employee = ePrvdr.AddNewElement(new EmployeeModel
                {
                    LastName = "Петров",
                    FirstName = "Иван",
                    MiddleName = "Сергеевич",
                    Address = "г. Гомель, ул. Советская, 97/3",
                    BornDate = DateTime.Now,
                    JobStartDate = DateTime.Now.AddDays(-1),
                    HomePhoneNumber = "99-26-25",
                    MobilePhoneNumber = "+375994438512",
                    Pasport = "HB#1234567",
                    PersonalNumber = "U-2345",
                    PostId = 2,
                    RankId = 3,
                    IsPensioner = false
                });

                Assert.IsNotNull(employee);
            }
        }
        
    }
}
