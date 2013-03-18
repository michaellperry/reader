using System.Collections.Generic;
using System.Linq;
using UpdateControls.Correspondence;
using UpdateControls.Correspondence.Mementos;
using UpdateControls.Correspondence.Strategy;
using System;
using System.IO;

/**
/ For use with http://graphviz.org/
digraph "Correspondence.Reader.Model"
{
    rankdir=BT
    Attach -> Individual [color="red"]
    Attach -> Account
    Subscription -> Account [color="red"]
    Subscription -> Feed
}
**/

namespace Correspondence.Reader.Model
{
    public partial class Individual : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Individual newFact = new Individual(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._anonymousId = (string)_fieldSerializerByType[typeof(string)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Individual fact = (Individual)obj;
				_fieldSerializerByType[typeof(string)].WriteData(output, fact._anonymousId);
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"Correspondence.Reader.Model.Individual", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles

        // Queries
        public static Query MakeQueryAccounts()
		{
			return new Query()
				.JoinSuccessors(Attach.RoleIndividual)
				.JoinPredecessors(Attach.RoleAccount)
            ;
		}
        public static Query QueryAccounts = MakeQueryAccounts();
        public static Query MakeQueryFeeds()
		{
			return new Query()
				.JoinSuccessors(Attach.RoleIndividual)
				.JoinPredecessors(Attach.RoleAccount)
				.JoinSuccessors(Subscription.RoleAccount)
				.JoinPredecessors(Subscription.RoleFeed)
            ;
		}
        public static Query QueryFeeds = MakeQueryFeeds();

        // Predicates

        // Predecessors

        // Fields
        private string _anonymousId;

        // Results
        private Result<Account> _accounts;
        private Result<Feed> _feeds;

        // Business constructor
        public Individual(
            string anonymousId
            )
        {
            InitializeResults();
            _anonymousId = anonymousId;
        }

        // Hydration constructor
        private Individual(FactMemento memento)
        {
            InitializeResults();
        }

        // Result initializer
        private void InitializeResults()
        {
            _accounts = new Result<Account>(this, QueryAccounts);
            _feeds = new Result<Feed>(this, QueryFeeds);
        }

        // Predecessor access

        // Field access
        public string AnonymousId
        {
            get { return _anonymousId; }
        }

        // Query result access
        public Result<Account> Accounts
        {
            get { return _accounts; }
        }
        public Result<Feed> Feeds
        {
            get { return _feeds; }
        }

        // Mutable property access

    }
    
    public partial class Account : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Account newFact = new Account(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._identifier = (string)_fieldSerializerByType[typeof(string)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Account fact = (Account)obj;
				_fieldSerializerByType[typeof(string)].WriteData(output, fact._identifier);
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"Correspondence.Reader.Model.Account", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles

        // Queries

        // Predicates

        // Predecessors

        // Fields
        private string _identifier;

        // Results

        // Business constructor
        public Account(
            string identifier
            )
        {
            InitializeResults();
            _identifier = identifier;
        }

        // Hydration constructor
        private Account(FactMemento memento)
        {
            InitializeResults();
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access

        // Field access
        public string Identifier
        {
            get { return _identifier; }
        }

        // Query result access

        // Mutable property access

    }
    
    public partial class Attach : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Attach newFact = new Attach(memento);


				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Attach fact = (Attach)obj;
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"Correspondence.Reader.Model.Attach", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RoleIndividual = new Role(new RoleMemento(
			_correspondenceFactType,
			"individual",
			new CorrespondenceFactType("Correspondence.Reader.Model.Individual", 1),
			true));
        public static Role RoleAccount = new Role(new RoleMemento(
			_correspondenceFactType,
			"account",
			new CorrespondenceFactType("Correspondence.Reader.Model.Account", 1),
			false));

        // Queries

        // Predicates

        // Predecessors
        private PredecessorObj<Individual> _individual;
        private PredecessorObj<Account> _account;

        // Fields

        // Results

        // Business constructor
        public Attach(
            Individual individual
            ,Account account
            )
        {
            InitializeResults();
            _individual = new PredecessorObj<Individual>(this, RoleIndividual, individual);
            _account = new PredecessorObj<Account>(this, RoleAccount, account);
        }

        // Hydration constructor
        private Attach(FactMemento memento)
        {
            InitializeResults();
            _individual = new PredecessorObj<Individual>(this, RoleIndividual, memento);
            _account = new PredecessorObj<Account>(this, RoleAccount, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public Individual Individual
        {
            get { return _individual.Fact; }
        }
        public Account Account
        {
            get { return _account.Fact; }
        }

        // Field access

        // Query result access

        // Mutable property access

    }
    
    public partial class Feed : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Feed newFact = new Feed(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._url = (string)_fieldSerializerByType[typeof(string)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Feed fact = (Feed)obj;
				_fieldSerializerByType[typeof(string)].WriteData(output, fact._url);
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"Correspondence.Reader.Model.Feed", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles

        // Queries

        // Predicates

        // Predecessors

        // Fields
        private string _url;

        // Results

        // Business constructor
        public Feed(
            string url
            )
        {
            InitializeResults();
            _url = url;
        }

        // Hydration constructor
        private Feed(FactMemento memento)
        {
            InitializeResults();
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access

        // Field access
        public string Url
        {
            get { return _url; }
        }

        // Query result access

        // Mutable property access

    }
    
    public partial class Subscription : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Subscription newFact = new Subscription(memento);


				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Subscription fact = (Subscription)obj;
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"Correspondence.Reader.Model.Subscription", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RoleAccount = new Role(new RoleMemento(
			_correspondenceFactType,
			"account",
			new CorrespondenceFactType("Correspondence.Reader.Model.Account", 1),
			true));
        public static Role RoleFeed = new Role(new RoleMemento(
			_correspondenceFactType,
			"feed",
			new CorrespondenceFactType("Correspondence.Reader.Model.Feed", 1),
			false));

        // Queries

        // Predicates

        // Predecessors
        private PredecessorObj<Account> _account;
        private PredecessorObj<Feed> _feed;

        // Fields

        // Results

        // Business constructor
        public Subscription(
            Account account
            ,Feed feed
            )
        {
            InitializeResults();
            _account = new PredecessorObj<Account>(this, RoleAccount, account);
            _feed = new PredecessorObj<Feed>(this, RoleFeed, feed);
        }

        // Hydration constructor
        private Subscription(FactMemento memento)
        {
            InitializeResults();
            _account = new PredecessorObj<Account>(this, RoleAccount, memento);
            _feed = new PredecessorObj<Feed>(this, RoleFeed, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public Account Account
        {
            get { return _account.Fact; }
        }
        public Feed Feed
        {
            get { return _feed.Fact; }
        }

        // Field access

        // Query result access

        // Mutable property access

    }
    

	public class CorrespondenceModel : ICorrespondenceModel
	{
		public void RegisterAllFactTypes(Community community, IDictionary<Type, IFieldSerializer> fieldSerializerByType)
		{
			community.AddType(
				Individual._correspondenceFactType,
				new Individual.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Individual._correspondenceFactType }));
			community.AddQuery(
				Individual._correspondenceFactType,
				Individual.QueryAccounts.QueryDefinition);
			community.AddQuery(
				Individual._correspondenceFactType,
				Individual.QueryFeeds.QueryDefinition);
			community.AddType(
				Account._correspondenceFactType,
				new Account.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Account._correspondenceFactType }));
			community.AddType(
				Attach._correspondenceFactType,
				new Attach.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Attach._correspondenceFactType }));
			community.AddType(
				Feed._correspondenceFactType,
				new Feed.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Feed._correspondenceFactType }));
			community.AddType(
				Subscription._correspondenceFactType,
				new Subscription.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Subscription._correspondenceFactType }));
		}
	}
}
