using System.Text.Json.Serialization;

namespace CurrencyTracker
{
    // API'den gelen karmaşık veriyi karşılayan sınıf
    public class CurrencyResponse
    {
        [JsonPropertyName("base")] 
        public string Base { get; set; }

        [JsonPropertyName("rates")]
        public Dictionary<string, decimal> Rates { get; set; }
    }

    // Bizim listelerde kullanacağımız sade sınıf
    public class Currency
    {
        public string Code { get; set; }
        public decimal Rate { get; set; }
    }
}