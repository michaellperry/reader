using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdateControls.Correspondence;

namespace Correspondence.Reader.Model
{
    public static class Server
    {
        public static void Subscribe(Community community, Func<FeedService> service)
        {
            community.Subscribe(() => service());
        }
    }
}
