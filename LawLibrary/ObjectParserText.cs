namespace LawLibrary
{
    /// <summary>
    /// Representa o objeto Law em formato texto.
    /// </summary>
    internal static class ObjectParserText
    {
        private static int indentationCount = 0;
        private const char tabChar = '\t';

        /// <summary>
        /// Salva o objeto Law da classe ObjectParser em um formato de texto.
        /// </summary>
        /// <param name="parser">O objeto da classe ObjectParser.</param>
        /// <param name="outputFile">O caminho completo do arquivo de texto a ser salvo.</param>
        /// <param name="indent">Define se o texto será indentado.</param>
        public static void Save(ObjectParser parser, string outputFile, bool indent)
        {
            List<string> lines = new();
            Law law = parser.Law;

            for (int i = 0; i < law.PreliminaryPart.Count; i++)
            {
                lines.Add(law.PreliminaryPart[i].Text);
            }

            for (int i = 0; i < law.NormativePart.Count; i++)
            {
                lines.Add(law.NormativePart[i].Text);
                SetLines(lines, law.NormativePart[i].SubContents, indent);
            }

            for (int i = 0; i < law.FinalPart.Count; i++)
            {
                lines.Add(law.FinalPart[i].Text);
            }

            using FileStream fs = File.Create(outputFile);
            fs.Close();
            File.AppendAllLines(outputFile, lines);
        }       

        private static void SetLines(List<string> lines, List<LawText>? list, bool indentText)
        {
            if (list == null)
                return;

            indentationCount++;

            foreach (LawText t in list)
            {
                if (indentText)
                {
                    lines.Add($"{new string(tabChar, indentationCount)}{t.Text}");
                }
                else
                {
                    lines.Add(t.Text);
                }

                if (t.SubContents != null && t.SubContents.Count > 0)
                {
                    SetLines(lines, t.SubContents, indentText);
                }
            }

            indentationCount--;
        }
    }
}
