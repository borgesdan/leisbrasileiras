namespace LawLibrary
{
    /// <summary>
    /// Representa a análise da parte normativa da lei.
    /// </summary>
    internal static class LawParserNormativePart
    {
        /// <summary>
        /// Executa a análise das palavras chaves e chama os métodos Add equivalentes do objeto parser.
        /// </summary>
        /// <param name="line">A linha a ser análisada.</param>
        /// <param name="parser">O objeto da classe ObjectParser.</param>
        public static void Parse(ref string? line, LawParser parser)
        {
            if (line == null)
                return;

            if (line.StartsWith("Art."))
            {
                parser.AddArtigo(ref line);
            }
            else if (line.StartsWith("Parágrafo") || line.StartsWith("§"))
            {
                parser.AddParagrafo(ref line);
            }
            else if (line.StartsWith("I") || line.StartsWith("V") || line.StartsWith("X") || line.StartsWith("L"))
            {
                var split = line.Split(" ");

                if (split[1] == "-")
                {
                    parser.AddInciso(ref line);
                }
            }
            else if (line.StartsWith("Título") || line.StartsWith("TÍTULO"))
            {
                parser.AddTitulo(ref line);
            }
            else if (line.StartsWith("Capítulo") || line.StartsWith("CAPÍTULO"))
            {
                parser.AddCapitulo(ref line);
            }
            else if (line.StartsWith("Seção") || line.StartsWith("SEÇÃO"))
            {
                parser.AddSecao(ref line);
            }
            else if (line.StartsWith("Subseção") || line.StartsWith("SUBSEÇÃO"))
            {
                parser.AddSubSecao(ref line);
            }
            else if (line.StartsWith("Livro") || line.StartsWith("LIVRO"))
            {
                parser.AddLivro(ref line);
            }
            else if (line.StartsWith("Parte") || line.StartsWith("PARTE"))
            {
                parser.AddParte(ref line);
            }
            else if (line.StartsWith("a)") || line.StartsWith("b)") || line.StartsWith("c)") || line.StartsWith("d)")
                || line.StartsWith("e)") || line.StartsWith("f)") || line.StartsWith("g)") || line.StartsWith("h)")
                || line.StartsWith("i)") || line.StartsWith("j)") || line.StartsWith("k)") || line.StartsWith("l)")
                || line.StartsWith("m)") || line.StartsWith("n)") || line.StartsWith("o)") || line.StartsWith("p)")
                || line.StartsWith("q)") || line.StartsWith("r)") || line.StartsWith("s)") || line.StartsWith("t)")
                || line.StartsWith("u)") || line.StartsWith("v)") || line.StartsWith("x)") || line.StartsWith("z)")
                || line.StartsWith("y)") || line.StartsWith("w)"))
            {
                parser.AddAlinea(ref line);
            }
            else
            {
                parser.AddATexto(ref line);
            }
        }
    }
}