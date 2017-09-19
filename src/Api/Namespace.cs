using System.Collections.Generic;

namespace Epoxy.Api
{
    public class Namespace : NamedApiContainer
    {
        public Namespace(string id, string name)
            : base(id, name)
        {
            Namespaces = new List<Namespace>();
        }
        
        public List<Namespace> Namespaces { get; }        
    }
}