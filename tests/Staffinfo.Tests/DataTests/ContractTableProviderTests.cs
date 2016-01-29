using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staffinfo.Desktop.Data.DataTableProviders;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Tests.DataTests
{
    [TestClass]
    public class ContractTableProviderTests
    {
        [TestMethod]
        public void AddNewContractIntoDatabase()
        {
            using (var prvdr = new  ContractTableProvider())
            {
                var contract = prvdr.AddNewElement(
                    new ContractModel
                    {
                        Id = null,
                        EmployeeId = 1,
                        StartDate = DateTime.Now.AddYears(-1),
                        FinishDate = DateTime.Now,
                        Description = "описание"
                    });

                Assert.IsNotNull(contract, "contract != null");

                Assert.IsNotNull(contract.Id, "contract.Id != null");
                Assert.IsTrue(prvdr.DeleteById(contract.Id), "prvdr.DeleteById(contract.Id)");
            }
        }

        [TestMethod]
        public void GetContractById()
        {
            using (var prvdr = new ContractTableProvider())
            {
                var contract = prvdr.GetElementById(1);

                Assert.IsNotNull(contract, "contract != null");
            }
        }

        [TestMethod]
        public void GetAllContracts_HasElements()
        {
            using (var prvdr = new ContractTableProvider())
            {
                var contractList = prvdr.GetAllElements();

                Assert.IsNotNull(contractList, "contractList != null");
                Assert.IsTrue(contractList.Count > 0,"contractList.Count > 0");
            }
        }

        [TestMethod]
        public void UpdateContract()
        {
            using (var prvdr = new ContractTableProvider())
            {
                Assert.IsTrue(
                    prvdr.Update(new ContractModel
                    {
                        Id = 1,
                        EmployeeId = 1,
                        StartDate = DateTime.MinValue,
                        FinishDate = DateTime.MaxValue,
                        Description = "изменен"
                    }), "updating was failed.");
            }
        }
    }
}
