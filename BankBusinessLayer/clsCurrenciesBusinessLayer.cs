using BankDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBusinessLayer
{
    public class clsCurrenciesBusinessLayer
    {
        public string Code { get; set; }
        public double Value { get; set; }

        private clsCurrenciesBusinessLayer(string code, double value)
        {
            this.Code = code;
            this.Value = value;
        }
        public clsCurrenciesBusinessLayer()
        {
            this.Code = string.Empty;
            this.Value=.0d;
        }
        public static DataTable GetCurrencies()
        {
            return clsCurrenciesDataAccessLayer.GetCurrencies(); 
        }

        public static clsCurrenciesBusinessLayer Find(string CurrencyCode)
        {
            double CurrencyValue =.0d;
            if (clsCurrenciesDataAccessLayer.Find(CurrencyCode, ref CurrencyValue))
            {
                return new clsCurrenciesBusinessLayer(CurrencyCode, CurrencyValue);
            }
            else
                return null;
        }


    }
}
