namespace Sprakcoach
{
    public class OrdPost
    {
        public string Svenska { get; set; } = "";
        public string Engelska { get; set; } = "";
        public override string ToString() => $"{Svenska} -> {Engelska}";
    }

}