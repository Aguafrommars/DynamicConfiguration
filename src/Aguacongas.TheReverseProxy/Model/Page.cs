using System.Collections.Generic;

namespace Aguacongas.TheReverseProxy.Model
{
    public class Page<T>
    {
        public int Count { get; set; }

        public IEnumerable<T>? Items { get; set; }
    }
}
