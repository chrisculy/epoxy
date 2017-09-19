using System.Collections.Generic;

namespace Epoxy.Api
{
    public abstract class NamedApiContainer : ApiContainer
    {
        public NamedApiContainer(string id, string name)
        {
            Id = id;
            Name = name;
            Functions = new List<Function>();
            Variables = new List<NamedElement>();
        }

        public string Name { get; }
        public string Id { get; }
        public List<Function> Functions { get; private set; }
        public List<NamedElement> Variables { get; private set; }
    }
}