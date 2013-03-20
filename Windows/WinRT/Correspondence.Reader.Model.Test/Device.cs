using System.Threading.Tasks;
using UpdateControls.Correspondence;
using UpdateControls.Correspondence.Memory;

namespace Correspondence.Reader.Model.Test
{
    class Device
    {
        private readonly string name;

        private Community community;
        private Individual individual;
        
        public Device(string name)
        {
            this.name = name;
        }

        public async Task InitializeAsync(MemoryCommunicationStrategy communication, string identifier)
        {
            community = new Community(new MemoryStorageStrategy())
                .AddCommunicationStrategy(communication)
                .Register<CorrespondenceModel>()
                ;

            Client.Subscribe(community, () => individual);

            individual = await community.AddFactAsync(new Individual(name));
            var account = await community.AddFactAsync(new Account(identifier));
            var attach = await community.AddFactAsync(new Attach(individual, account));
        }

        public Community Community
        {
            get { return community; }
        }

        public Individual Individual
        {
            get { return individual; }
        }
    }
}
