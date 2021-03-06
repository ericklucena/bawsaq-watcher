﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Newtonsoft.Json;

using System.Text;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Navigation;
using Microsoft.Phone.Tasks;

namespace BawsaqWatcher
{
    public partial class MainPage : PhoneApplicationPage
    {

        bool error = false;

        public MainPage()
        {
            InitializeComponent();
            StockRepository.getInstance();

            loadStocks();
           
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Handle selection changed on ListBox
        private void ps3Stocks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected index is -1 (no selection) do nothing
            if (ps3Stocks.SelectedIndex == -1)
                return;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/DetailsPage.xaml?platform=ps3&selectedItem=" + ps3Stocks.SelectedIndex, UriKind.Relative));

            // Reset selected index to -1 (no selection)
            ps3Stocks.SelectedIndex = -1;
        }

        // Handle selection changed on ListBox
        private void xboxStocks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected index is -1 (no selection) do nothing
            if (xboxStocks.SelectedIndex == -1)
                return;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/DetailsPage.xaml?platform=xbox&selectedItem=" + xboxStocks.SelectedIndex, UriKind.Relative));

            // Reset selected index to -1 (no selection)
            xboxStocks.SelectedIndex = -1;
        }

        public void loadStocks()
        {
            WebClient webClientPs3 = new WebClient();
            WebClient webClientXbox = new WebClient();

            webClientPs3.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClientPs3_DownloadStringCompleted);
            webClientPs3.DownloadStringAsync(new Uri("http://socialclub.rockstargames.com/games/gtav/ajax/stockdetail?platFormId=2"));

            webClientXbox.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClientXbox_DownloadStringCompleted);
            webClientXbox.DownloadStringAsync(new Uri("http://socialclub.rockstargames.com/games/gtav/ajax/stockdetail?platFormId=1"));
        }

        private void webClientPs3_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string jsonInput = e.Result;
                saveOnFile(jsonInput, "ps3.json");
                var rootObject = JsonConvert.DeserializeObject<RootObject>(jsonInput);
                var stocks = rootObject.Stocks;

                StockRepository.getInstance().setStocksPs3(stocks);

                ps3Stocks.ItemsSource = StockRepository.getInstance().StocksPs3;
            }
            catch (Exception ex)
            {
                if (!error)
                {
                    MessageBox.Show("No internet connection. Please verify your phone's connectivity and then hit the reload button.", "WASTED!", MessageBoxButton.OK);
                    error = true;

                    try
                    {
                        loadOfflineStocks();
                        MessageBox.Show("You are viewing the last loaded version of the stocks. Check your internet connection and then hit the reload button.");
                    }
                    catch (Exception ex2)
                    {
                        MessageBox.Show("There's no cached information of the BAWSAQ stocks. Please, check your internet connection and then hit the reload button.", "BUSTED!", MessageBoxButton.OK);
                    }

                    
                }
            }
        }

        private void webClientXbox_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string jsonInput = e.Result;
                saveOnFile(jsonInput, "xbox.json");
                var rootObject = JsonConvert.DeserializeObject<RootObject>(jsonInput);
                var stocks = rootObject.Stocks;

                StockRepository.getInstance().setStocksXbox(stocks);

                xboxStocks.ItemsSource = StockRepository.getInstance().StocksXbox;
            }
            catch (Exception ex)
            {
                if (!error)
                {
                    MessageBox.Show("No internet connection. Please verify your phone's connectivity and then hit the reload button.", "WASTED!", MessageBoxButton.OK);
                    error = true;

                    try
                    {
                        loadOfflineStocks();
                        MessageBox.Show("You are viewing the last loaded version of the stocks. Check your internet connection and then hit the reload button.");
                    }
                    catch (Exception ex2)
                    {
                        MessageBox.Show("There's no cached information of the BAWSAQ stocks. Please, check your internet connection and then hit the reload button.", "BUSTED!", MessageBoxButton.OK);
                    }
                }
            }
        }

        public void loadOfflineStocks()
        {
            var rootObject = JsonConvert.DeserializeObject<RootObject>(readFromFile("ps3.json"));
            var stocks = rootObject.Stocks;
            StockRepository.getInstance().setStocksPs3(stocks);
            ps3Stocks.ItemsSource = StockRepository.getInstance().StocksPs3;

            rootObject = JsonConvert.DeserializeObject<RootObject>(readFromFile("xbox.json"));
            stocks = rootObject.Stocks;
            StockRepository.getInstance().setStocksXbox(stocks);
            xboxStocks.ItemsSource = StockRepository.getInstance().StocksXbox;
        }

        public void saveOnFile(string data, string filename)
        {
            IsolatedStorageFile myFile = IsolatedStorageFile.GetUserStoreForApplication();
            StreamWriter sw = new StreamWriter(new IsolatedStorageFileStream(filename, FileMode.Create, myFile));
            sw.WriteLine(data); //Wrting to the file
            sw.Close();

        }

        public string readFromFile(string filename)
        {
            IsolatedStorageFile myFile = IsolatedStorageFile.GetUserStoreForApplication();
            try
            {

                StreamReader reader = new StreamReader(new IsolatedStorageFileStream(filename, FileMode.Open, myFile));
                string rawData = reader.ReadToEnd();
                reader.Close();
                return rawData;
            }
            catch { }

            return "";
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void Reload_Click(object sender, EventArgs e)
        {
            error = false;
            loadStocks();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var askforReview = (bool)IsolatedStorageSettings.ApplicationSettings["askforreview"];
            if (askforReview)
            {
                //make sure we only ask once! 
                IsolatedStorageSettings.ApplicationSettings["askforreview"] = false;
                var returnvalue = MessageBox.Show("Thank you for using BAWSAQ Watcher for a while now, would you like to review this app?", "Please review my app", MessageBoxButton.OKCancel);
                if (returnvalue == MessageBoxResult.OK)
                {
                    var marketplaceReviewTask = new MarketplaceReviewTask();
                    marketplaceReviewTask.Show();
                }
            }
        }

        private void AppBarAbout_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }

    }
}