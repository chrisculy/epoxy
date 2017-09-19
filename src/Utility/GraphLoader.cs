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
                        Class newClass = ProcessNamedApiContainer<Class>(mainNode);
                        graph.Classes.Add(newClass);
                        break;
                    case "namespace":
                        Namespace newNamespace = ProcessNamedApiContainer<Namespace>(mainNode);
                        graph.Namespaces.Add(newNamespace);
                        break;
                    case "file":
                        ProcessMembers(graph, mainNode);
                        break;
                    default:
                        break;
                   
                }
            }
        }

        private static T ProcessNamedApiContainer<T>(XmlNode node) where T : NamedApiContainer
        {
            string id = Xml.GetId(node);
            string name = Xml.GetCompoundNodeName(node);
            T newApiContainer = System.Activator.CreateInstance(typeof(T), new object []{ id, name }) as T;

            ProcessMembers(newApiContainer, node);
            return newApiContainer;
        }

        private static void ProcessMembers(ApiContainer apiContainer, XmlNode node)
        {
            foreach (XmlNode memberNode in GetMemberNodes(node))
            {
                switch (Xml.GetKind(memberNode))
                {
                    case Xml.Function:
                        ProcessFunction(apiContainer, memberNode);
                        break;
                    case Xml.Variable:
                        ProcessVariable(apiContainer, memberNode);
                        break;
                    default:
                        // This can only happen if GetMemberNodes is broken.
                        throw new System.InvalidOperationException("Member nodes must have type 'Epoxy.Api.Xml.Function' or 'Epoxy.Api.Xml.Variable'.");
                }
            }
        }

        private static void ProcessFunction(ApiContainer apiContainer, XmlNode memberNode)
        {
            List<NamedElement> parameters = new List<NamedElement>();
            foreach (XmlNode parameterNode in memberNode.SelectNodes(Xml.Param))
            {
                parameters.Add(GetNamedElement(parameterNode));
            }

            Element returnType = GetElement(memberNode);

            apiContainer.Functions.Add(new Function(Xml.GetId(memberNode), Xml.GetName(memberNode), returnType, Xml.GetIsConstant(memberNode), parameters.AsReadOnly()));
        }

        private static void ProcessVariable(ApiContainer apiContainer, XmlNode memberNode)
        {
            NamedElement variable = GetNamedElement(memberNode);
            if (variable != null)
            {
                apiContainer.Variables.Add(variable);
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