using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Epoxy.Api;

namespace Epoxy.Utility
{
    public static class GraphLoader
    {
        public static Graph LoadGraph(IEnumerable<string> xmlFilePaths)
        {
            Graph graph = new Graph();
            foreach (string xmlFilePath in xmlFilePaths)
            {
                ProcessXmlFile(graph, xmlFilePath);
            }

            return graph;
        }

        private static void ProcessXmlFile(Graph graph, string xmlFilePath)
        {
            XmlDocument document = new XmlDocument();
            document.Load(xmlFilePath);

            XmlNode mainNode = document.DocumentElement.SelectSingleNode(Xml.CompoundDef);
            if (mainNode != null)
            {
                string kind = Xml.GetKind(mainNode);
                switch (kind)
                {
                    case "class":
                    case "struct":
                        Class newClass = ProcessClass(mainNode);
                        graph.Classes.Add(newClass);
                        break;
                    case "namespace":
                        string namespaceName = Xml.GetCompoundNodeName(mainNode);
                        ProcessMembers(graph, mainNode, namespaceName);
                        break;
                    default:
                        break;
                }
            }
        }

        private static Class ProcessClass(XmlNode node)
        {
            string id = Xml.GetId(node);
            string fullName = Xml.GetCompoundNodeName(node);
            string namespaceName = GetNamespaceFromFullyQualifiedName(fullName);
            Class classDefinition = new Class(id, GetNameFromFullyQualifiedName(fullName), namespaceName);

            ProcessMembers(classDefinition, node, namespaceName);
            return classDefinition;
        }

        private static void ProcessMembers(ApiContainer apiContainer, XmlNode node, string namespaceName)
        {
            foreach (XmlNode memberNode in GetMemberNodes(node))
            {
                switch (Xml.GetKind(memberNode))
                {
                    case Xml.Function:
                        ProcessFunction(apiContainer, memberNode, namespaceName);
                        break;
                    case Xml.Variable:
                        ProcessVariable(apiContainer, memberNode, namespaceName);
                        break;
                    default:
                        // This can only happen if GetMemberNodes is broken.
                        throw new System.InvalidOperationException("Member nodes must have type 'Epoxy.Api.Xml.Function' or 'Epoxy.Api.Xml.Variable'.");
                }
            }
        }

        private static void ProcessFunction(ApiContainer apiContainer, XmlNode memberNode, string namespaceName)
        {
            List<NamedElement> parameters = new List<NamedElement>();
            foreach (XmlNode parameterNode in memberNode.SelectNodes(Xml.Param))
            {
                parameters.Add(GetNamedElement(parameterNode));
            }

            Element returnType = GetElement(memberNode);
            string name = Xml.GetName(memberNode);
            bool isConstructor = apiContainer is Class classDefinition && classDefinition.Name == name;
            apiContainer.Functions.Add(new Function(Xml.GetId(memberNode), namespaceName, name, returnType, Xml.GetIsConstant(memberNode), isConstructor, parameters.AsReadOnly()));
        }

        private static void ProcessVariable(ApiContainer apiContainer, XmlNode memberNode, string namespaceName)
        {
            NamedElement namedElement = GetNamedElement(memberNode);
            if (namedElement != null)
            {
                apiContainer.Variables.Add(new Variable(namespaceName, namedElement.Name, namedElement.Type,
                    namedElement.UnresolvedTypeInfo, namedElement.IsConstant, namedElement.IsReference,
                    namedElement.IsRawPointer, namedElement.IsSharedPointer, namedElement.IsUniquePointer));
            }
        }

        private static IEnumerable<XmlNode> GetMemberNodes(XmlNode node)
        {
            return node.SelectNodes(Xml.MemberDefinitionSelector).OfType<XmlNode>().Where(childNode =>
                childNode.Attributes[Xml.Protection].Value == Xml.Public && (Xml.GetKind(childNode) == Xml.Function || Xml.GetKind(childNode) == Xml.Variable));
        }       

        private static Type GetType(string typeString)
        {
            return s_typeStringToType.ContainsKey(typeString) ? s_typeStringToType[typeString] : Type.Unresolved;
        }

        private static Element GetElement(XmlNode node)
        {
            string fullType = Xml.GetType(node);

            List<string> tokens = new List<string>(fullType.Split(' '));

            bool isConstant = false;
            int index = tokens.IndexOf("const");
            if (index != -1)
            {
                isConstant = true;
                tokens.RemoveAt(index);
            }

            bool isReference = false;
            index = tokens.IndexOf("&");
            if (index != -1)
            {
                isReference = true;
                tokens.RemoveAt(index);
            }

            bool isRawPointer = false;
            index = tokens.IndexOf("*");
            if (index != -1)
            {
                isRawPointer = true;
                tokens.RemoveAt(index);
            }

            bool isSharedPointer = false;
            bool isUniquePointer = false;
            string typeString = "";
            foreach (string token in tokens)
            {
                if (c_ignoredQualifiers.Contains(token))
                {
                    continue;
                }
                
                MatchCollection matches = c_smartPointerPattern.Matches(token);
                if (matches.Count != 0)
                {
                    isSharedPointer = matches[0].Value == "std::shared_pointer<";
                    isUniquePointer = matches[0].Value == "std::unique_ptr<";
                    typeString = matches[1].Value;
                    continue;
                }

                typeString = token;
                break;
            }

            Type type = GetType(typeString);
            string unresolvedTypeInfo = type == Type.Unresolved ? typeString : string.Empty;

            return new Element(type, unresolvedTypeInfo, isConstant, isRawPointer, isReference, isSharedPointer, isUniquePointer);
        }

        private static NamedElement GetNamedElement(XmlNode node)
        {
            Element element = GetElement(node);
            string name = Xml.GetName(node) ?? Xml.GetDeclname(node);

            return new NamedElement(name, element.Type, element.UnresolvedTypeInfo, element.IsConstant, element.IsRawPointer, element.IsReference, element.IsSharedPointer, element.IsUniquePointer);
        }

        private static string GetNameFromFullyQualifiedName(string fullName)
        {
            int index = fullName.LastIndexOf("::");
            return index != -1 ? fullName.Substring(index + 2, fullName.Length - (index + 2)) : fullName;
        }

        private static string GetNamespaceFromFullyQualifiedName(string fullName)
        {
            int index = fullName.LastIndexOf("::");
            return index != -1 ? fullName.Substring(0, index) : string.Empty;
        }

        private static readonly string[] c_ignoredQualifiers = new string[]{ "volatile", "mutable" };

        private static readonly Regex c_smartPointerPattern = new Regex(@"^(std::shared_ptr|std::unique_ptr)\<(.+)\>$");

        private static readonly Dictionary<string, Type> s_typeStringToType = new Dictionary<string, Type>() {
            { "bool", Type.Boolean },
            { "char", Type.Character8 },
            { "char8_t", Type.Character8 },
            { "char16_t", Type.Character16 },
            { "char32_t", Type.Character32 },
            { "int8_t", Type.Integer8 },
            { "int16_t", Type.Integer16 },
            { "int32_t", Type.Integer32 },
            { "int", Type.Integer32 },
            { "uint8_t", Type.UnsignedInteger8 },
            { "uint16_t", Type.UnsignedInteger16 },
            { "uint32_t", Type.UnsignedInteger32 },
            { "unsigned", Type.UnsignedInteger32 },
            { "float", Type.Float },
            { "double", Type.Double },
            { "std::string", Type.String },
            { "void", Type.Void }
        };
    }
}