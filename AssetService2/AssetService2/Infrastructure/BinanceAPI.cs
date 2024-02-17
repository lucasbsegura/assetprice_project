using AssetService2.Models;
using log4net;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace AssetService2.Infrastructure
{
    internal class BinanceAPI
    {
        public static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static async Task<Asset> Get(string url, string pair)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(string.Format("{0}{1}", url, pair));
            var result = response.Content.ReadAsStringAsync().Result;
            var asset = JsonConvert.DeserializeObject<Asset>(result);
            Log.Info($"Get price for {pair} : {asset.Price}");
            return asset;
        }
    }
}
