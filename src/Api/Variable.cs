namespace Epoxy.Api
{
	public class Variable : NamedElement
	{
		public Variable(string namespaceName, string name, Type type, string unresolvedTypeInfo, bool isStatic, bool isClassMember, bool isConstant, bool isReference, bool isRawPointer, bool isSharedPointer, bool isUniquePtr)
			: base(name, type, unresolvedTypeInfo, isConstant, isReference, isRawPointer, isSharedPointer, isUniquePtr)
		{
			IsClassMember = isClassMember;
			IsStatic = isStatic;
			Namespace = namespaceName;
		}

		public bool IsClassMember { get; private set; }
		public bool IsStatic { get; private set; }
		public string Namespace { get; private set; }
	}
}
