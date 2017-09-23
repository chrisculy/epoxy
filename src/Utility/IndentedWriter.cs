using System;
using System.IO;

namespace Epoxy.Utility
{
    public class IndentedWriter : IDisposable
    {
        public IndentedWriter(string filePath)
        {
            m_writer = new StreamWriter(filePath);
            m_indent = 0;
        }

        public Scope Indent(int indentDelta = 1)
        {
            if (indentDelta < 0 && (m_indent + indentDelta) < 0)
            {
                indentDelta = -m_indent;
            }

            m_indent += indentDelta;

            return new Scope(() => m_indent -= indentDelta);
        }

        public Scope IndentBlock(int indentDelta = 1)
        {
            if (indentDelta < 0 && (m_indent + indentDelta) < 0)
            {
                indentDelta = -m_indent;
            }

            WriteLine("{");
            m_indent += indentDelta;
            
            return new Scope(() => {
                m_indent -= indentDelta;
                WriteLine("}");
            });
        }

        public void WriteLine(string line = "")
        {
            for (int i = 0; i < m_indent; i++)
            {
                m_writer.Write("\t");
            }
            
            m_writer.WriteLine(line);
        }

        public void Dispose()
        {
            m_writer.Dispose();
        }

        private StreamWriter m_writer;
        private int m_indent;
    }

}
