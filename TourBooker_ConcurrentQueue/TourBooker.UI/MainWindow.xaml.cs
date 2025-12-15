using AdvCShColls.TourBooker.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdvCShColls.TourBooker.UI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private AppData AllData { get; } = new AppData();

		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			AllData.Initialize(@"D:\Training\PopByLargest.csv");
			this.DataContext = AllData;
		}

		private void UpdateAllLists()
		{
			this.lbxItinerary.Items.Refresh();
			this.lbxToursToBook.Items.Refresh();
			this.lbxConfirmedBookedTours.Items.Refresh();
			this.lbxRequests.Items.Refresh();
			//			this.lbxRequests.ItemsSource = AllData.BookingRequests.ToList();
			this.lbxRequests.Items.Refresh();
			this.tbxNextBookingRequest.Text = GetLatestBookingRequestText();
		}


		private void btnAddToItinerary_Click(object sender, RoutedEventArgs e)
		{
			int selectedIndex = this.lbxAllCountries.SelectedIndex;
			if (selectedIndex == -1)
				return;

			Country selectedCountry = AllData.AllCountries[selectedIndex];
			AllData.ItineraryBuilder.AddLast(selectedCountry);
			var change = new ItineraryChange(
				ChangeType.Append, AllData.ItineraryBuilder.Count, selectedCountry);
			AllData.ChangeLog.Push(change);

			this.UpdateAllLists();
		}

		private void btnRemoveFromItinerary_Click(object sender, RoutedEventArgs e)
		{
			int selectedItinIndex = this.lbxItinerary.SelectedIndex;
			if (selectedItinIndex < 0)
				return;

			var nodeToRemove = AllData.ItineraryBuilder.GetNthNode(selectedItinIndex);
			AllData.ItineraryBuilder.Remove(nodeToRemove);
			var change = new ItineraryChange(
				ChangeType.Remove, selectedItinIndex, nodeToRemove.Value);
			AllData.ChangeLog.Push(change);

			this.UpdateAllLists();
		}

		private void btnInsertInItinerary_Click(object sender, RoutedEventArgs e)
		{
			int selectedIndex = this.lbxAllCountries.SelectedIndex;
			if (selectedIndex == -1)
				return;

			int selectedItinIndex = this.lbxItinerary.SelectedIndex;
			if (selectedItinIndex < 0)
				return;

			Country selectedCountry = AllData.AllCountries[selectedIndex];

			var insertBeforeNode = AllData.ItineraryBuilder.GetNthNode(selectedItinIndex);
			AllData.ItineraryBuilder.AddBefore(insertBeforeNode, selectedCountry);
			var change = new ItineraryChange(
				ChangeType.Insert, selectedItinIndex, selectedCountry);
			AllData.ChangeLog.Push(change);

			this.UpdateAllLists();
		}

		private void btnSaveTour_Click(object sender, RoutedEventArgs e)
		{
			string name = this.tbxTourName.Text.Trim();
			Country[] itinerary = AllData.ItineraryBuilder.ToArray();

			try
			{
				Tour newTour = new Tour(name, itinerary);
				AllData.AllTours.Add(newTour.Name, newTour);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Cannot save tour", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			AllData.ItineraryBuilder.Clear();
			this.tbxTourName.Text = null;
			this.UpdateAllLists();

			MessageBox.Show("Tour added", "Success");
		}

		private void btnUndo_Click(object sender, RoutedEventArgs e)
		{
			if (AllData.ChangeLog.Count == 0)
				return;

			ItineraryChange lastChange = AllData.ChangeLog.Pop();
			ChangeUndoer.Undo(AllData.ItineraryBuilder, lastChange);
			this.UpdateAllLists();
		}

		List<Tour> GetRequestedTours() => this.lbxToursToBook.SelectedItems.Cast<Tour>().ToList();

		private void lbxToursToBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			List<Tour> selectedTours = GetRequestedTours();
			StringBuilder sb = new StringBuilder();

			foreach (Tour tour in selectedTours)
			{
				sb.AppendLine($"{tour.Name}:");
				foreach (Country country in tour.Itinerary)
					sb.AppendLine($"   {country.Name}");
				sb.AppendLine();

			}
			this.tbxToursItinerary.Text = sb.ToString();
		}
		private void btnBookTour_Click(object sender, RoutedEventArgs e)
		{
			Customer customer = this.lbxCustomer.SelectedItem as Customer;
			if (customer == null)
			{
				MessageBox.Show("You must select which customer you are!");
				return;
			}

			List<Tour> requestedTours = GetRequestedTours();
			if (requestedTours.Count == 0)
			{
				MessageBox.Show("You must select a tour to book!", "No tour selected");
				return;
			}

			foreach (Tour tour in requestedTours)
			{
				this.AllData.BookingRequests.Enqueue((customer, tour));
			}
			MessageBox.Show($"{requestedTours.Count} tours requested", "Tours requested");
			this.UpdateAllLists();
		}

		private void btnApproveRequest_Click(object sender, RoutedEventArgs e)
		{
			if (AllData.BookingRequests.Count == 0)
				return;

			var request = AllData.BookingRequests.Dequeue();
			request.TheCustomer.BookedTours.Add(request.TheTour);
			this.UpdateAllLists();
		}

		private string GetLatestBookingRequestText()
		{
			if (AllData.BookingRequests.Count == 0)
				return null;
			else
				return AllData.BookingRequests.Peek().ToString();
		}

		private void lbxCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Customer customer = this.lbxCustomer.SelectedItem as Customer;
			this.gbxBookedTours.DataContext = customer;
		}
	}
}
