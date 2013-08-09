using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IntAirAct;
using System.Collections.Generic;

namespace IntAirActTests
{
    [TestClass]
    public class IADeserializationTests
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
        public void BodyAsStringTest()
        {
            string expected = "example string";
            IADeSerialization deSerialization = new IADeSerialization();
            deSerialization.SetBodyWithString(expected);
            string actual = deSerialization.BodyAs<String>();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void BodyAsNumberTest()
        {
            int expected = 50;
            IADeSerialization deSerialization = new IADeSerialization();
            deSerialization.SetBodyWith(expected);
            int actual = deSerialization.BodyAs<int>();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void BodyAsAnArrayOfStringTest()
        {
            string[] expected = new string[] { "example string" };
            IADeSerialization deSerialization = new IADeSerialization();
            deSerialization.SetBodyWith(expected);
            string[] actual = deSerialization.BodyAs<string[]>();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void BodyAsAnArrayOfNumbersTest()
        {
            int[] expected = new int[] { 50 };
            IADeSerialization deSerialization = new IADeSerialization();
            deSerialization.SetBodyWith(expected);
            int[] actual = deSerialization.BodyAs<int[]>();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void BodyAsADictionaryTest()
        {
            Dictionary<string, string> expected = new Dictionary<string, string>() { {"key", "value"} };
            IADeSerialization deSerialization = new IADeSerialization();
            deSerialization.SetBodyWith(expected);
            Dictionary<string, string> actual = deSerialization.BodyAs<Dictionary<string, string>>();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void BodyAsIAModelWithIntPropertyTest()
        {
            IAModelWithIntProperty expected = new IAModelWithIntProperty();
            expected.Number = 50;
            IADeSerialization deSerialization = new IADeSerialization();
            deSerialization.SetBodyWith(expected);
            IAModelWithIntProperty actual = deSerialization.BodyAs<IAModelWithIntProperty>();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void BodyAsIAModelWithFloatPropertyTest()
        {
            IAModelWithFloatProperty expected = new IAModelWithFloatProperty();
            expected.Number = 5.434f;
            IADeSerialization deSerialization = new IADeSerialization();
            deSerialization.SetBodyWith(expected);
            IAModelWithFloatProperty actual = deSerialization.BodyAs<IAModelWithFloatProperty>();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void BodyAsIAModelInheritanceTest()
        {
            IAModelInheritance expected = new IAModelInheritance();
            expected.Number = 50;
            expected.NumberTwo = 60;
            IADeSerialization deSerialization = new IADeSerialization();
            deSerialization.SetBodyWith(expected);
            IAModelInheritance actual = deSerialization.BodyAs<IAModelInheritance>();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void BodyAsIAModelReferenceTest()
        {
            IAModelWithIntProperty model = new IAModelWithIntProperty();
            model.Number = 50;
            IAModelReference expected = new IAModelReference();
            expected.Number = model;
            IADeSerialization deSerialization = new IADeSerialization();
            deSerialization.SetBodyWith(expected);
            IAModelReference actual = deSerialization.BodyAs<IAModelReference>();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void BodyAsReturnsNilTest()
        {
            IADeSerialization deSerialization = new IADeSerialization();
            deSerialization.SetBodyWith("-");
            IAModelWithIntProperty actual = deSerialization.BodyAs<IAModelWithIntProperty>();
            Assert.AreEqual(null, actual);
        }

        [TestMethod()]
        public void StringBodyAsNumberTest()
        {
            IAModelWithFloatProperty expected = new IAModelWithFloatProperty();
            expected.Number = 50.6f;

            IADeSerialization deSerialization = new IADeSerialization();
            deSerialization.SetBodyWith("{\"number\":\"50.6\"}");
            IAModelWithFloatProperty actual = deSerialization.BodyAs<IAModelWithFloatProperty>();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void BodyAsIAModelWithStringPropertyTest()
        {
            IAModelWithStringProperty expected = new IAModelWithStringProperty();
            expected.StringProperty = "50.6";

            IADeSerialization deSerialization = new IADeSerialization();
            deSerialization.SetBodyWith("{\"stringProperty\":\"50.6\"}");
            IAModelWithStringProperty actual = deSerialization.BodyAs<IAModelWithStringProperty>();
            Assert.AreEqual(expected, actual);
        }
    }
}
