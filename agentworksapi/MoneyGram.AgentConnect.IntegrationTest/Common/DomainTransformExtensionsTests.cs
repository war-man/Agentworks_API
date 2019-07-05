using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.AgentConnect;
using System.Collections;
using System.Collections.Generic;

namespace MoneyGram.AgentConnect.IntegrationTest.Common
{
    [TestClass]
    public class DomainTransformExtensionsTests
    {
        [TestMethod]
        public void NullifyWhiteSpaceStringsTest()
        {
            var testModel = new ServiceObject(" ", 1, new List<string>(), "    ", " ", 5);
            var excluded = new List<string>();
            excluded.Add("TelePhoneNumber");
            DomainTransformExtensions.NullifyWhiteSpaceStrings(testModel, excluded);

            Assert.IsTrue(testModel.Name == null, "");
            Assert.IsTrue(testModel.FullName == null, "");
            Assert.IsTrue(testModel.TelePhoneNumber != null);
            // Testing recursiveness
            Assert.IsTrue(testModel.Person.FirstName != null);
            Assert.IsTrue(testModel.Person.LastName == null);
        }

        public class ServiceObject
        {
            public NestedObject Person { get; set; }
            public string Name { get; set; }
            public int Id { get; set; }
            public IEnumerable Collection { get; set; }
            public string FullName { get; set; }
            public string TelePhoneNumber { get; set; }
            public int CountyCode { get; set; }            
            public ServiceObject(string name, int id, IEnumerable collection, string fullname, string tele, int countycode)
            {
                this.Name = name;
                this.Id = id;
                this.Collection = collection;
                this.FullName = fullname;
                this.TelePhoneNumber = tele;
                this.CountyCode = countycode;
                Person = new NestedObject
                {
                    FirstName = "Populated",
                    LastName = "  "
                };
            }
        }

        public class NestedObject
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }
}