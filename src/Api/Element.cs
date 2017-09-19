namespace Epoxy.Api
{
    public class Element
    {
        public Element(Type type, string unresolvedTypeInfo, bool isConstant, bool isReference, bool isRawPointer, bool isSharedPointer, bool isUniquePtr)
        {
            Type = type;
            UnresolvedTypeInfo = unresolvedTypeInfo;
            IsConstant = isConstant;
            IsReference = isReference;
            IsRawPointer = isRawPointer;
            IsSharedPointer = isSharedPointer;
            IsUniquePointer = isUniquePtr;
        }

        public Type Type { get; private set; }
        public string UnresolvedTypeInfo { get; private set; }
        public bool IsConstant { get; private set; }
        public bool IsReference { get; private set; }
        public bool IsRawPointer { get; private set; }
        public bool IsSharedPointer { get; private set; }
        public bool IsUniquePointer { get; private set; }
    }
}