namespace Sprakcoach
{
    class Program
    {
        static readonly string filnamn = "ordbok.json";

        static void Main()
        {
            OrdbokService service = new OrdbokService(JsonLagring.Ladda(filnamn));
            Random randomNum = new Random();

            while (true)
            {
                Meny();
                Console.Write("Val: ");
                string val = Console.ReadLine() ?? string.Empty;

                switch (val)
                {
                    case "1": LaggaTill(service); break;
                    case "2": Andra(service); break;
                    case "3": TaBort(service); break;
                    case "4": Soka(service); break;
                    case "5": Quiz(service, randomNum); break;
                    case "6": Spara(service); break;
                    case "0":
                        // autospara vid avslutning
                        Spara(service);
                        Console.WriteLine("Hejdå!");
                        return;
                    default:
                        Console.WriteLine("Ogiltigt val.");
                        break;
                }
            }
        }

        static void Meny()
        {
            Console.WriteLine("\n=== Språkcoach ===");
            Console.WriteLine("1. Lägg till glosa");
            Console.WriteLine("2. Ändra glosa");
            Console.WriteLine("3. Ta bort glosa");
            Console.WriteLine("4. Sök (svenska ord/engelska ord)");
            Console.WriteLine("5. Quiz (10 frågor)");
            Console.WriteLine("6. Spara till JSON");
            Console.WriteLine("0. Avsluta");
        }

        static void LaggaTill(OrdbokService service)
        {
            Console.Write("Svenska: ");
            string svenska = Console.ReadLine() ?? "";
            Console.Write("Engelska: ");
            string engelska = Console.ReadLine() ?? "";

            service.LaggaTill(new OrdPost
            {
                Svenska = svenska.Trim(),
                Engelska = engelska.Trim(),
            });

            Console.WriteLine("Tillagd.");
        }

        static void Andra(OrdbokService service)
        {
            Console.Write("Vilken svensk glosa vill du ändra? ");
            string nyckel = Console.ReadLine() ?? "";

            bool ok = service.Andra(nyckel, p =>
            {
                Console.WriteLine($"Hittad: {p}");
                Console.Write("Ny engelska (lämna tom för att behålla): ");
                string engelska = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(engelska)) p.Engelska = engelska.Trim();
            });

            Console.WriteLine(ok ? "Ändrad." : "Hittade ingen glosa med det svenska ordet.");
        }

        static void TaBort(OrdbokService service)
        {
            Console.Write("Vilken svensk glosa vill du ta bort? ");
            string nyckel = Console.ReadLine() ?? "";
            Console.WriteLine(service.TaBort(nyckel) ? "Borttagen." : "Hittades inte.");
        }

        static void Soka(OrdbokService service)
        {
            Console.Write("Sökterm: ");
            string term = Console.ReadLine() ?? "";
            List<OrdPost> resultat = service.Soka(term).ToList();
            if (resultat.Count == 0) Console.WriteLine("Inget resultat.");
            else resultat.ForEach(p => Console.WriteLine(p));
        }


        static void Quiz(OrdbokService service, Random randomNum) => service.Quiz(10, randomNum);


        static void Spara(OrdbokService service)
        {
            JsonLagring.Spara(filnamn, service.HamtaAlla().ToList());
            Console.WriteLine($"Sparat till {filnamn}");
        }
    }
}