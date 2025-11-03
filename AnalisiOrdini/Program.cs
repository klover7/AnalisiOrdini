using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AnalisiOrdini
{
    public class Order
    {
        [Name("Id")]
        public int Id { get; set; }

        [Name("Article Name")]
        public string ArticleName { get; set; }

        [Name("Quantity")]
        public int Quantity { get; set; }

        [Name("Unit price")]
        public decimal UnitPrice { get; set; }

        [Name("Percentage discount")]
        public double PercentageDiscount { get; set; }

        [Name("Buyer")]
        public string Buyer { get; set; }

        // Proprietà calcolate
        public decimal TotalWithoutDiscount => Quantity * UnitPrice;
        public decimal TotalWithDiscount => TotalWithoutDiscount * (1 - (decimal)(PercentageDiscount / 100));
        public decimal DiscountAmount => TotalWithoutDiscount - TotalWithDiscount;
    }

    class Program
    {
        static int Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Verifica che sia stato passato il percorso del file
            if (args.Length == 0)
            {
                Console.Error.WriteLine("Errore: Nessun percorso del file CSV fornito.");
                PrintUsage();
                return 1;
            }

            string csvPath = args[0];

            try
            {
                if (!File.Exists(csvPath))
                {
                    Console.Error.WriteLine($"Errore: File non trovato '{csvPath}'");
                    return 1;
                }

                var orders = LoadOrdersFromCsv(csvPath);

                if (!orders.Any())
                {
                    Console.WriteLine("Attenzione: Nessun dato valido trovato nel file.");
                    return 0;
                }

                DisplayStatistics(orders);
                return 0;
            }
            catch (CsvHelperException ex)
            {
                Console.Error.WriteLine($"Errore di analisi del CSV: {ex.Message}");
                return 2;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Errore sconosciuto: {ex.Message}");
                return 3;
            }
        }

        /// Carica gli ordini dal file CSV
        static List<Order> LoadOrdersFromCsv(string path)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
                PrepareHeaderForMatch = args => args.Header.Trim().ToLower(),
                TrimOptions = TrimOptions.Trim
            };

            using var reader = new StreamReader(path, System.Text.Encoding.UTF8);
            using var csv = new CsvReader(reader, config);

            var records = csv.GetRecords<Order>()
                             .Where(o => IsValidOrder(o))
                             .ToList();

            return records;
        }

        /// <summary>
        /// Valida un ordine
        /// </summary>
        static bool IsValidOrder(Order order)
        {
            if (order == null) return false;

            return !string.IsNullOrWhiteSpace(order.ArticleName) &&
                   !string.IsNullOrWhiteSpace(order.Buyer) &&
                   order.Quantity > 0 &&
                   order.UnitPrice >= 0 &&
                   order.PercentageDiscount >= 0 &&
                   order.PercentageDiscount <= 100;
        }

        /// <summary>
        /// Mostra le statistiche richieste
        /// </summary>
        static void DisplayStatistics(List<Order> orders)
        {
            Console.WriteLine("\n--- ANALISI ORDINE ---\n");

            var highestTotal = FindOrderWithHighestTotal(orders);
            if (highestTotal != null)
            {
                Console.WriteLine("Record con importo totale più alto:");
                Console.WriteLine($"  ID: {highestTotal.Id}");
                Console.WriteLine($"  Article: {highestTotal.ArticleName}");
                Console.WriteLine($"  Buyer: {highestTotal.Buyer}");
                Console.WriteLine($"  Total (with discount): {highestTotal.TotalWithDiscount:C2}");
                Console.WriteLine();
            }

            var highestQuantity = FindOrderWithHighestQuantity(orders);
            if (highestQuantity != null)
            {
                Console.WriteLine("Record con quantità più alta:");
                Console.WriteLine($"  ID: {highestQuantity.Id}");
                Console.WriteLine($"  Article: {highestQuantity.ArticleName}");
                Console.WriteLine($"  Buyer: {highestQuantity.Buyer}");
                Console.WriteLine($"  Quantity: {highestQuantity.Quantity}");
                Console.WriteLine();
            }

            var highestDiscount = FindOrderWithHighestDiscountAmount(orders);
            if (highestDiscount != null)
            {
                Console.WriteLine("Record con maggior differenza tra totale senza sconto e totale con sconto:");
                Console.WriteLine($"  ID: {highestDiscount.Id}");
                Console.WriteLine($"  Article: {highestDiscount.ArticleName}");
                Console.WriteLine($"  Buyer: {highestDiscount.Buyer}");
                Console.WriteLine($"  Discount: {highestDiscount.DiscountAmount:C2} ({highestDiscount.PercentageDiscount}%)");
                Console.WriteLine($"  Total without discount: {highestDiscount.TotalWithoutDiscount:C2}");
                Console.WriteLine($"  Total with discount: {highestDiscount.TotalWithDiscount:C2}");
            }
        }

        static Order FindOrderWithHighestTotal(List<Order> orders)
        {
            return orders.OrderByDescending(o => o.TotalWithDiscount).FirstOrDefault();
        }

        static Order FindOrderWithHighestQuantity(List<Order> orders)
        {
            return orders.OrderByDescending(o => o.Quantity).FirstOrDefault();
        }

        static Order FindOrderWithHighestDiscountAmount(List<Order> orders)
        {
            return orders.OrderByDescending(o => o.DiscountAmount).FirstOrDefault();
        }

        static void PrintUsage()
        {
            Console.WriteLine("Utilizzo: AnalisiOrdini [csv_file_path]");
            Console.WriteLine("  percorso_file_csv: percorso del file CSV contenente i dati degli ordini (obbligatorio).");
        }
    }
}