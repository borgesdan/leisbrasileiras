namespace LawLibrary
{
    /// <summary>
    /// Representa uma analisador do arquivo de texto que contém a lei e converte em um objeto com formato hierárquico.
    /// </summary>
    public class LawParser
    {
        const string TEXT_MARK_EPIGRAFE = "[__EPIGRAFRE__]";
        const string TEXT_MARK_EMENTA = "[__EMENTA__]";
        const string TEXT_MARK_PREAMBULO = "[__PREAMBULO__]";
        const string TEXT_MARK_NORMATIVA = "[__NORMATIVA__]";
        const string TEXT_MARK_FINAL = "[__FINAL__]";

        string textFile = string.Empty;
        Law law = new();
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
        public Law Law { get => law; private set => law = value; }

        /// <summary>
        /// Obtém ou define o arquivo de texto que contém os textos da lei e suas configurações.
        /// </summary>
        public string TextFile { get => textFile; private set => textFile = value; }

        /// <summary>
        /// Inicializa uma nova instância da classe.
        /// </summary>
        private LawParser() { }

        /// <summary>
        /// Inicializar uma nova instância da classe.
        /// </summary>
        /// <param name="file">Define o arquivo de texto configurado para análise.</param>
        public LawParser(string file)
        {
            this.textFile = file;
        }

        private static int GerWordsCount(ref string text)
        {
            return text.Split(' ').Length;
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
            law.Stats.Partes++;
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

            law.Stats.Livros++;
            law.Stats.Linhas++;
            law.Stats.Palavras += GerWordsCount(ref text);
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

            law.Stats.Titulos++;
            law.Stats.Linhas++;
            law.Stats.Palavras += GerWordsCount(ref text);
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

            law.Stats.Capitulos++;
            law.Stats.Linhas++;
            law.Stats.Palavras += GerWordsCount(ref text);
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
            else if (currentTitulo != null)
            {
                currentTitulo.Add(currentSecao);
            }
            else
            {
                law.NormativePart.Add(currentSecao);
            }

            law.Stats.Secoes++;
            law.Stats.Linhas++;
            law.Stats.Palavras += GerWordsCount(ref text);
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

            law.Stats.SubSecoes++;
            law.Stats.Linhas++;
            law.Stats.Palavras += GerWordsCount(ref text);
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
            else if (currentTitulo != null)
            {
                currentTitulo.Add(currentArtigo);
            }

            law.Stats.Artigos++;
            law.Stats.Linhas++;
            law.Stats.Palavras += GerWordsCount(ref text);
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

            law.Stats.Paragrafos++;
            law.Stats.Linhas++;
            law.Stats.Palavras += GerWordsCount(ref text);
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

            law.Stats.Incisos++;
            law.Stats.Linhas++;
            law.Stats.Palavras += GerWordsCount(ref text);
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

            law.Stats.Alineas++;
            law.Stats.Linhas++;
            law.Stats.Palavras += GerWordsCount(ref text);
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

            law.Stats.Outros++;
            law.Stats.Linhas++;
            law.Stats.Palavras += GerWordsCount(ref text);
        }

        /// <summary>
        /// Inicia a análise do texto e o converte em um objeto hierárquico. Retorna true caso bem sucedido.
        /// </summary>
        public bool Parse()
        {
            if (textFile == null 
                || string.IsNullOrWhiteSpace(textFile) 
                || !File.Exists(textFile))
                return false;

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
                        prelimaryContentType = LawContentType.Texto;
                        break;
                    case LawPartType.Normativa:
                        LawParserNormativePart.Parse(ref line, this);
                        break;
                    case LawPartType.Final:
                        law.FinalPart.Add(new LawText() { Text = line, ContentType = LawContentType.Texto });
                        break;
                }
            }

            return true;
        }        

        /// <summary>
        /// Salva o objeto no formato desejado.
        /// </summary>
        /// <param name="saveType">O mtipo de arquivo desejado.</param>
        /// <param name="outputFile">O caminho do arquivo a ser criado.</param>
        /// <param name="indent">Defina TRUE caso o texto deva estar indentado.</param>
        public void Save(LawParserSaveType saveType, string outputFile, bool indent = true)
        {
            switch (saveType)
            {
                case LawParserSaveType.Text:
                    LawParserText.Save(this, outputFile, indent);
                    break;
                case LawParserSaveType.Xml:
                    LawParserXml.Save(this, outputFile, indent);
                    break;
                case LawParserSaveType.Json:
                    LawParserJson.Save(this, outputFile, indent);
                    break;
            }
        }

        /// <summary>
        /// Carrega e converte um arquivo salvo pelo método Save desta classe.
        /// </summary>
        /// <param name="fileType"></param>
        /// <param name="outputFile"></param>
        /// <returns></returns>
        public static LawParser? Load(LawParserSaveType fileType, string outputFile)
        {
            switch (fileType)
            {
                case LawParserSaveType.Json:
                    LawParser parser = new LawParser();
                    parser.Law = LawParserJson.Load(outputFile) ?? new Law();
                    return parser;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Carrega e converte um arquivo salvo pelo método Save desta classe.
        /// </summary>
        /// <param name="fileType"></param>
        /// <param name="outputFile"></param>
        /// <returns></returns>
        public static async Task<LawParser?> LoadAsync(LawParserSaveType fileType, string outputFile)
        {
            switch (fileType)
            {
                case LawParserSaveType.Json:
                    return new LawParser() { Law = await LawParserJson.LoadAsync(outputFile) ?? new Law() };
                default:
                    return null;
            }
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
        public async Task SaveAsync(LawParserSaveType saveType, string outputFile, bool indent)
        {
            await Task.Run(() => { Save(saveType, outputFile, indent); });
        }
    }
}