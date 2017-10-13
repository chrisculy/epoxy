namespace Epoxy.Api
{
	using System;
	using System.Collections.Generic;

    public class Enumeration
    {
		public Enumeration(string name, string namespaceName, List<(string Name, int Value)> values)
		{
			Name = name;
			Namespace = namespaceName;
			Values = values;
		}

        public string Name { get; private set; }
		public string Namespace { get; private set; }
		public List<(string Name, int Value)> Values { get; private set; }
    }
}
