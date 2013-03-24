using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UpdateControls.Correspondence.Strategy;
using System.Threading.Tasks;
using UpdateControls.Correspondence;
using UpdateControls.Correspondence.Memory;

namespace Correspondence.Reader.Model.Test
{
    public class Host
    {
        private Community community;
        private FeedService service;

        public async Task InitializeAsync(ICommunicationStrategy communication)
        {
            community = new Community(new MemoryStorageStrategy())
                .AddCommunicationStrategy(communication)
                .Register<CorrespondenceModel>()
                ;

            Server.Subscribe(community, () => service);

            service = await community.AddFactAsync(new FeedService());
        }

        public Community Community
        {
            get { return community; }
        }

        public FeedService Service
        {
            get { return service; }
        }
    }
}
