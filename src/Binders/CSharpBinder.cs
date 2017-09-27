using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Epoxy.Api;
using Epoxy.Utility;

namespace Epoxy.Binders
{
    public class CSharpBinder : IBinder
    {
        public CSharpBinder(BinderConfiguration configuration)
            : base(configuration)
        {
        }

        public override void GenerateLanguageBindings(Graph graph)
        {
            if (!Directory.Exists(Configuration.LanguageBindingsDirectory))
            {
                Directory.CreateDirectory(Configuration.LanguageBindingsDirectory);
            }

            foreach (Class classDefinition in graph.Classes)
            {
                WriteCsClass(classDefinition, Path.Combine(Configuration.LanguageBindingsDirectory, $"{classDefinition.Name}.cs"));
            }

            using (IndentedWriter writer = new IndentedWriter($"{Path.Combine(Configuration.LanguageBindingsDirectory, Configuration.GlobalsClassName)}.cs"))
            {
                WriteFileHeader(writer);

                writer.WriteLine($"namespace {Configuration.GlobalsNamespace}");
                using (Scope namespaceScope = writer.IndentBlock())
                {
                    writer.WriteLine($"public static class {Configuration.GlobalsClassName}");
                    using (Scope classScope = writer.IndentBlock())
                    {
                        // write out the functions
                        foreach (Function function in graph.Functions)
                        {
                            WriteCsFunction(function, writer);
                        }

                        // write out the variables
                        foreach (NamedElement variable in graph.Variables)
                        {
                            WriteCsVariable(variable, writer);
                        }

                        // write out the native bindings for the functions
                        foreach (Function function in graph.Functions)
                        {
                            WriteFunctionNativeBindings(function, writer);
                        }

                        // write out the native bindings for the variables
                        foreach (NamedElement variable in graph.Variables)
                        {
                            WriteVariableNativeBindings(variable, writer);
                        }
                    }
                }
            }
        }

        public override void GenerateNativeBindings(Graph graph)
        {
            // TODO
        }

        private void WriteCsClass(Class classDefinition, string filePath)
        {
            using (IndentedWriter writer = new IndentedWriter(filePath))
            {
                WriteFileHeader(writer);

                writer.WriteLine($"namespace {ToCsNamespace(classDefinition.Namespace)}");
                using (Scope namespaceScope = writer.IndentBlock())
                {
                    string className = ToPascalCase(classDefinition.Name);
                    writer.WriteLine($"class {className} : SafeEpoxyHandle");

                    using (Scope classScope = writer.IndentBlock())
                    {
                        // write out the constructor, destructor, and the member functions
                        foreach (Function function in classDefinition.Functions)
                        {
                            WriteCsFunction(function, writer);
                        }

                        // write out the member variables
                        foreach (NamedElement variable in classDefinition.Variables)
                        {
                            WriteCsVariable(variable, writer);
                        }

                        // write out the native bindings for the functions
                        foreach (Function function in classDefinition.Functions)
                        {
                            WriteFunctionNativeBindings(function, writer);
                        }

                        // write out the native bindings for the variables
                        foreach (NamedElement variable in classDefinition.Variables)
                        {
                            WriteVariableNativeBindings(variable, writer);
                        }
                    }
                }
            }
        }

        private void WriteCsFunction(Function function, IndentedWriter writer)
        {
            string returnType = ToCsReturnType(function.Return);

            writer.WriteLine($"public {returnType} {ToPascalCase(function.Name)}{ToCsParameterString(function.Parameters)}");
            using (Scope functionScope = writer.IndentBlock())
            {
                writer.WriteLine($@"{(returnType == "void" ? "" : "return ")}{ToNativeFunction(function)}({(string.Join(", ", function.Parameters.Select(parameter => parameter.Name)))});");
            }
            writer.WriteLine();
        }

        private void WriteFunctionNativeBindings(Function function, IndentedWriter writer)
        {
            writer.WriteLine($"[DllImport(\"{Configuration.DllFileName}\")]");
            writer.WriteLine($"private static extern {ToCsReturnType(function.Return)} {ToNativeFunction(function)}{ToCsParameterString(function.Parameters)};");
            writer.WriteLine();
        }
        
        private void WriteCsVariable(NamedElement variable, IndentedWriter writer)
        {
            // TODO: is return type the right marshalling strategy here?
            writer.WriteLine($"public {ToCsReturnType(variable)} {ToPascalCase(variable.Name)}");
            using (Scope propertyScope = writer.IndentBlock())
            {
                writer.WriteLine($"get {{ return {ToNativeVariableGet(variable)}(); }}");
                writer.WriteLine($"set {{ {ToNativeVariableSet(variable)}(value); }}");
            }
            writer.WriteLine();
        }

        private void WriteVariableNativeBindings(NamedElement variable, IndentedWriter writer)
        {
            writer.WriteLine($"[DllImport(\"{Configuration.DllFileName}\")]");
            writer.WriteLine($"private static extern {ToCsReturnType(variable)} {ToNativeVariableGet(variable)}();");
            writer.WriteLine($"[DllImport(\"{Configuration.DllFileName}\")]");
            writer.WriteLine($"private static extern void {ToNativeVariableSet(variable)}({ToCsParameter(variable)});");
            writer.WriteLine();
        }

        private static void WriteFileHeader(IndentedWriter writer)
        {
            writer.WriteLine("// WARNING: This file is generated by Epoxy. Do not edit this file manually.");
            writer.WriteLine("// -------------------------------------------------------------------------");
            writer.WriteLine();
            writer.WriteLine("using System.Runtime.InteropServices;");
            writer.WriteLine();
        }

        private static string ToCsNamespace(string nativeNamespace)
        {
            return ToPascalCase(nativeNamespace.Replace("::", "_"));
        }

        private static string ToCsReturnType(Element nativeType)
        {
            // TODO
            return nativeType.Type.ToString().ToLower();
        }

        private static string ToCsParameter(NamedElement nativeParameter)
        {
            // TODO
            return $"{nativeParameter.Type.ToString()} {nativeParameter.Name}";
        }

        private static string ToCsParameterString(ReadOnlyCollection<NamedElement> parameters)
        {
            return $"({string.Join(", ", parameters.Select(parameter => ToCsParameter(parameter)))})";
        }

        private static string ToNativeFunction(Function function)
        {
            return $"{ToPascalCase(function.Name)}_Native";
        }

        private static string ToNativeVariableGet(NamedElement variable)
        {
            return $"Get{variable.Name}_Native";
        }

        private static string ToNativeVariableSet(NamedElement variable)
        {
            return $"Set{variable.Name}_Native";
        }

        private static string ToPascalCase(string name)
        {
            if (name.IsNullOrEmpty())
                return name;
            
            // capitalize first letter
            List<char> characters = new List<char>();
            characters.Add(char.ToUpper(name[0]));

            // handle snake case
            if (name.Contains("_"))
            {
                for (int index = 1; index < name.Length; index++)
                {
                    if (name[index] == '_')
                    {
                        if (index + 1 < name.Length)
                        {
                            index++;
                            characters.Add(char.ToUpper(name[index]));
                        }
                    }
                    else
                    {
                        characters.Add(name[index]);
                    }
                }
            }
            else
            {
                characters.AddRange(name.Substring(1).ToArray());
            }

            return new string(characters.ToArray());
        }
    }
}