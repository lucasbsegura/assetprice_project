using AssetService2.Infrastructure;
using log4net;
using System;
using System.Globalization;

namespace AssetService2
{
    internal class AssetPriceHandler
    {
        public static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void Handle(string asset)
        {
            try
            {
                var result = BinanceAPI.Get("https://api.binance.com/api/v3/ticker/price?symbol=", asset).Result;
                var currencySymbol = (result.Symbol == "USDTBRL" ? "R$" : "US$");
                var cultureInfo = (result.Symbol == "USDTBRL" ? "pt-BR" : "en-US");
                Mosquitto.Publish(string.Format("{0} - {1}{2}", result.Symbol.Substring(0, 3), currencySymbol, result.Price.ToString("N2", new CultureInfo(cultureInfo))));
            }
            catch (Exception ex)
            {
                Log.Info($"Error getting value for {asset}");
                Log.Info($"Error: {ex.Message}");
            }
        }
    }
}
