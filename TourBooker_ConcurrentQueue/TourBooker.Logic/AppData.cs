using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvCShColls.TourBooker.Logic
{
	public class AppData
	{
		public List<Country> AllCountries { get; private set; }
		public Dictionary<CountryCode, Country> AllCountriesByKey { get; private set; }
		public List<Customer> Customers { get; private set; }
			 = new List<Customer>() { new Customer("Simon"), new Customer("Kim") };
		public Queue<(Customer TheCustomer, Tour TheTour)> BookingRequests { get; }
			= new Queue<(Customer, Tour)>();
		public LinkedList<Country> ItineraryBuilder { get; } = new LinkedList<Country>();
		public SortedDictionary<string, Tour> AllTours { get; private set; } 
			= new SortedDictionary<string, Tour>();
		public Stack<ItineraryChange> ChangeLog { get; } = new Stack<ItineraryChange>();

		public void Initialize(string csvFilePath)
		{
			CsvReader reader = new CsvReader(csvFilePath);
			this.AllCountries = reader.ReadAllCountries().OrderBy(x=>x.Name).ToList();
			var dict = AllCountries.ToDictionary(x => x.Code);
			this.AllCountriesByKey = dict;
		}

	}
}
