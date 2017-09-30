using System.Xml;

namespace Epoxy.Utility
{
    public static class Xml
    {
        public const string Bind = "bind";
        public const string CompoundDef = "compounddef";
        public const string CompoundName = "compoundname";
        public const string Const = "const";
        public const string Declname = "declname";
        public const string Function = "function";
        public const string Id = "id";
        public const string Kind = "kind";
        public const string MemberDefinitionSelector = ".//memberdef";
        public const string Name = "name";
        public const string Param = "param";
        public const string Protection = "prot";
        public const string Public = "public";
        public const string Static = "static";
        public const string Type = "type";
        public const string Variable = "variable";
        public const string XmlOnlySelector = ".//xmlonly";

        public static string GetId(XmlNode node)
        {
            return node.Attributes[Xml.Id].Value;
        }

        public static bool GetIsConstant(XmlNode node)
        {
            return node.Attributes[Xml.Const].Value == Yes;
        }

        public static bool GetIsStatic(XmlNode node)
        {
            return node.Attributes[Xml.Static].Value == Yes;
        }

        public static string GetKind(XmlNode node)
        {
            return node.Attributes[Xml.Kind].Value;
        }
        
        public static string GetName(XmlNode node)
        {
            return node.SelectSingleNode(Xml.Name)?.InnerText;
        }

        public static string GetDeclname(XmlNode node)
        {
            return node.SelectSingleNode(Xml.Declname)?.InnerText;
        }

        public static string GetType(XmlNode node)
        {
            return node.SelectSingleNode(Xml.Type)?.InnerText;
        }

        public static string GetCompoundNodeName(XmlNode node)
        {
            return node.SelectSingleNode(Xml.CompoundName)?.InnerText;
        }

        private const string Yes = "yes";
    }
}