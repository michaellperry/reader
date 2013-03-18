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
        private Device _phone;
        private Device _tablet;

        [TestInitialize]
        public async Task Initialize()
        {
            var sharedCommunication = new MemoryCommunicationStrategy();
            string identifier = Guid.NewGuid().ToString();

            _phone  = new Device("phone");
            _tablet = new Device("tablet");

            await _phone.InitializeAsync (sharedCommunication, identifier);
            await _tablet.InitializeAsync(sharedCommunication, identifier);
        }

        [TestMethod]
        public async Task ModelTest_SharedAccount()
        {
            var phoneAccont =  (await _phone .Individual.Accounts.EnsureAsync()).FirstOrDefault();
            var tabletAccont = (await _tablet.Individual.Accounts.EnsureAsync()).FirstOrDefault();

            Assert.IsNotNull(phoneAccont);
            Assert.IsNotNull(tabletAccont);
            Assert.AreEqual(phoneAccont.Identifier, tabletAccont.Identifier);
        }

        [TestMethod]
        public async Task ModelTest_SubscribeToFeed_SeeSubscription()
        {
            var account = (await _phone.Individual.Accounts.EnsureAsync()).FirstOrDefault();
            var feed = await _phone.Community.AddFactAsync(new Feed("http://myblog.com/rss"));
            var subscription = await _phone.Community.AddFactAsync(new Subscription(account, feed));

            var feeds = await _phone.Individual.Feeds.EnsureAsync();

            Assert.AreEqual(1, feeds.Count());
            Assert.AreEqual("http://myblog.com/rss", feeds.Single().Url);
        }

        [TestMethod]
        public async Task ModelTest_SubscribeToFeed_SharedBetweenDevices()
        {
            var account = (await _phone.Individual.Accounts.EnsureAsync()).FirstOrDefault();
            var feed = await _phone.Community.AddFactAsync(new Feed("http://myblog.com/rss"));
            var subscription = await _phone.Community.AddFactAsync(new Subscription(account, feed));

            await Synchronize();

            var feeds = await _tablet.Individual.Feeds.EnsureAsync();

            Assert.AreEqual(1, feeds.Count());
            Assert.AreEqual("http://myblog.com/rss", feeds.Single().Url);
        }

        private async Task Synchronize()
        {
            while (await _phone.Community.SynchronizeAsync() || await _tablet.Community.SynchronizeAsync()) ;
        }
    }
}
