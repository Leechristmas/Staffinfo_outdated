using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staffinfo.Desktop.Data.DataTableProviders;
using Staffinfo.Desktop.Model;

    namespace Staffinfo.Tests.DataTests
{
    [TestClass]
    public class EmployeeTableProviderTests
    {
        [TestMethod]
        public void AddNewEmployeeIsSuccess()
        {
            using (var prvdr = new EmployeeTableProvider())
            {
                var employee = prvdr.Save(new EmployeeModel
                {
                    LastName = "Петров",
                    FirstName = "Иван",
                    MiddleName = "Сергеевич",
                    Address = "г. Гомель, ул. Советская, 97/3",
                    BornDate = DateTime.Now,
                    JobStartDate = DateTime.Now.AddDays(-1),
                    HomePhoneNumber = "99-26-25",
                    MobilePhoneNumber = "+375994438512",
                    PasportId = 2,
                    PersonalNumber = "U-2345",
                    PostId = 2,
                    RankId = 3,
                    IsPensioner = false
                });

                Assert.IsNotNull(employee);
                Assert.IsNotNull(employee.Id);
                Assert.IsTrue(prvdr.DeleteById(employee.Id));
            }
        }

        [TestMethod]
        public void GetEmployeeById()
        {
            using (var prvdr = new EmployeeTableProvider())
            {
                var employeeModel = prvdr.Select(1);

                Assert.IsNotNull(employeeModel);
                Assert.IsTrue(employeeModel.GetType() == typeof(EmployeeModel));
            }
        }

        [TestMethod]
        public void GetAllEmployees_HasElements()
        {
            using (var prvdr = new EmployeeTableProvider())
            {
                var employeeList = prvdr.Select();

                Assert.IsNotNull(employeeList);
                Assert.IsTrue(employeeList.Count > 0);
            }
        }

        [TestMethod]
        public void UpdateEmployee()
        {
            using (var prvdr = new EmployeeTableProvider())
            {
                Assert.IsTrue(prvdr.Update(new EmployeeModel
                {
                    Id = 1,
                    LastName = "Изменен",
                    FirstName = "Иван",
                    MiddleName = "Сергеевич",
                    Address = "г. Гомель, ул. Советская, 97/3",
                    BornDate = DateTime.Now,
                    JobStartDate = DateTime.Now.AddDays(-1),
                    HomePhoneNumber = "99-26-25",
                    MobilePhoneNumber = "+375994438512",
                    PasportId = 1,
                    PersonalNumber = "U-2345",
                    PostId = 2,
                    RankId = 3,
                    IsPensioner = false
                }));
            }
        }
    }
}
