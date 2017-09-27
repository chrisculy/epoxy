namespace Epoxy
{
    public class BinderConfiguration
    {
        public string Language { get; set; }
        public string NativeBindingsDirectory { get; set; }
        public string LanguageBindingsDirectory { get; set; }
        public string GlobalsClassName { get; set; }
        public string GlobalsNamespace { get; set; }
        public string DllFileName { get; set; }
    }
}