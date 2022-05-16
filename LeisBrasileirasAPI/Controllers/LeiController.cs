using LawLibrary;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using FileIO = System.IO.File;

namespace LeisBrasileirasAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LeiController : Controller
    {
        readonly string[] specialKeys = new string[1]
        {
            "constituicao",
        };

        readonly string filesPath = "Files\\Leis";
        readonly string currentConstituicao = "constituicao88";

        // Obtém o objeto LawParser ao informar o número da lei.
        async Task<LawParser?> Parse(string lei)
        {
            CheckKey(ref lei);

            string fileName = $"{filesPath}\\{lei}.json";
            return await LawParser.LoadAsync(LawParserSaveType.Json, fileName);
        }

        //Obtém todas as ocorrências do tipo especifícado na parte normativa da lei.
        async Task<IEnumerable<LawText>?> GetNormativeAll(string lei, LawContentType contentType)
        {
            var parser = await Parse(lei);

            if (parser == null)
            {
                return null;
            }

            List<LawText> list = new();
            foreach (var item in parser.Law.NormativePart)
            {
                if((int)item.ContentType > (int)contentType)
                {
                    continue;
                }

                // Se o primeiro item a ser procurado for do tipo específicado
                // então deve-se adicioná-lo a lista e continuar o foreach.
                // Já que um Capítulo não contém outro Capítulo, ou um Artigo não contém outro Artigo,
                // não precisa então procurar no seu conteúdo.
                if (item.ContentType == contentType)
                {
                    list.Add(item);
                    continue;
                }

                var result = item.GetAll(contentType);

                if (result.Any())
                {
                    list.AddRange(result);
                }
            }

            return list.Any() ? list : null;
        }

        async Task<LawText?> GetNormativeSingle(string lei, string value, string startsMatch, LawContentType contentType)
        {
            var items = await GetNormativeAll(lei, contentType);

            if (items != null)
            {
                if (value.ToLower() == "unico")
                {
                    value = "único";
                }

                foreach (var i in items)
                {
                    var text = i.Text.ToLower();
                    startsMatch = startsMatch.ToLower();
                    value = value.ToLower();

                    string match = $"{startsMatch} {value}";
                    if (text.StartsWith(match))
                    {
                        return i;
                    }
                }
            }

            return null;
        }

        //Verifica se a entrada é uma chave especial a ser considerada.
        void CheckKey(ref string key)
        {
            if (key == specialKeys[0])
            {
                key = currentConstituicao;
            }
        }

        /// <summary>Obtém todo o conteúdo da lei.</summary>
        [HttpGet("{lei}")]        
        public async Task<ActionResult<JsonDocument>> GetLei(string lei)
        {
            CheckKey(ref lei);

            string fileName = $"{filesPath}\\{lei}.json";

            if (!FileIO.Exists(fileName))
            {
                return NotFound();
            }

            var json = await JsonDocument.ParseAsync(new FileStream(fileName, FileMode.Open, FileAccess.Read));

            return json != null ? json : BadRequest();
        }        

        /// <summary>Obtém as estatísticas</summary>
        [HttpGet("{lei}/stats")]
        public async Task<ActionResult<LawStats>> GetStats(string lei)
        {
            var parser = await Parse(lei);

            if(parser == null)
            {
                return NotFound();
            }

            return Ok(parser.Law.Stats);
        }

        //--------------------------------------------------//
        // $PARTE_PRELIMINAR                                //
        //--------------------------------------------------//

        /// <summary>Obtém todo o conteúdo da parte preliminar da lei.</summary>
        [HttpGet("{lei}/preliminar")]
        public async Task<ActionResult<IEnumerable<LawText>>> GetPreliminar(string lei)
        {
            var parser = await Parse(lei);

            if(parser == null)
            {
                return NotFound();
            }

            return Ok(parser.Law.PreliminaryPart);
        }

        /// <summary>Obtém todo o conteúdo da parte normativa da lei.</summary>
        [HttpGet("{lei}/normativa")]
        public async Task<ActionResult<IEnumerable<LawText>>> GetNormativa(string lei)
        {
            var parser = await Parse(lei);

            if (parser == null)
            {
                return NotFound();
            }

            return Ok(parser.Law.NormativePart);
        }

        /// <summary>Obtém todo o conteúdo da parte final da lei.</summary>
        [HttpGet("{lei}/final")]
        public async Task<ActionResult<IEnumerable<LawText>>> GetFinal(string lei)
        {
            var parser = await Parse(lei);

            if (parser == null)
            {
                return NotFound();
            }

            return Ok(parser.Law.FinalPart);
        }

        [HttpGet("{lei}/epigrafe")]
        public async Task<ActionResult<LawText>> GetEpigrafe(string lei)
        {
            var parser = await Parse(lei);

            if (parser == null)
            {
                return NotFound();
            }

            var result = parser.Law.PreliminaryPart.FirstOrDefault(x => x.ContentType == LawContentType.Epigrafe);

            return result != null ? Ok(result) : BadRequest();
        }

        [HttpGet("{lei}/ementa")]
        public async Task<ActionResult<LawText>> GetEmenta(string lei)
        {
            var parser = await Parse(lei);

            if (parser == null)
            {
                return NotFound();
            }

            var result = parser.Law.PreliminaryPart.FirstOrDefault(x => x.ContentType == LawContentType.Ementa);

            return result != null ? Ok(result) : BadRequest();
        }

        [HttpGet("{lei}/preambulo")]
        public async Task<ActionResult<LawText>> GetPreambulo(string lei)
        {
            var parser = await Parse(lei);

            if (parser == null)
            {
                return NotFound();
            }

            var result = parser.Law.PreliminaryPart.FirstOrDefault(x => x.ContentType == LawContentType.Preambulo);

            return result != null ? Ok(result) : BadRequest();
        }

        //--------------------------------------------------//
        // $GET_ALL                                         //
        //--------------------------------------------------//

        /// <summary>Obtém todas as ocorrências da divisão Parte na parte normativa da lei.</summary>
        [HttpGet("{lei}/partes")]
        public async Task<ActionResult<IEnumerable<LawText>>> GetAllPartes(string lei)
        {
            var result = await GetNormativeAll(lei, LawContentType.Parte);

            return result == null ? NotFound() : Ok(result);
        }

        /// <summary>Obtém todas as ocorrências da divisão Livro na parte normativa da lei.</summary>
        [HttpGet("{lei}/livros")]
        public async Task<ActionResult<IEnumerable<LawText>>> GetAllLivros(string lei)
        {
            var result = await GetNormativeAll(lei, LawContentType.Livro);

            return result == null ? NotFound() : Ok(result);
        }

        /// <summary>Obtém todas as ocorrências da divisão Titulo na parte normativa da lei.</summary>
        [HttpGet("{lei}/titulos")]
        public async Task<ActionResult<IEnumerable<LawText>>> GetAllTitulos(string lei)
        {
            var result = await GetNormativeAll(lei, LawContentType.Titulo);

            return result == null ? NotFound() : Ok(result);
        }

        /// <summary>Obtém todas as ocorrências da divisão Capítulo na parte normativa da lei.</summary>
        [HttpGet("{lei}/capitulos")]
        public async Task<ActionResult<IEnumerable<LawText>>> GetAllCapitulos(string lei)
        {
            var result = await GetNormativeAll(lei, LawContentType.Capitulo);

            return result == null ? NotFound() : Ok(result);
        }

        /// <summary>Obtém todas as ocorrências dos Artigos na parte normativa da lei.</summary>
        [HttpGet("{lei}/artigos")]
        public async Task<ActionResult<IEnumerable<LawText>>> GetAllArtigos(string lei)
        {
            var result = await GetNormativeAll(lei, LawContentType.Artigo);

            return result == null ? NotFound() : Ok(result);
        }

        //--------------------------------------------------//
        // $GET_SINGLE                                      //
        //--------------------------------------------------//

        [HttpGet("{lei}/partes/{value}")]
        public async Task<ActionResult<LawText>> GetParte(string lei, string value)
        {
            var parte = await GetNormativeSingle(lei, value, "Parte", LawContentType.Parte);
            return parte != null ? Ok(parte) : NotFound();
        }

        [HttpGet("{lei}/livros/{value}")]
        public async Task<ActionResult<LawText>> GetLivro(string lei, string value)
        {
            var livro = await GetNormativeSingle(lei, value, "Livro", LawContentType.Livro);
            return livro != null ? Ok(livro) : NotFound();
        }

        [HttpGet("{lei}/titulos/{value}")]
        public async Task<ActionResult<LawText>> GetTitulo(string lei, string value)
        {
            var titulo = await GetNormativeSingle(lei, value, "Título", LawContentType.Titulo);
            return titulo != null ? Ok(titulo) : NotFound();
        }

        [HttpGet("{lei}/capitulos/{value}")]
        public async Task<ActionResult<LawText>> GetCapitulo(string lei, string value)
        {
            var capitulo = await GetNormativeSingle(lei, value, "Capítulo", LawContentType.Capitulo);
            return capitulo != null ? Ok(capitulo) : NotFound();
        }

        [HttpGet("{lei}/artigos/{value}")]
        public async Task<ActionResult<LawText>> GetArtigo(string lei, string value)
        {
            var artigo = await GetNormativeSingle(lei, value, "Art.", LawContentType.Artigo);
            return artigo != null ? Ok(artigo) : NotFound();
        }
    }
}