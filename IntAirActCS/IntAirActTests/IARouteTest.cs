using IntAirAct;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IntAirActTests
{

    /// <summary>
    ///This is a test class for IARouteTest and is intended
    ///to contain all IARouteTest Unit Tests
    ///</summary>
    [TestClass()]
    public class IARouteTest
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
        public void IARouteConstructorTest()
        {
            string action = "GET";
            string resource = "/tests";
            string actualAction;
            string actualResource;
            IARoute target = new IARoute(action, resource);
            actualAction = target.Action;
            actualResource = target.Resource;
            Assert.AreEqual(action, actualAction);
            Assert.AreEqual(resource, actualResource);
        }

        [TestMethod()]
        public void EqualsTest()
        {
            IARoute route = new IARoute("GET", "/example");
            IARoute other = new IARoute("GET", "/example");
            Assert.IsTrue(route.Equals(other));
            Assert.AreEqual(route.GetHashCode(), other.GetHashCode());
        }

        [TestMethod()]
        public void EqualsSelfTest()
        {
            IARoute route = new IARoute("GET", "/example");
            IARoute other = route;
            Assert.IsTrue(route.Equals(other));
            Assert.AreEqual(route.GetHashCode(), other.GetHashCode());
        }

        [TestMethod()]
        public void EqualsNullTest()
        {
            IARoute route = new IARoute("GET", "/example");
            IARoute other = null;
            Assert.IsFalse(route.Equals(other));
        }

        [TestMethod()]
        public void EqualsDifferentActionTest()
        {
            IARoute route = new IARoute("GET", "/example");
            IARoute other = new IARoute("PUT", "/example");
            Assert.IsFalse(route.Equals(other));
            Assert.AreNotEqual(route.GetHashCode(), other.GetHashCode());
        }

        [TestMethod()]
        public void EqualsDifferentResourceTest()
        {
            IARoute route = new IARoute("GET", "/example");
            IARoute other = new IARoute("GET", "/example2");
            Assert.IsFalse(route.Equals(other));
            Assert.AreNotEqual(route.GetHashCode(), other.GetHashCode());
        }

        [TestMethod()]
        public void EqualsActionIsNullTest()
        {
            IARoute route = new IARoute(null, "/example");
            IARoute other = new IARoute("GET", "/example2");
            Assert.IsFalse(route.Equals(other));
            Assert.AreNotEqual(route.GetHashCode(), other.GetHashCode());
        }

        [TestMethod()]
        public void EqualsResourceIsNullTest()
        {
            IARoute route = new IARoute("GET", null);
            IARoute other = new IARoute("GET", "/example");
            Assert.IsFalse(route.Equals(other));
            Assert.AreNotEqual(route.GetHashCode(), other.GetHashCode());
        }

        [TestMethod()]
        public void GetConstructorTest()
        {
            IARoute route = IARoute.Get("");
            Assert.AreEqual("GET", route.Action);
        }

        [TestMethod()]
        public void PutConstructorTest()
        {
            IARoute route = IARoute.Put("");
            Assert.AreEqual("PUT", route.Action);
        }

        [TestMethod()]
        public void PostConstructorTest()
        {
            IARoute route = IARoute.Post("");
            Assert.AreEqual("POST", route.Action);
        }

        [TestMethod()]
        public void DeleteConstructorTest()
        {
            IARoute route = IARoute.Delete("");
            Assert.AreEqual("DELETE", route.Action);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            IARoute route = new IARoute("", "");
            Assert.IsNotNull(route.ToString());
        }
    }
}
