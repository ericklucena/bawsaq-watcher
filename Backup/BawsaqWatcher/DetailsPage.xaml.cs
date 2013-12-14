using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Newtonsoft.Json;
using System.Windows.Controls.DataVisualization.Charting;

namespace BawsaqWatcher
{
    public partial class DetailsPage : PhoneApplicationPage
    {
        // Constructor
        public DetailsPage()
        {
            InitializeComponent();
            
        }

        // When page is navigated to set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string selectedIndex = "";
            string platform;
            NavigationContext.QueryString.TryGetValue("platform", out platform);
            if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex))
            {
                int index = int.Parse(selectedIndex);
                Stock s = null;
                if(platform.Equals("ps3")){
                    s = StockRepository.getInstance().getStockPs3(index);
                }else{
                    s = StockRepository.getInstance().getStockXbox(index);
                }

                Uri uri = new Uri("/stockLogos/stock-"+s.CompanyCode+".png", UriKind.Relative);
                ImageSource imgSource = new BitmapImage(uri);
                StockLogo.Source = imgSource;    

                StockHistory h = s.getHistory();
                ((LineSeries)myChart.Series[0]).ItemsSource = h;

                if (s.PriceMovementDirection.Equals("down"))
                {
                    PriceMovementDirection.Foreground = new SolidColorBrush(Color.FromArgb(255, 204, 0, 0));
                    PriceMovement.Foreground = new SolidColorBrush(Color.FromArgb(255, 204, 0, 0));
                    PriceMovementPercent.Foreground = new SolidColorBrush(Color.FromArgb(255, 204, 0, 0));
                    s.PriceMovementDirection = "6";
                }
                else
                {
                    PriceMovementDirection.Foreground = new SolidColorBrush(Color.FromArgb(255, 108, 148, 56));
                    PriceMovement.Foreground = new SolidColorBrush(Color.FromArgb(255, 108, 148, 56));
                    PriceMovementPercent.Foreground = new SolidColorBrush(Color.FromArgb(255, 108, 148, 56));
                    s.PriceMovementDirection = "5";
                }
                DataContext = s;
                //NavigationService.Navigate(new Uri("/Page1.xaml", UriKind.Relative));
            }
        }
    }
}