namespace Epoxy.Api
{
    public class NamedElement : Element
    {
        public NamedElement(string name, Type type, string unresolvedTypeInfo, bool isConstant, bool isReference, bool isRawPointer, bool isSharedPointer, bool isUniquePtr)
            : base(type, unresolvedTypeInfo, isConstant, isReference, isRawPointer, isSharedPointer, isUniquePtr)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}