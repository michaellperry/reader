using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdateControls.Correspondence;

namespace Correspondence.Reader.Model
{
    public static class Client
    {
        public static Community Subscribe(Community community, Func<Individual> individual)
        {
            community.Subscribe(() => individual());
            community.Subscribe(() => AccountsOf(individual()));
            return community;
        }

        private static IEnumerable<Account> AccountsOf(Individual individual)
        {
            if (individual == null)
                return new List<Account>();

            return individual.Accounts;
        }
    }
}
