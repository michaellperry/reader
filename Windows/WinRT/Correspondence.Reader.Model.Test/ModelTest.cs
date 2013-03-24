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
        private Host server;

        [TestInitialize]
        public async Task Initialize()
        {
            var sharedCommunication = new MemoryCommunicationStrategy();
            string identifier = Guid.NewGuid().ToString();

            phone  = new Device("phone");
            tablet = new Device("tablet");
            server = new Host();

            await phone.InitializeAsync (sharedCommunication, identifier);
            await tablet.InitializeAsync(sharedCommunication, identifier);
            await server.InitializeAsync(sharedCommunication);
        }

        [TestMethod]
        public void ModelTest_SharedAccount()
        {
            var phoneAccont =  phone .Individual.Accounts.FirstOrDefault();
            var tabletAccont = tablet.Individual.Accounts.FirstOrDefault();

            Assert.IsNotNull(phoneAccont);
            Assert.IsNotNull(tabletAccont);
            Assert.AreEqual(phoneAccont.Identifier, tabletAccont.Identifier);
        }

        [TestMethod]
        public async Task ModelTest_SubscribeToFeed_SeeSubscription()
        {
            await SubscribeToMyBlogOnPhoneAsync();

            Assert.AreEqual(1, phone.Individual.Feeds.Count());
            Assert.AreEqual("http://myblog.com/rss", phone.Individual.Feeds.Single().Url);
        }

        [TestMethod]
        public async Task ModelTest_SubscribeToFeed_SharedBetweenDevices()
        {
            await SubscribeToMyBlogOnPhoneAsync();

            await Synchronize();

            Assert.AreEqual(1, tablet.Individual.Feeds.Count());
            Assert.AreEqual("http://myblog.com/rss", tablet.Individual.Feeds.Single().Url);
        }

        [TestMethod]
        public async Task ModelTest_PublishArticle_SharedToDevices()
        {
            await SubscribeToMyBlogOnPhoneAsync();

            await Synchronize();

            Assert.AreEqual(1, server.Service.Feeds.Count());
            await server.Community.AddFactAsync(new Article(server.Service.Feeds.Single(), "http://myblog.com/articles/1"));

            await Synchronize();

            Assert.AreEqual(1, phone.Individual.Feeds.Count());
            Feed feed = phone.Individual.Feeds.Single();
            Assert.AreEqual(1, feed.Articles.Count());
            Assert.AreEqual("http://myblog.com/articles/1", feed.Articles.Single().Url);
        }

        private async Task Synchronize()
        {
            while (
                await phone.Community.SynchronizeAsync() ||
                await tablet.Community.SynchronizeAsync() ||
                await server.Community.SynchronizeAsync()) ;
        }

        private async Task SubscribeToMyBlogOnPhoneAsync()
        {
            var account = phone.Individual.Accounts.Single();
            var service = await phone.Community.AddFactAsync(new FeedService());
            var feed = await phone.Community.AddFactAsync(new Feed(service, "http://myblog.com/rss"));
            var subscription = await phone.Community.AddFactAsync(new Subscription(account, feed));
        }
    }
}
