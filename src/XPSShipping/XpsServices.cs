using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace XPSShipping
{
    public partial class XpsServices
    {
        [JsonProperty("services")]
        public Service[] Services { get; set; }
    }

    public partial class Service
    {
        [JsonProperty("carrierCode")]
        public string CarrierCode { get; set; }

        [JsonProperty("carrierLabel")]
        public string CarrierLabel { get; set; }

        [JsonProperty("serviceLabel")]
        public string ServiceLabel { get; set; }

        [JsonProperty("serviceCode")]
        public string ServiceCode { get; set; }

        [JsonProperty("inbound")]
        public bool Inbound { get; set; }

        [JsonProperty("supportedCountries")]
        public string[] SupportedCountries { get; set; }

        [JsonProperty("unsupportedCountries")]
        public string[] UnsupportedCountries { get; set; }

        [JsonProperty("signatureOptions")]
        public SignatureOption[] SignatureOptions { get; set; }

        [JsonProperty("packageTypes")]
        public PackageType[] PackageTypes { get; set; }
    }

    public partial class PackageType
    {
        [JsonProperty("packageTypeCode")]
        public string PackageTypeCode { get; set; }

        [JsonProperty("packageTypeLabel")]
        public string PackageTypeLabel { get; set; }

        [JsonProperty("multipieceSupported")]
        public bool MultipieceSupported { get; set; }

        [JsonProperty("weightUnit")]
        public string WeightUnit { get; set; }

        [JsonProperty("maxWeightPerPiece")]
        public object MaxWeightPerPiece { get; set; }

        [JsonProperty("dimType")]
        public object DimType { get; set; }

        [JsonProperty("dimUnit")]
        public string DimUnit { get; set; }

        [JsonProperty("presetLength")]
        public object PresetLength { get; set; }

        [JsonProperty("presetWidth")]
        public object PresetWidth { get; set; }

        [JsonProperty("presetHeight")]
        public object PresetHeight { get; set; }
    }

    public partial class SignatureOption
    {
        [JsonProperty("signatureOptionCode")]
        public string SignatureOptionCode { get; set; }

        [JsonProperty("signatureOptionLabel")]
        public string SignatureOptionLabel { get; set; }
    }

    public partial class XpsServices
    {
        public static XpsServices FromJson(string json)
        {
            return JsonConvert.DeserializeObject<XpsServices>(json, Converter.Settings);
        }
    }

    public static class Serialize
    {
        public static string ToJson(this XpsServices self)
        {
            return JsonConvert.SerializeObject(self, Converter.Settings);
        }
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}