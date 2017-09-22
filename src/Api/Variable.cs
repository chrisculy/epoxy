namespace Epoxy.Api
{
    public class Variable : NamedElement
    {
        public Variable(string namespaceName, string name, Type type, string unresolvedTypeInfo, bool isConstant, bool isReference, bool isRawPointer, bool isSharedPointer, bool isUniquePtr)
            : base(name, type, unresolvedTypeInfo, isConstant, isReference, isRawPointer, isSharedPointer, isUniquePtr)
        {
            Namespace = namespaceName;
        }

        public string Namespace { get; private set; }
    }
}