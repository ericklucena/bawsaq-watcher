using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BawsaqWatcher
{
    // Class for storing information about an activity
    public class StockHistoryInfo
    {
        public string Day { get; set; }
        public double Value { get; set; }

        public StockHistoryInfo(string day, double value)
        {
            Day = day;
            Value = value;
        }
    }

    // Class for storing activities
    public class StockHistory : List<StockHistoryInfo>
    {
        public StockHistory()
        {
           
        }
    }
}
