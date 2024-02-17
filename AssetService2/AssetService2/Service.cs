using log4net;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceProcess;
using System.Timers;

namespace AssetService2
{
    public partial class Service : ServiceBase
    {
        public static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Timer _timer;

        public Service()
        {
            InitializeComponent();
        }

        public void TimerCheckNewPrices()
        {
            double outDble;

            _timer = new Timer();
            _timer.Elapsed += LoadNewPrices;
            _timer.Interval = double.TryParse(ConfigurationManager.AppSettings["timerPeriodCheck"], out outDble) ? outDble : 60000; // in mSec.
            _timer.Enabled = true;
        }

        private void LoadNewPrices(object sender, ElapsedEventArgs e)
        {
            _timer.Enabled = false;
            
            var assetList = new List<string> { "USDTBRL", "BTCUSDT", "ETHUSDT", "SOLUSDT", "ADAUSDT", "DOTUSDT" };
            foreach (var asset in assetList)
            {
                AssetPriceHandler.Handle(asset);
            }

            _timer.Enabled = true;
        }

        protected override void OnStart(string[] args)
        {
            Log.Info("OnStart: Checking New Prices");
            TimerCheckNewPrices();
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        //protected override void OnStop()
        //{
        //}
    }
}
