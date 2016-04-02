using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Staffinfo.Tests
{
    [TestClass]
    public class ExpirienceTimeTests
    {
        /// <summary>
        /// TODO
        /// </summary>
        [TestMethod]
        public void GetDateFromDateDiff()
        {
            var startDate = new DateTime(2001, 1, 1);
            var finishDate = new DateTime(2016, 1, 1);

            var days = (finishDate - startDate).Days;

            int years = 0;
            while (days > 365)
            {
                days -= 365;
                years++;
            }

        }
    }
}