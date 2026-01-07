using System.Text.Json;
using CurrencyTracker; 

class Program
{
    // Hocanın istediği Frankfurter API Adresi
    private const string ApiUrl = "https://api.frankfurter.app/latest?from=TRY";

    static async Task Main(string[] args)
    {
        Console.WriteLine("--- CurrencyTracker Başlatılıyor ---");
        
        // 1. Verileri API'den Çek
        Console.WriteLine("Veriler sunucudan çekiliyor, lütfen bekleyin...");
        List<Currency> currencies = await GetCurrenciesAsync();

        if (currencies == null || currencies.Count == 0)
        {
            Console.WriteLine("Veri çekilemedi! İnternet bağlantınızı kontrol edin.");
            return;
        }

        Console.WriteLine($"✅ BAŞARILI: Toplam {currencies.Count} adet döviz verisi alındı.\n");

        // 2. Menü Döngüsü
        while (true)
        {
            Console.WriteLine("\n===== CurrencyTracker =====");
            Console.WriteLine("1. Tüm dövizleri listele");
            Console.WriteLine("2. Koda göre döviz ara (USD, EUR vb.)");
            Console.WriteLine("3. Belirli bir değerden büyükleri listele");
            Console.WriteLine("4. Dövizleri değere göre sırala");
            Console.WriteLine("5. İstatistiksel özet göster");
            Console.WriteLine("0. Çıkış");
            Console.Write("Seçiminiz: ");

            string secim = Console.ReadLine();
            Console.WriteLine("------------------------------------------");

            switch (secim)
            {
                case "1":
                    ListAll(currencies);
                    break;
                case "2":
                    SearchByCode(currencies);
                    break;
                case "3":
                    FilterByValue(currencies);
                    break;
                case "4":
                    SortByValue(currencies);
                    break;
                case "5":
                    ShowStatistics(currencies);
                    break;
                case "0":
                    Console.WriteLine("Çıkış yapılıyor...");
                    return;
                default:
                    Console.WriteLine("Geçersiz seçim! Lütfen tekrar deneyin.");
                    break;
            }
        }
    }

    // --- API İŞLEMLERİ (HttpClient) ---
    static async Task<List<Currency>> GetCurrenciesAsync()
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                // API isteği yap
                string jsonResponse = await client.GetStringAsync(ApiUrl);

                // Gelen JSON verisini C# nesnesine çevir
                var responseObj = JsonSerializer.Deserialize<CurrencyResponse>(jsonResponse);

                // Sözlük (Dictionary) yapısını Listeye çevir (LINQ Select)
                List<Currency> list = responseObj.Rates
                    .Select(x => new Currency { Code = x.Key, Rate = x.Value })
                    .ToList();

                return list;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
            return new List<Currency>();
        }
    }

    // --- 1. LİSTELEME ---
    static void ListAll(List<Currency> list)
    {
        Console.WriteLine("KOD\tDEĞER (1 TRY Karşılığı)");
        foreach (var item in list)
        {
            Console.WriteLine($"{item.Code}\t{item.Rate}");
        }
    }

    // --- 2. ARAMA (LINQ Where) ---
    static void SearchByCode(List<Currency> list)
    {
        Console.Write("Aranacak Döviz Kodu: ");
        string input = Console.ReadLine()?.ToUpper().Trim();

        var result = list.FirstOrDefault(c => c.Code == input);

        if (result != null)
            Console.WriteLine($"✅ BULUNDU -> {result.Code}: {result.Rate}");
        else
            Console.WriteLine("❌ Böyle bir döviz kodu bulunamadı.");
    }

    // --- 3. FİLTRELEME (LINQ Where) ---
    static void FilterByValue(List<Currency> list)
    {
        Console.Write("Minimum Değer Girin (Örn: 0.5): ");
        string valStr = Console.ReadLine().Replace('.', ','); // Virgül hatası olmasın diye

        if (decimal.TryParse(valStr, out decimal minVal))
        {
            var filtered = list.Where(c => c.Rate > minVal).ToList();
            Console.WriteLine($"\n{minVal} değerinden büyük {filtered.Count} adet döviz bulundu:\n");
            foreach (var item in filtered)
                Console.WriteLine($"{item.Code}\t{item.Rate}");
        }
        else
        {
            Console.WriteLine("❌ Geçersiz sayı formatı!");
        }
    }

    // --- 4. SIRALAMA (LINQ OrderBy) ---
    static void SortByValue(List<Currency> list)
    {
        Console.WriteLine("1. Küçükten Büyüğe (Artan)");
        Console.WriteLine("2. Büyükten Küçüğe (Azalan)");
        Console.Write("Seçim: ");
        string sortChoice = Console.ReadLine();

        List<Currency> sortedList;

        if (sortChoice == "1")
            sortedList = list.OrderBy(c => c.Rate).ToList();
        else
            sortedList = list.OrderByDescending(c => c.Rate).ToList();

        Console.WriteLine("\nSIRALI LİSTE:");
        foreach (var item in sortedList)
            Console.WriteLine($"{item.Code}\t{item.Rate}");
    }

    // --- 5. İSTATİSTİK (LINQ Max, Min, Avg) ---
    static void ShowStatistics(List<Currency> list)
    {
        int count = list.Count();
        decimal maxRate = list.Max(c => c.Rate);
        decimal minRate = list.Min(c => c.Rate);
        decimal avgRate = list.Average(c => c.Rate);

        var maxCurrency = list.First(c => c.Rate == maxRate);
        var minCurrency = list.First(c => c.Rate == minRate);

        Console.WriteLine("📊 --- İSTATİSTİKLER ---");
        Console.WriteLine($"Toplam Döviz Sayısı : {count}");
        Console.WriteLine($"En Yüksek Kur       : {maxRate} ({maxCurrency.Code})");
        Console.WriteLine($"En Düşük Kur        : {minRate} ({minCurrency.Code})");
        Console.WriteLine($"Ortalama Kur        : {avgRate:F4}");
    }
}