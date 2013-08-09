using ServiceDiscovery;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Threading;

namespace ServiceDiscoveryTests
{
    
    /// <summary>
    ///This is a test class for SDServiceDiscoveryTest and is intended
    ///to contain all SDServiceDiscoveryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SDServiceDiscoveryTest
    {
        private static TraceSource logger = new TraceSource("ServiceDiscoveryTests");

        const string SERVICE_TYPE = "_test._tcp.";
        const ushort SERVICE_PORT = 80;
        TimeSpan WAIT_TIME = new TimeSpan(0, 0, 5);

        private TestContext testContextInstance;
        private SDServiceDiscovery serviceDiscovery;

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


        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void SDServiceDiscoveryTestInitialize()
        {
            this.serviceDiscovery = new SDServiceDiscovery();
        }

        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            this.serviceDiscovery.Stop();
        }

        private void PublishWithName(string name)
        {
            DateTime dateTimeWhenToStopWaiting;
            bool found = false;
            AutoResetEvent are = new AutoResetEvent(false);

            this.serviceDiscovery.ServiceFound += delegate(SDService service , bool ownService)
            {
                logger.TraceEvent(TraceEventType.Information, 0, String.Format("Found service: {0} ownService: {1}", service, ownService));
                if (ownService)
                {
                    found = true;
                    are.Set();
                }
            };

            this.serviceDiscovery.SearchForServices(SERVICE_TYPE);

            Assert.IsTrue(this.serviceDiscovery.IsSearching);

            this.serviceDiscovery.PublishService(SERVICE_TYPE, SERVICE_PORT);

            Assert.IsTrue(this.serviceDiscovery.IsPublishing);

            dateTimeWhenToStopWaiting = DateTime.Now.Add(WAIT_TIME);

            while (!found && DateTime.Now < dateTimeWhenToStopWaiting)
            {
                long ticksToWait = dateTimeWhenToStopWaiting.Subtract(DateTime.Now).Ticks;
                are.WaitOne(new TimeSpan(ticksToWait));
            }

            this.serviceDiscovery.StopSearchingForServices(SERVICE_TYPE);

            Assert.IsFalse (this.serviceDiscovery.IsSearching);

            this.serviceDiscovery.StopPublishingService(SERVICE_TYPE, SERVICE_PORT);

            Assert.IsFalse(this.serviceDiscovery.IsPublishing);

            if (!found)
            {
                Assert.Fail();
            }
        }

        /// <summary>
        ///A test for SDServiceDiscovery Constructor
        ///</summary>
        [TestMethod()]
        public void PublishAndDiscoveryTest()
        {
            this.PublishWithName("");
        }

        /// <summary>
        ///A test for SDServiceDiscovery Constructor
        ///</summary>
        [TestMethod()]
        public void PublishingWithAWeirdNameTest()
        {
            this.PublishWithName(" #$%&\\,.<>/^|_[]{}()ď§χβґм₣£Ĵო1đÒŖ@ŤßỊ§ţЯ");
        }

        /// <summary>
        ///A test for SDServiceDiscovery Constructor
        ///</summary>
        [TestMethod()]
        public void PublishDiscoveryAndRemovalTest()
        {
            DateTime dateTimeWhenToStopWaiting;
            bool found = false;
            AutoResetEvent are = new AutoResetEvent(false);
            SDService foundService = null;

            this.serviceDiscovery.ServiceFound += delegate(SDService service, bool ownService)
            {
                if (ownService)
                {
                    foundService = service;
                    found = true;
                    are.Set();
                }
            };

            this.serviceDiscovery.ServiceLost += delegate(SDService service)
            {
                if (service.Equals(foundService))
                {
                    found = false;
                    are.Set();
                }
            };

            this.serviceDiscovery.SearchForServices(SERVICE_TYPE);
            this.serviceDiscovery.PublishService(SERVICE_TYPE, SERVICE_PORT);

            dateTimeWhenToStopWaiting = DateTime.Now.Add(WAIT_TIME);

            while (!found && DateTime.Now < dateTimeWhenToStopWaiting)
            {
                long ticksToWait = dateTimeWhenToStopWaiting.Subtract(DateTime.Now).Ticks;
                are.WaitOne(new TimeSpan(ticksToWait));
            }

            are.Reset();

            if (!found)
            {
                this.serviceDiscovery.StopSearchingForServices(SERVICE_TYPE);
                this.serviceDiscovery.StopPublishingService(SERVICE_TYPE, SERVICE_PORT);
                Assert.Fail();
            }

            this.serviceDiscovery.StopPublishing();

            dateTimeWhenToStopWaiting = DateTime.Now.Add(WAIT_TIME);
            while (found && DateTime.Now < dateTimeWhenToStopWaiting)
            {
                long ticksToWait = dateTimeWhenToStopWaiting.Subtract(DateTime.Now).Ticks;
                are.WaitOne(new TimeSpan(ticksToWait));
            }

            this.serviceDiscovery.StopSearchingForServices(SERVICE_TYPE);
            this.serviceDiscovery.StopPublishingService(SERVICE_TYPE, SERVICE_PORT);

            if (found)
            {
                Assert.Fail();
            }
        }

        /// <summary>
        ///A test for SDServiceDiscovery Constructor
        ///</summary>
        [TestMethod()]
        public void SearchingTwiceShouldFailTest()
        {
            Assert.IsTrue(this.serviceDiscovery.SearchForServices(SERVICE_TYPE));
            Assert.IsFalse(this.serviceDiscovery.SearchForServices(SERVICE_TYPE));
        }

        /// <summary>
        ///A test for SDServiceDiscovery Constructor
        ///</summary>
        [TestMethod()]
        public void PublishingTwiceShouldFailTest()
        {
            Assert.IsTrue(this.serviceDiscovery.PublishService(SERVICE_TYPE, SERVICE_PORT));
            Assert.IsFalse(this.serviceDiscovery.PublishService(SERVICE_TYPE, SERVICE_PORT));
        }

        /// <summary>
        ///A test for SDServiceDiscovery Constructor
        ///</summary>
        [TestMethod()]
        public void SearchingTwiceWithAStopInbetweenShouldWorkTest()
        {
            Assert.IsTrue(this.serviceDiscovery.SearchForServices(SERVICE_TYPE));
            Assert.IsFalse(this.serviceDiscovery.SearchForServices(SERVICE_TYPE));
            this.serviceDiscovery.StopSearching();
            Assert.IsTrue(this.serviceDiscovery.SearchForServices(SERVICE_TYPE));
        }

        /// <summary>
        ///A test for SDServiceDiscovery Constructor
        ///</summary>
        [TestMethod()]
        public void PublishingTwiceWithAStopInbetweenShouldWorkTest()
        {
            Assert.IsTrue(this.serviceDiscovery.PublishService(SERVICE_TYPE, SERVICE_PORT));
            Assert.IsFalse(this.serviceDiscovery.PublishService(SERVICE_TYPE, SERVICE_PORT));
            this.serviceDiscovery.StopPublishing();
            Assert.IsTrue(this.serviceDiscovery.PublishService(SERVICE_TYPE, SERVICE_PORT));
        }

        private void Publish(string startType, string stopType)
        {
            this.serviceDiscovery.PublishService(startType, SERVICE_PORT);
            Assert.IsTrue(this.serviceDiscovery.IsPublishing);

            this.serviceDiscovery.StopPublishingService(stopType, SERVICE_PORT);
            Assert.IsFalse(this.serviceDiscovery.IsPublishing);
        }

        /// <summary>
        ///A test for SDServiceDiscovery Constructor
        ///</summary>
        [TestMethod()]
        public void PublishWithDotAndStopWithoutTest()
        {
            this.Publish("_test._tcp.", "_test._tcp");
        }

        /// <summary>
        ///A test for SDServiceDiscovery Constructor
        ///</summary>
        [TestMethod()]
        public void PublishWithoutDotAndStopWithoutTest()
        {
            this.Publish("_test._tcp", "_test._tcp");
        }

        /// <summary>
        ///A test for SDServiceDiscovery Constructor
        ///</summary>
        [TestMethod()]
        public void PublishWithDotAndStopWithTest()
        {
            this.Publish("_test._tcp.", "_test._tcp.");
        }

        /// <summary>
        ///A test for SDServiceDiscovery Constructor
        ///</summary>
        [TestMethod()]
        public void PublishWithoutDotAndStopWithTest()
        {
            this.Publish("_test._tcp", "_test._tcp.");
        }

        private void Search(string startType, string stopType)
        {
            this.serviceDiscovery.SearchForServices(startType);
            Assert.IsTrue(this.serviceDiscovery.IsSearching);

            this.serviceDiscovery.StopSearchingForServices(stopType);
            Assert.IsFalse(this.serviceDiscovery.IsSearching);
        }

        /// <summary>
        ///A test for SDServiceDiscovery Constructor
        ///</summary>
        [TestMethod()]
        public void SearchWithDotAndStopWithoutTest()
        {
            this.Search("_test._tcp.", "_test._tcp");
        }

        /// <summary>
        ///A test for SDServiceDiscovery Constructor
        ///</summary>
        [TestMethod()]
        public void SearchWithoutDotAndStopWithoutTest()
        {
            this.Search("_test._tcp", "_test._tcp");
        }

        /// <summary>
        ///A test for SDServiceDiscovery Constructor
        ///</summary>
        [TestMethod()]
        public void SearchWithDotAndStopWithTest()
        {
            this.Search("_test._tcp.", "_test._tcp.");
        }

        /// <summary>
        ///A test for SDServiceDiscovery Constructor
        ///</summary>
        [TestMethod()]
        public void SearchWithoutDotAndStopWithTest()
        {
            this.Search("_test._tcp", "_test._tcp.");
        }
    }
}
