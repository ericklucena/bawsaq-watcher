using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BawsaqWatcher
{
    class StockRepository
    {
        private static StockRepository instance;
        public Stock[] StocksPs3 {get; set;}
        public Stock[] StocksXbox { get; set; }

        public static StockRepository getInstance()
        {
            if (instance == null)
            {
                instance = new StockRepository();
            }
            return instance;
        }

        public void setStocksPs3(List<Stock> stocks)
        {
            this.StocksPs3 = stocks.ToArray();
        }

        public void setStocksXbox(List<Stock> stocks)
        {
            this.StocksXbox = stocks.ToArray();
        }

        public Stock getStockPs3(int index)
        {
            return StocksPs3[index];
        }

        public Stock getStockXbox(int index)
        {
            return StocksXbox[index];
        }

    }
}
