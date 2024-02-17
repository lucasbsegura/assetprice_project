using AssetService2.Infrastructure;
using log4net;
using System;

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
                var currency = (result.Symbol == "USDTBRL" ? "BRL" : result.Symbol.Substring(0, 3));
                Mosquitto.Publish(string.Format("{0}:US${1}", currency, result.Price));
            }
            catch (Exception ex)
            {
                Log.Info($"Error getting value for {asset}");
                Log.Info($"Error: {ex.Message}");
            }
        }
    }
}
