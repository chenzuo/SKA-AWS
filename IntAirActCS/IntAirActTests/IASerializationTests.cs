using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IntAirAct;
using System.Collections.Generic;

namespace IntAirActTests
{
    [TestClass]
    public class IASerializationTests
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

        [TestMethod()]
        public void ConstructorTest()
        {
            byte[] expectedBody = new byte[] { 0, 1 };
            string expectedContentType = "text/plain";
            IADeSerialization deSerialization = new IADeSerialization(expectedBody, expectedContentType);
            Assert.AreEqual(expectedBody, deSerialization.Body);
            Assert.AreEqual(expectedContentType, deSerialization.ContentType);
        }

        [TestMethod()]
        public void SetBodyWithStringTest()
        {
            string body = "example string";
            IADeSerialization deSerialization = new IADeSerialization();
            deSerialization.SetBodyWithString(body);
            Assert.AreEqual(body, deSerialization.BodyAsString());
        }

        [TestMethod()]
        public void SetBodyWithWithAStringTest()
        {
            string body = "example string";
            IADeSerialization deSerialization = new IADeSerialization();
            deSerialization.SetBodyWith(body);
            Assert.AreEqual(body, deSerialization.BodyAsString());
        }

        [TestMethod()]
        public void SetBodyWithWithAnArrayOfStringTest()
        {
            string[] body = new string[] { "example string" };
            IADeSerialization deSerialization = new IADeSerialization();
            deSerialization.SetBodyWith(body);
            Assert.AreEqual("[\"example string\"]", deSerialization.BodyAsString());
        }

        [TestMethod()]
        public void SetBodyWithWithANumberTest()
        {
            IADeSerialization deSerialization = new IADeSerialization();
            deSerialization.SetBodyWith(50);
            Assert.AreEqual("50", deSerialization.BodyAsString());
        }

        [TestMethod()]
        public void SetBodyWithWithAnArrayOfNumbersTest()
        {
            IADeSerialization deSerialization = new IADeSerialization();
            deSerialization.SetBodyWith(new int[] { 50 });
            Assert.AreEqual("[50]", deSerialization.BodyAsString());
        }

        [TestMethod()]
        public void SetBodyWithWithADictionaryTest()
        {
            IADeSerialization deSerialization = new IADeSerialization();
            deSerialization.SetBodyWith(new Dictionary<string, string>() { {"key", "value"} });
            Assert.AreEqual("{\"key\":\"value\"}", deSerialization.BodyAsString());
        }

        [TestMethod()]
        public void SetBodyWithWithADictionaryUsingNumberKeysTest()
        {
            IADeSerialization deSerialization = new IADeSerialization();
            deSerialization.SetBodyWith(new Dictionary<int, string>() { { 50, "value" } });
            Assert.AreEqual("{\"50\":\"value\"}", deSerialization.BodyAsString());
        }

        [TestMethod()]
        public void SetBodyWithWithAnIAModelWithIntPropertyTest()
        {
            IAModelWithIntProperty model = new IAModelWithIntProperty();
            model.Number = 50;
            IADeSerialization deSerialization = new IADeSerialization();
            deSerialization.SetBodyWith(model);
            Assert.AreEqual("{\"Number\":50}", deSerialization.BodyAsString());
        }

        [TestMethod()]
        public void SetBodyWithWithAnIAModelWithFloatPropertyTest()
        {
            IAModelWithFloatProperty model = new IAModelWithFloatProperty();
            model.Number = 5.434f;
            IADeSerialization deSerialization = new IADeSerialization();
            deSerialization.SetBodyWith(model);
            Assert.AreEqual("{\"Number\":5.434}", deSerialization.BodyAsString());
        }

        [TestMethod()]
        public void SetBodyWithWithAnIAModelInheritanceTest()
        {
            IAModelInheritance model = new IAModelInheritance();
            model.Number = 50;
            model.NumberTwo = 60;
            IADeSerialization deSerialization = new IADeSerialization();
            deSerialization.SetBodyWith(model);
            Assert.AreEqual("{\"NumberTwo\":60,\"Number\":50}", deSerialization.BodyAsString());
        }

        [TestMethod()]
        public void SetBodyWithWithAnIAModelReferenceTest()
        {
            IAModelWithIntProperty intprop = new IAModelWithIntProperty();
            intprop.Number = 50;
            IAModelReference model = new IAModelReference();
            model.Number = intprop;
            IADeSerialization deSerialization = new IADeSerialization();
            deSerialization.SetBodyWith(model);
            Assert.AreEqual("{\"Number\":{\"Number\":50}}", deSerialization.BodyAsString());
        }

        [TestMethod()]
        public void SetBodyWithWithNullTest()
        {
            IADeSerialization deSerialization = new IADeSerialization();
            deSerialization.SetBodyWith(null);
            Assert.AreEqual("", deSerialization.BodyAsString());
        }
    }
}
