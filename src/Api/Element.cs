namespace Epoxy.Api
{
    public class Element
    {
        public Element(Type type, string typeInfo, bool isConstant, bool isReference, bool isRawPointer, bool isSharedPointer, bool isUniquePtr)
        {
            Type = type;
            TypeInfo = typeInfo;
            IsConstant = isConstant;
            IsReference = isReference;
            IsRawPointer = isRawPointer;
            IsSharedPointer = isSharedPointer;
            IsUniquePointer = isUniquePtr;
        }

        public Type Type { get; set; }
        public string TypeInfo { get; private set; }
        public bool IsConstant { get; private set; }
        public bool IsReference { get; private set; }
        public bool IsRawPointer { get; private set; }
        public bool IsSharedPointer { get; private set; }
        public bool IsUniquePointer { get; private set; }
    }
}