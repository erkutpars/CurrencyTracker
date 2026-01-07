# CurrencyTracker - Döviz Takip Konsol Uygulaması

* **Ders:** Görsel Programlama

---

## 🎯 Proje Hakkında
Bu proje, C# programlama dili kullanılarak geliştirilmiş, anlık döviz verilerini takip eden bir konsol uygulamasıdır. Frankfurter API kullanılarak veriler çekilmekte ve LINQ sorguları ile işlenmektedir.

## 🛠 Kullanılan Teknolojiler
* **Dil:** C# (.NET 8.0)
* **API:** Frankfurter FREE API
* **Veri Formatı:** JSON
* **Kütüphaneler:** System.Text.Json, HttpClient

## 📋 Özellikler ve LINQ Kullanımı
1. **Listeleme:** Tüm kurlar `Select` ile işlenip listelenir.
2. **Arama:** `Where` ve `FirstOrDefault` kullanılarak döviz koduna göre arama yapılır.
3. **Filtreleme:** `Where` kullanılarak belirli bir değerin üzerindeki kurlar getirilir.
4. **Sıralama:** `OrderBy` ve `OrderByDescending` ile kurlar sıralanır.
5. **İstatistik:** `Max`, `Min`, `Average`, `Count` ile analiz yapılır.