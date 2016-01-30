using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staffinfo.Desktop.Data.DataTableProviders;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Tests.DataTests
{
    [TestClass]
    public class EducationalInstitutionTableProviderTests
    {
        [TestMethod]
        public void AddNewEducationalInstitutionTimeIntoDatabase()
        {
            using (var prvdr = new EducationalInstitutonTableProvider())
            {
                var educationalInstitutionModel = prvdr.AddNewElement(
                    new EducationalInstitutionModel
                    {
                        Id = null,
                        InstituitionTitle = "ГГТУ",
                        InstituitionType = "ВУЗ",
                        Description = "описание"
                    });

                Assert.IsNotNull(educationalInstitutionModel, prvdr.ErrorInfo);

                Assert.IsNotNull(educationalInstitutionModel.Id, prvdr.ErrorInfo);
                Assert.IsTrue(prvdr.DeleteById(educationalInstitutionModel.Id), prvdr.ErrorInfo);
            }
        }

        [TestMethod]
        public void GetEducationalInstitutionById()
        {
            using (var prvdr = new EducationalInstitutonTableProvider())
            {
                var educationalInstitution = prvdr.GetElementById(1);

                Assert.IsNotNull(educationalInstitution, prvdr.ErrorInfo);
            }
        }

        [TestMethod]
        public void GetAllEducationalInstitutions_HasElements()
        {
            using (var prvdr = new EducationalInstitutonTableProvider())
            {
                var educationalInstitutionList = prvdr.GetAllElements();

                Assert.IsNotNull(educationalInstitutionList, "educationalInstitutionList != null");
                Assert.IsTrue(educationalInstitutionList.Count > 0, "educationalInstitutionList.Count > 0");
            }
        }

        [TestMethod]
        public void UpdateEducationalInstitution()
        {
            using (var prvdr = new EducationalInstitutonTableProvider())
            {
                Assert.IsTrue(
                    prvdr.Update(new EducationalInstitutionModel
                    {
                        Id = 1,
                        InstituitionTitle = "изменен",
                        InstituitionType = "ВУЗ",
                        Description = "описание"
                    }), prvdr.ErrorInfo);
            }
        }
    }
}
