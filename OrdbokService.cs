// Logik för att hantera ordlistan

namespace Sprakcoach
{
    public class OrdbokService
    {
        // Intern lagring av ordposter
        private readonly List<OrdPost> _poster;
        // Konstruktor som initierar med en lista av ordposter eller en tom lista
        public OrdbokService(List<OrdPost>? start = null)
        {
            _poster = start ?? new List<OrdPost>();
        }

        public IReadOnlyList<OrdPost> HamtaAlla() => _poster;

        public void LaggaTill(OrdPost p) => _poster.Add(p);

        public bool Andra(string svenska, Action<OrdPost> updater)
        {
            OrdPost? post = _poster.FirstOrDefault(p => p.Svenska.Equals(svenska, StringComparison.OrdinalIgnoreCase));
            if (post is null) return false;
            updater(post);
            return true;
        }

        public bool TaBort(string svenska)
        {
            OrdPost? post = _poster.FirstOrDefault(p => p.Svenska.Equals(svenska, StringComparison.OrdinalIgnoreCase));
            if (post is null) return false;
            _poster.Remove(post);
            return true;
        }

        public IEnumerable<OrdPost> Soka(string term)
        {
            term = term.Trim();
            return _poster.Where(p =>
                p.Svenska.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                p.Engelska.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        public void Quiz(int antalFrogor, Random randomNum)
        {
            if (_poster.Count == 0)
            {
                Console.WriteLine("Ordboken är tom. Lägg till ord först.");
                return;
            }
            // IEnumerable betyder att vi kan loopa över resultat
            IEnumerable<OrdPost> frogor = _poster.OrderBy(_ => randomNum.Next()).Take(Math.Min(antalFrogor, _poster.Count)).ToList();
            int ratt = 0;

            foreach (OrdPost froga in frogor)
            {
                Console.Write($"Översätt till engelska: \"{froga.Svenska}\": ");
                string svar = Console.ReadLine()?.Trim() ?? "";
                if (svar.Equals(froga.Engelska, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Rätt!");
                    ratt++;
                }
                else
                {
                    Console.WriteLine($"Fel. Rätt svar: {froga.Engelska}");
                }
            }

            Console.WriteLine($"\nResultat: {ratt}/{frogor.Count()} rätt.");
        }
    }
}