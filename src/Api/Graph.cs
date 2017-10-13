using System.Collections.Generic;

namespace Epoxy.Api
{
	public class Graph : ApiContainer
	{
		public Graph()
		{
			Classes = new List<Class>();
			Functions = new List<Function>();
			Variables = new List<Variable>();
		}

		public List<Class> Classes { get; }
		public List<Function> Functions { get; }
		public List<Variable> Variables { get; }
	}
}
