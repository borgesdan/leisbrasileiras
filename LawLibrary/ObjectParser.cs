using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LawLibrary
{
    /// <summary>
    /// Representa uma analisador do arquivo de texto que contém a lei e converte em um objeto com formato hierárquico.
    /// </summary>
    public class ObjectParser
    {
        const string TEXT_MARK_EPIGRAFE = "[__EPIGRAFRE__]";
        const string TEXT_MARK_EMENTA = "[__EMENTA__]";
        const string TEXT_MARK_PREAMBULO = "[__PREAMBULO__]";
        const string TEXT_MARK_NORMATIVA = "[__NORMATIVA__]";
        const string TEXT_MARK_FINAL = "[__FINAL__]";

        readonly string textFile;
        readonly Law law = new();
        //bool isPreliminaryPart = false;
        //bool isNomartivePart = false;
        //bool isfinalPart = false;
        LawPartType currentLawPartType = LawPartType.Preliminar;
        LawContentType prelimaryContentType = LawContentType.Texto;

        LawText? currentParte = null;
        LawText? currentLivro = null;
        LawText? currentTitulo = null;
        LawText? currentCapitulo = null;
        LawText? currentSecao = null;
        LawText? currentSubSecao = null;
        LawText? currentArtigo = null;
        LawText? currentParagrafo = null;
        LawText? currentInciso = null;

        /// <summary>
        /// Obtém o objeto a conter os textos da lei em formato hierárquico.
        /// </summary>
        public Law Law { get => law; }

        /// <summary>
        /// Inicializar uma nova instância da classe.
        /// </summary>
        /// <param name="file">Define o arquivo de texto configurado para análise.</param>
        public ObjectParser(string file)
        {
            this.textFile = file;
        }

        /// <summary>
        /// Adiciona uma divisão da lei denominada Parte.
        /// </summary>
        public void AddParte(ref string text)
        {
            currentLivro = null;
            currentTitulo = null;
            currentCapitulo = null;
            currentSecao = null;
            currentSubSecao = null;
            currentArtigo = null;
            currentParagrafo = null;
            currentInciso = null;

            currentParte = new LawText()
            {
                Text = text,
                ContentType = LawContentType.Parte
            };

            law.NormativePart.Add(currentParte);
        }

        /// <summary>
        /// Adiciona uma divisão da lei denominada Livro.
        /// </summary>
        public void AddLivro(ref string text)
        {
            currentTitulo = null;
            currentCapitulo = null;
            currentSecao = null;
            currentSubSecao = null;
            currentArtigo = null;
            currentParagrafo = null;
            currentInciso = null;

            currentLivro = new LawText()
            {
                Text = text,
                ContentType = LawContentType.Livro
            };

            if (currentParte != null)
            {
                currentParte.Add(currentLivro);
            }
            else
            {
                law.NormativePart.Add(currentLivro);
            }
        }

        /// <summary>
        /// Adiciona uma divisão da lei denominada Título.
        /// </summary>
        public void AddTitulo(ref string text)
        {
            currentCapitulo = null;
            currentSecao = null;
            currentSubSecao = null;
            currentArtigo = null;
            currentParagrafo = null;
            currentInciso = null;

            currentTitulo = new LawText()
            {
                Text = text,
                ContentType = LawContentType.Titulo
            };

            if (currentLivro != null)
            {
                currentLivro.Add(currentTitulo);
            }
            else
            {
                law.NormativePart.Add(currentTitulo);
            }
        }

        /// <summary>
        /// Adiciona uma divisão da lei denominada Capítulo.
        /// </summary>
        public void AddCapitulo(ref string text)
        {
            currentSecao = null;
            currentSubSecao = null;
            currentArtigo = null;
            currentParagrafo = null;
            currentInciso = null;

            currentCapitulo = new LawText()
            {
                Text = text,
                ContentType = LawContentType.Capitulo
            };

            if (currentTitulo != null)
            {
                currentTitulo.Add(currentCapitulo);
            }
            else
            {
                law.NormativePart.Add(currentCapitulo);
            }
        }

        /// <summary>
        /// Adiciona uma divisão da lei denominada Seção ao último capítulo adicionado.
        /// </summary>
        public void AddSecao(ref string text)
        {
            currentSubSecao = null;
            currentArtigo = null;
            currentParagrafo = null;
            currentInciso = null;

            currentSecao = new LawText()
            {
                Text = text,
                ContentType = LawContentType.Secao
            };

            if (currentCapitulo != null)
            {
                currentCapitulo.Add(currentSecao);
            }
        }

        /// <summary>
        /// Adiciona uma divisão da lei denominada Subseção a última seção adicionada.
        /// </summary>
        public void AddSubSecao(ref string text)
        {
            currentArtigo = null;
            currentParagrafo = null;
            currentInciso = null;

            currentSubSecao = new LawText()
            {
                Text = text,
                ContentType = LawContentType.SubSecao
            };

            if (currentSecao != null)
            {
                currentSecao.Add(currentSubSecao);
            }
        }

        /// <summary>
        /// Adiciona um artigo da lei.
        /// </summary>
        public void AddArtigo(ref string text)
        {
            currentParagrafo = null;
            currentInciso = null;

            currentArtigo = new LawText()
            {
                Text = text,
                ContentType = LawContentType.Artigo
            };

            if (currentSubSecao != null)
            {
                currentSubSecao.Add(currentArtigo);
            }
            else if (currentSecao != null)
            {
                currentSecao.Add(currentArtigo);
            }
            else if (currentCapitulo != null)
            {
                currentCapitulo.Add(currentArtigo);
            }
        }

        /// <summary>
        /// Adiciona um parágrafo da lei.
        /// </summary>
        public void AddParagrafo(ref string text)
        {
            currentInciso = null;

            currentParagrafo = new LawText()
            {
                Text = text,
                ContentType = LawContentType.Paragrafo
            };

            if (currentArtigo != null)
            {
                currentArtigo.Add(currentParagrafo);
            }
        }

        /// <summary>
        /// Adiciona um incíso da lei.
        /// </summary>
        public void AddInciso(ref string text)
        {
            currentInciso = new LawText()
            {
                Text = text,
                ContentType = LawContentType.Inciso
            };

            if (currentArtigo != null)
            {
                currentArtigo.Add(currentInciso);
            }
        }

        /// <summary>
        /// Adiciona uma alínea da lei.
        /// </summary>
        public void AddAlinea(ref string text)
        {
            var alinea = new LawText()
            {
                Text = text,
                ContentType = LawContentType.Alinea
            };

            if (currentInciso != null)
            {
                currentInciso.Add(alinea);
            }
            else if (currentArtigo != null)
            {
                currentArtigo.Add(alinea);
            }
        }

        /// <summary>
        /// Adiciona um texto independente da lei.
        /// </summary>
        public void AddATexto(ref string text)
        {
            var texto = new LawText()
            {
                Text = text,
                ContentType = LawContentType.Texto
            };

            if (currentArtigo != null)
            {
                //currentArtigo.SubContents.Add(texto);
                currentArtigo.Add(texto);
            }
            else if (currentSubSecao != null)
            {
                //currentSubSecao.SubContents.Add(texto);
                currentSubSecao.Add(texto);
            }
            else if (currentSecao != null)
            {
                //currentSecao.SubContents.Add(texto);
                currentSecao.Add(texto);
            }
            else if (currentCapitulo != null)
            {
                //currentCapitulo.SubContents.Add(texto);
                currentCapitulo.Add(texto);
            }
            else if (currentTitulo != null)
            {
                //currentTitulo.SubContents.Add(texto);
                currentTitulo.Add(texto);
            }
            else if (currentLivro != null)
            {
                //currentLivro.SubContents.Add(texto);
                currentLivro.Add(texto);
            }
            else if (currentParte != null)
            {
                //currentParte.SubContents.Add(texto);
                currentParte.Add(texto);
            }
            else
            {
                law.NormativePart.Add(texto);
            }
        }

        /// <summary>
        /// Inicia a análise do texto e o converte em um objeto hierárquico.
        /// </summary>
        public void Parse()
        {
            using StreamReader sr = new(textFile);

            while (!sr.EndOfStream)
            {
                string? line = sr.ReadLine();

                if (line == null || string.IsNullOrWhiteSpace(line))
                    continue;

                line = line.Trim();                

                switch (line)
                {
                    case TEXT_MARK_EPIGRAFE:
                        currentLawPartType = LawPartType.Preliminar;
                        prelimaryContentType = LawContentType.Epigrafe;
                        continue;                        
                    case TEXT_MARK_EMENTA:
                        currentLawPartType = LawPartType.Preliminar;
                        prelimaryContentType = LawContentType.Ementa;
                        continue;
                    case TEXT_MARK_PREAMBULO:
                        currentLawPartType = LawPartType.Preliminar;
                        prelimaryContentType = LawContentType.Preambulo;
                        continue;
                    case TEXT_MARK_NORMATIVA:
                        currentLawPartType = LawPartType.Normativa;
                        continue;
                    case TEXT_MARK_FINAL:
                        currentLawPartType = LawPartType.Final;
                        continue;
                }

                switch (currentLawPartType)
                {
                    case LawPartType.Preliminar:
                        law.PreliminaryPart.Add(new LawText() { Text = line, ContentType = prelimaryContentType });
                        break;
                    case LawPartType.Normativa:
                        NormativePartParse.Parse(ref line, this);
                        break;
                    case LawPartType.Final:
                        law.FinalPart.Add(new LawText() { Text = line, ContentType = LawContentType.Texto });
                        break;
                }
            }
        }        

        /// <summary>
        /// Salva o objeto no formato desejado.
        /// </summary>
        /// <param name="saveType">O mtipo de arquivo desejado.</param>
        /// <param name="outputFile">O caminho do arquivo a ser criado.</param>
        /// <param name="indent">Defina TRUE caso o texto deva estar indentado.</param>
        public void Save(ObjectParserSaveType saveType, string outputFile, bool indent)
        {
            switch (saveType)
            {
                case ObjectParserSaveType.Text:
                    SaveAsText(outputFile, indent);
                    break;
                case ObjectParserSaveType.Xml:
                    SaveAsXml(outputFile, indent);
                    break;
            }
        }        

        private void SaveAsXml(string outputFile, bool indent)
        {
            using XmlWriter writer = XmlWriter.Create(outputFile, new XmlWriterSettings() { Indent = indent });

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
            SetXmlElements(writer, law.NormativePart);
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

        private static void SetXmlElements(XmlWriter writer, List<LawText>? list)
        {
            if (list == null)
                return;

            foreach (LawText t in list)
            {
                writer.WriteStartElement(GetXmlTag(t.Text));
                writer.WriteElementString("valor", t.Text);

                if (t.SubContents != null && t.SubContents.Count > 0)
                {
                    SetXmlElements(writer, t.SubContents);
                }

                writer.WriteEndElement();
            }
        }

        private static string GetXmlTag(string text)
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

        private void SaveAsText(string outputFile, bool indent)
        {
            List<string> lines = new();

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

            File.AppendAllLines(outputFile, lines);
        }

        //Campos estáticos do método SetLines para melhor indentificação.
        private static int indentationCount = 0;
        private const char tabChar = '\t';

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


        /// <summary>
        /// Inicia a análise do texto e o converter em um objeto hierárquico de modo assíncrono.
        /// </summary>
        /// <returns></returns>
        public async Task ParseAsync()
        {
            await Task.Run(Parse);
        }

        /// <summary>
        /// Salva o objeto no formato desejado assíncronamente.
        /// </summary>
        /// <param name="saveType">O mtipo de arquivo desejado.</param>
        /// <param name="outputFile">O caminho do arquivo a ser criado.</param>
        /// <param name="indent">Defina TRUE caso o texto deva estar indentado.</param>
        public async Task SaveAsync(ObjectParserSaveType saveType, string outputFile, bool indent)
        {
            await Task.Run(() => { Save(saveType, outputFile, indent); });
        }
    }
}