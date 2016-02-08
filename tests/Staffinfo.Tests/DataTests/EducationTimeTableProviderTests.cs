using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staffinfo.Desktop.Data.DataTableProviders;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Tests.DataTests
{
    [TestClass]
    public class EducationTimeTableProviderTests
    {
        [TestMethod]
        public void AddNewEducationTimeIntoDatabase()
        {
            using (var prvdr = new EducationTimeTableProvider())
            {
                var educationTimeModel = prvdr.Save(
                    new EducationTimeModel
                    {
                        Id = null,
                        EmployeeId = 1,
                        StartDate = DateTime.Now.AddYears(-1),
                        FinishDate = DateTime.Now,
                        SpecialityId = 1,
                        InstitutionId = 1,
                        Description = "описание"
                    });

                Assert.IsNotNull(educationTimeModel, prvdr.ErrorInfo);

                Assert.IsNotNull(educationTimeModel.Id, prvdr.ErrorInfo);
                Assert.IsTrue(prvdr.DeleteById(educationTimeModel.Id), prvdr.ErrorInfo);
            }
        }

        [TestMethod]
        public void GetEducationTimeById()
        {
            using (var prvdr = new EducationTimeTableProvider())
            {
                var educationTime = prvdr.Select(1);

                Assert.IsNotNull(educationTime, prvdr.ErrorInfo);
            }
        }

        [TestMethod]
        public void GetAllEducationTimes_HasElements()
        {
            using (var prvdr = new EducationTimeTableProvider())
            {
                var educationTimeList = prvdr.Select( );

                Assert.IsNotNull(educationTimeList, "educationTimeList != null");
                Assert.IsTrue(educationTimeList.Count > 0, "educationTimeList.Count > 0");
            }
        }

        [TestMethod]
        public void UpdateEducationTime()
        {
            using (var prvdr = new EducationTimeTableProvider())
            {
                Assert.IsTrue(
                    prvdr.Update(new EducationTimeModel
                    {
                        Id = 1,
                        EmployeeId = 1,
                        StartDate = DateTime.Now.AddYears(-1),
                        FinishDate = DateTime.Now,
                        SpecialityId = 1,
                        InstitutionId = 1,
                        Description = "описание"
                    }), prvdr.ErrorInfo);
            }
        }
    }
}
