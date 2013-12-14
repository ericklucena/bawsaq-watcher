using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Controls.DataVisualization.Charting;

namespace BawsaqWatcher
{
    public partial class Page1 : PhoneApplicationPage
    {

        public Page1()
        {
            InitializeComponent();
            StockHistory h = new StockHistory();
            h.Add(new StockHistoryInfo("seg", 100));
            h.Add(new StockHistoryInfo("ter", 50));

            ((LineSeries)myChart.Series[0]).ItemsSource = StockRepository.getInstance().getStockPs3(0).getHistory();       
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

    }
}