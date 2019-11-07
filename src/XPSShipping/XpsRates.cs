using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace XPSShipping
{
    /// <summary>
    /// Summary description for XpsRates
    /// </summary>
    public class XpsRates
    {
        private readonly string _apiKey;
        private readonly string _customerId;

        public XpsRates(string apiKey, string customerId)
        {
            _apiKey = apiKey;
            _customerId = customerId;
        }

        public XpsRateQuotes Quotes(dynamic pkg) // change from dynamic to custom class
        {
            XpsRateQuotes quotes = new XpsRateQuotes();
            var services = GetServices();
            foreach (var s in services.Services)
            {
                if (s.CarrierCode == "dhl") // they don't want to use DHL at all
                    continue;
                quotes.Add(GetQuote(pkg));
            }

            return quotes;
        }

        private XpsRateQuote GetQuote(dynamic pkg) // change from dynamic to custom class (doesn't have to be the same type as Quotes())
        {
            byte[] body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(pkg));
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://xpsshipper.com/restapi/v1/customers/" + _customerId + "/quote");
            req.Method = "POST";
            req.ContentType = "application/json";
            req.Headers.Add("Authorization", "RSIS " + _apiKey);
            req.Accept = "application/json";
            req.ContentLength = body.Length;

            using (Stream s = req.GetRequestStream())
                s.Write(body, 0, body.Length);

            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                return JsonConvert.DeserializeObject<XpsRateQuote>(sr.ReadToEnd());
        }

        private XpsServices GetServices()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://xpsshipper.com/restapi/v1/customers/" + _customerId + "/services");
            req.Method = "GET";
            req.ContentType = "application/json";
            req.Headers.Add("Authorization", "RSIS " + _apiKey);
            req.Accept = "application/json";

            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                return XpsServices.FromJson(sr.ReadToEnd());
        }
    }

    public class XpsRateQuotes : List<XpsRateQuote>
    { }

    public class XpsRateQuote
    {
        public string Currency { get; set; }
        public string CustomsCurrency { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal BaseAmount { get; set; }
        public XpsSurcharge[] Surcharges { get; set; }
        public string Zone { get; set; }
        public XpsQuotePiece[] Pieces { get; set; }
    }
    public class XpsSurcharge
    {
        public string Description { get; set; }

        public decimal Amount { get; set; }
    }
    public class XpsQuotePiece
    {
        public decimal TotalAmount { get; set; }
        public decimal BaseAmount { get; set; }
        public XpsSurcharge[] Surcharges { get; set; }
    }
}