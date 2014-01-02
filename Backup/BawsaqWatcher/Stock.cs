using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BawsaqWatcher
{
    public class Stock
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public int Index { get; set; }
        public string CurrentPrice { get; set; }
        public string HighPrice { get; set; }
        public string LowPrice { get; set; }
        public string PriceMovement { get; set; }
        public double PriceMovementPercent { get; set; }
        public string PriceMovementDirection { get; set; }
        public string TotalPrice { get; set; }
        public double TotalPriceMovement { get; set; }
        public double TotalPriceMovementPercent { get; set; }
        public string TotalPriceMovementDirection { get; set; }
        public List<object> Modifiers { get; set; }
        public string PriceHistory { get; set; }
        public bool IsLoggedIn { get; set; }
        public string CompanyCodeOld { get; set; }

        public StockHistory getHistory()
        {
            string toParse = PriceHistory;
            int start = 0;
            int end = toParse.IndexOf(',');
            StockHistory activities = new StockHistory();

            for (int i = 0; i < 7; i++)
            {
                string day = DateTime.Now.AddDays(-(i)).DayOfWeek.ToString().Substring(0,3);
                double value = Double.Parse(toParse.Substring(start, end-start));
                activities.Add(new StockHistoryInfo(day, value));
                start = end+1;
                
                if(i<5){
                    end = toParse.IndexOf(',', start);
                }else{
                    end = toParse.Length;
                }
            }

            activities.Reverse();

            return activities;
        }

    }
}
