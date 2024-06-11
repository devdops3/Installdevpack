using System.Configuration;

namespace MA2AAPI.Constants
{
    public class MobileApiConstants
    {

        public static readonly string JooxTaxId = ConfigurationManager.AppSettings["JooxTaxId"].ToString();

        public static readonly string OKDollar = ConfigurationManager.AppSettings["OK$"].ToString();

        public static readonly string EasyMicrofinanceTaxId = ConfigurationManager.AppSettings["EasyMicrofinanceTaxId"].ToString();

        public static readonly string MobileVersion = ConfigurationManager.AppSettings["MobileVersion"].ToString();

        public static readonly string iTunesTaxId = ConfigurationManager.AppSettings["iTunesTaxId"].ToString();

        public static readonly string GooglePlayTaxId = ConfigurationManager.AppSettings["GooglePlayTaxId"].ToString();
        public static readonly string MyPlayTaxId = ConfigurationManager.AppSettings["MyPlayTaxId"].ToString();
        public static readonly string SteamWalletTaxId = ConfigurationManager.AppSettings["SteamWalletTaxId"].ToString();

        public static readonly string dash = "-";


    }
}