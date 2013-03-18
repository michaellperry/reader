using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdateControls.Correspondence;
using UpdateControls.Correspondence.Memory;
using UpdateControls.Correspondence.Strategy;

namespace Correspondence.Reader.Model.Test
{
    class Device
    {
        private readonly string _name;

        private Community _community;
        private Individual _individual;
        
        public Device(string name)
        {
            _name = name;
        }

        public async Task InitializeAsync(MemoryCommunicationStrategy communication, string identifier)
        {
            _community = new Community(new MemoryStorageStrategy())
                .AddCommunicationStrategy(communication)
                .Register<CorrespondenceModel>()
                ;

            Client.Subscribe(_community, () => _individual);

            _individual = await _community.AddFactAsync(new Individual(_name));
            var account = await _community.AddFactAsync(new Account(identifier));
            var attach = await _community.AddFactAsync(new Attach(_individual, account));
        }

        public Community Community
        {
            get { return _community; }
        }

        public Individual Individual
        {
            get { return _individual; }
        }
    }
}
