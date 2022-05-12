using System.Xml;

namespace LawLibrary
{
    /// <summary>
    /// Representa a versão em XML de um objeto ObjectParser.
    /// </summary>
    internal static class ObjectParserXml
    {
        /// <summary>
        /// Salva o objeto Law da classe ObjectParser em um arquivo Xml.
        /// </summary>
        /// <param name="parser">O objeto da classe ObjectParser.</param>
        /// <param name="outputFile">O caminho do arquivo a ser salvo com a extensão xml.</param>
        /// <param name="indent">True caso deseje que o Xml esteja indentado.</param>
        public static void Save(ObjectParser parser, string outputFile, bool indent)
        {
            using XmlWriter writer = XmlWriter.Create(outputFile, new XmlWriterSettings() { Indent = indent });
            Law law = parser.Law;

            writer.WriteStartDocument();
            writer.WriteStartElement("lei");
            writer.WriteStartElement("preliminar");            

            for (int i = 0; i < law.PreliminaryPart.Count; i++)
            {
                switch (law.PreliminaryPart[i].ContentType)
                {
                    case LawContentType.Epigrafe:
                        writer.WriteElementString("epigrafe", law.PreliminaryPart[i].Text);
                        break;
                    case LawContentType.Ementa:
                        writer.WriteElementString("ementa", law.PreliminaryPart[i].Text);
                        break;
                    case LawContentType.Preambulo:
                        writer.WriteElementString("preambulo", law.PreliminaryPart[i].Text);
                        break;
                    default:
                        writer.WriteElementString("texto", law.PreliminaryPart[i].Text);
                        break;
                }
            }

            writer.WriteEndElement();

            writer.WriteStartElement("normativa");
            SetXmlElementList(writer, law.NormativePart);
            writer.WriteEndElement();

            writer.WriteStartElement("final");
            for (int i = 0; i < law.FinalPart.Count; i++)
            {
                writer.WriteElementString("texto", law.FinalPart[i].Text);
            }
            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        private static void SetXmlElementList(XmlWriter writer, List<LawText>? list)
        {
            if (list == null)
                return;

            foreach (LawText t in list)
            {
                writer.WriteStartElement(GetTag(t.Text));
                writer.WriteElementString("valor", t.Text);

                if (t.SubContents != null && t.SubContents.Count > 0)
                {
                    SetXmlElementList(writer, t.SubContents);
                }

                writer.WriteEndElement();
            }
        }

        private static string GetTag(string text)
        {
            if (text.StartsWith("Art."))
            {
                return "artigo";
            }
            else if (text.StartsWith("Parágrafo") || text.StartsWith("§"))
            {
                return "paragrafo";
            }
            else if (text.StartsWith("I") || text.StartsWith("V") || text.StartsWith("X") || text.StartsWith("L"))
            {
                var split = text.Split(" ");

                if (split[1] == "-")
                {
                    return "inciso";
                }
            }
            else if (text.StartsWith("Título"))
            {
                return "titulo";
            }
            else if (text.StartsWith("Capítulo"))
            {
                return "capitulo";
            }
            else if (text.StartsWith("Seção"))
            {
                return "secao";
            }
            else if (text.StartsWith("Subseção"))
            {
                return "subsecao";
            }
            else if (text.StartsWith("Livro"))
            {
                return "livro";
            }
            else if (text.StartsWith("Parte"))
            {
                return "parte";
            }
            else if (text.StartsWith("a)") || text.StartsWith("b)") || text.StartsWith("c)") || text.StartsWith("d)")
                || text.StartsWith("e)") || text.StartsWith("f)") || text.StartsWith("g)") || text.StartsWith("h)")
                || text.StartsWith("i)") || text.StartsWith("j)") || text.StartsWith("k)") || text.StartsWith("l)")
                || text.StartsWith("m)") || text.StartsWith("n)") || text.StartsWith("o)") || text.StartsWith("p)")
                || text.StartsWith("q)") || text.StartsWith("r)") || text.StartsWith("s)") || text.StartsWith("t)")
                || text.StartsWith("u)") || text.StartsWith("v)") || text.StartsWith("x)") || text.StartsWith("z)")
                || text.StartsWith("y)") || text.StartsWith("w)"))
            {
                return "alinea";
            }

            return "texto";
        }
    }
}