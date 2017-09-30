using System.Collections.Generic;

namespace Epoxy.Api
{
    public class Class : ApiContainer
    {
        public Class(string id, string name, string namespaceName)
        {
            Id = id;
            Name = name;
            Namespace = namespaceName;
            Functions = new List<Function>();
            Variables = new List<Variable>();
        }

        public string Id { get; }
        public string Name { get; }
        public string Namespace { get; private set; }
        public List<Function> Functions { get; private set; }
        public List<Variable> Variables { get; private set; }
    }
}