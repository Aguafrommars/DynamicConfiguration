namespace Aguacongas.TheReverseProxy.Model
{
    public class Request
    {
        public string? Filter { get; set; }

        public string? Sort { get; set; }

        public string? Skip { get; set; }

        public string? Top { get; set; }

        public string? Expand { get; set; }

        public string? Select { get; set; }
    }
}
