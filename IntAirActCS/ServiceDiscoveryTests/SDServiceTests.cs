using ServiceDiscovery;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ServiceDiscoveryTests
{

    /// <summary>
    ///This is a test class for SDServiceTest and is intended
    ///to contain all SDServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SDServiceTests
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for SDService Constructor
        ///</summary>
        [TestMethod()]
        public void SDServiceConstructorTest()
        {
            string name = "name";
            string hostname = "hostname";
            ushort port = 8080;
            string type = "type";
            Dictionary<string, string> TXTRecord = new Dictionary<string, string>() { {"key", "value"} };
            SDService service = new SDService(name, hostname, port, type, TXTRecord);
            Assert.AreEqual(name, service.Name);
            Assert.AreEqual(hostname, service.Hostname);
            Assert.AreEqual(port, service.Port);
            Assert.AreEqual(type, service.Type);
            Assert.AreEqual(TXTRecord, service.TXTRecord);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        public void EqualsTest()
        {
            string name = "name";
            string hostname = "hostname";
            ushort port = 8080;
            string type = "type";
            Dictionary<string, string> TXTRecord = new Dictionary<string, string>() { { "key", "value" } };
            SDService service = new SDService(name, hostname, port, type, TXTRecord);
            SDService other = new SDService(name, hostname, port, type, TXTRecord);
            Assert.IsTrue(service.Equals(other));
            Assert.IsTrue(service.GetHashCode() == other.GetHashCode());
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        public void EqualsWithSelfTest()
        {
            string name = "name";
            string hostname = "hostname";
            ushort port = 8080;
            string type = "type";
            Dictionary<string, string> TXTRecord = new Dictionary<string, string>() { { "key", "value" } };
            SDService service = new SDService(name, hostname, port, type, TXTRecord);
            Assert.IsTrue(service.Equals(service));
            Assert.IsTrue(service.GetHashCode() == service.GetHashCode());
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        public void EqualsWithNullTest()
        {
            string name = "name";
            string hostname = "hostname";
            ushort port = 8080;
            string type = "type";
            Dictionary<string, string> TXTRecord = new Dictionary<string, string>() { { "key", "value" } };
            SDService service = new SDService(name, hostname, port, type, TXTRecord);
            Assert.IsFalse(service.Equals(null));
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        public void EqualsWithNameNullTest()
        {
            string name = "name";
            string hostname = "hostname";
            ushort port = 8080;
            string type = "type";
            Dictionary<string, string> TXTRecord = new Dictionary<string, string>() { { "key", "value" } };
            SDService service = new SDService(null, hostname, port, type, TXTRecord);
            SDService other = new SDService(name, hostname, port, type, TXTRecord);
            Assert.IsFalse(service.Equals(other));
            Assert.IsFalse(service.GetHashCode() == other.GetHashCode());
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        public void EqualsFailsTest()
        {
            string name = "name";
            string differentName = "differentName";

            string hostname = "hostname";
            ushort port = 8080;
            string type = "type";
            Dictionary<string, string> TXTRecord = new Dictionary<string, string>() { { "key", "value" } };
            SDService service = new SDService(name, hostname, port, type, TXTRecord);
            SDService other = new SDService(differentName, hostname, port, type, TXTRecord);
            Assert.IsFalse(service.Equals(other));
            Assert.IsFalse(service.GetHashCode() == other.GetHashCode());
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        public void EqualsWithDifferentObjectTest()
        {
            string name = "name";
            string hostname = "hostname";
            ushort port = 8080;
            string type = "type";
            Dictionary<string, string> TXTRecord = new Dictionary<string, string>() { { "key", "value" } };
            SDService service = new SDService(name, hostname, port, type, TXTRecord);

            string other = "name";
            Assert.IsFalse(service.Equals(other));
            Assert.IsFalse(service.GetHashCode() == other.GetHashCode());
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            string name = "name";
            string hostname = "hostname";
            ushort port = 8080;
            string type = "type";
            Dictionary<string, string> TXTRecord = new Dictionary<string, string>() { { "key", "value" } };
            SDService service = new SDService(name, hostname, port, type, TXTRecord);
            Assert.IsNotNull(service.ToString());
        }
    }
}
