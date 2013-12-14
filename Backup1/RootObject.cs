using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BawsaqWatcher
{
    public class RootObject
    {
        public List<Stock> Stocks { get; set; }
        public List<SubNavLink> SubNavLinks { get; set; }
        public bool IsLoggedIn { get; set; }
        public bool Status { get; set; }
    }
}
