using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using UpdateControls.Correspondence.Memory;

namespace Correspondence.Reader.Model.Test
{
    [TestClass]
    public class ModelTest
    {
        private Device phone;
        private Device tablet;

        [TestInitialize]
        public async Task Initialize()
        {
            var sharedCommunication = new MemoryCommunicationStrategy();
            string identifier = Guid.NewGuid().ToString();

            phone  = new Device("phone");
            tablet = new Device("tablet");

            await phone.InitializeAsync (sharedCommunication, identifier);
            await tablet.InitializeAsync(sharedCommunication, identifier);
        }

        [TestMethod]
        public async Task ModelTest_SharedAccount()
        {
            var phoneAccont =  (await phone .Individual.Accounts.EnsureAsync()).FirstOrDefault();
            var tabletAccont = (await tablet.Individual.Accounts.EnsureAsync()).FirstOrDefault();

            Assert.IsNotNull(phoneAccont);
            Assert.IsNotNull(tabletAccont);
            Assert.AreEqual(phoneAccont.Identifier, tabletAccont.Identifier);
        }

        [TestMethod]
        public async Task ModelTest_SubscribeToFeed_SeeSubscription()
        {
            var account = (await phone.Individual.Accounts.EnsureAsync()).FirstOrDefault();
            var feed = await phone.Community.AddFactAsync(new Feed("http://myblog.com/rss"));
            var subscription = await phone.Community.AddFactAsync(new Subscription(account, feed));

            var feeds = await phone.Individual.Feeds.EnsureAsync();

            Assert.AreEqual(1, feeds.Count());
            Assert.AreEqual("http://myblog.com/rss", feeds.Single().Url);
        }

        [TestMethod]
        public async Task ModelTest_SubscribeToFeed_SharedBetweenDevices()
        {
            var account = (await phone.Individual.Accounts.EnsureAsync()).FirstOrDefault();
            var feed = await phone.Community.AddFactAsync(new Feed("http://myblog.com/rss"));
            var subscription = await phone.Community.AddFactAsync(new Subscription(account, feed));

            await Synchronize();

            var feeds = await tablet.Individual.Feeds.EnsureAsync();

            Assert.AreEqual(1, feeds.Count());
            Assert.AreEqual("http://myblog.com/rss", feeds.Single().Url);
        }

        private async Task Synchronize()
        {
            while (await phone.Community.SynchronizeAsync() || await tablet.Community.SynchronizeAsync()) ;
        }
    }
}
