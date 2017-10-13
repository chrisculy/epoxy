using System.Collections.ObjectModel;

namespace Epoxy.Api
{
	public class Function
	{
		public Function(string id, string namespaceName, string name, Element returnType, bool isConstant, bool isConstructor, bool isStatic, bool isClassMember, ReadOnlyCollection<NamedElement> parameters)
		{
			Id = id;
			Namespace = namespaceName;
			Name = name;
			Return = returnType;
			IsClassMember = isClassMember;
			IsConstant = isConstant;
			IsConstructor = isConstructor;
			IsStatic = isStatic;
			Parameters = parameters;
		}

		public string Id { get; private set; }
		public string Namespace { get; private set; }
		public string Name { get; private set; }
		public Element Return { get; private set; }
		public bool IsClassMember { get; private set; }
		public bool IsConstant { get; private set; }
		public bool IsConstructor { get; private set; }
		public bool IsStatic { get; private set; }
		public ReadOnlyCollection<NamedElement> Parameters { get; private set; }
	}
}
