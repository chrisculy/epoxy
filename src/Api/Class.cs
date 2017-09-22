using System.Collections.Generic;

namespace Epoxy.Api
{
    public class Class : NamedApiContainer
    {
        public Class(string id, string name, string namespaceName)
            : base(id, name)
        {
            Namespace = namespaceName;
        }

        public string Namespace { get; private set; }
    }
}