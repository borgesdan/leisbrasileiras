using Microsoft.AspNetCore.Mvc;
using FileIO = System.IO.File;
using System.Text.Json;
using System.Text.Json.Nodes;
using LawLibrary;

namespace LeisBrasileirasAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LeiController : Controller
    {
        readonly string filesPath = "Files\\Leis";

        /// <summary>Obtém o objeto LawParser ao informar o número da lei.</summary>
        async Task<LawParser?> Parse(int leiNumero)
        {
            string fileName = $"{filesPath}\\{leiNumero}.json";
            return await LawParser.LoadAsync(LawParserSaveType.Json, fileName);
        }

        /// <summary>Obtém todas as ocorrências do tipo especifícado na parte normativa da lei.</summary>
        async Task<ActionResult<IEnumerable<LawText>>> GetNormativeAll(int lei, LawContentType contentType)
        {
            var parser = await Parse(lei);

            if (parser == null)
            {
                return NotFound();
            }

            var list = parser.Law.NormativePart.Where(x => x.ContentType == contentType);

            return list.Any() ? Ok(list) : BadRequest();
        }

        /// <summary>Obtém todo o conteúdo da lei.</summary>
        [HttpGet("{lei:int}")]
        public async Task<ActionResult<JsonDocument>> Get(int lei)
        {
            string fileName = $"{filesPath}\\{lei}.json";

            if (!FileIO.Exists(fileName))
            {
                return NotFound();
            }

            var json = await JsonDocument.ParseAsync(new FileStream(fileName, FileMode.Open, FileAccess.Read));

            return json != null ? json : BadRequest();
        }

        [HttpGet("{lei:int}/epigrafe")]
        public async Task<ActionResult<LawText>> GetEpigrafe(int lei)
        {
            var parser = await Parse(lei);

            if (parser == null)
            {
                return NotFound();
            }

            var result = parser.Law.PreliminaryPart.FirstOrDefault(x => x.ContentType == LawContentType.Epigrafe);

            return result != null ? Ok(result) : BadRequest();
        }

        [HttpGet("{lei:int}/ementa")]
        public async Task<ActionResult<LawText>> GetEmenta(int lei)
        {
            var parser = await Parse(lei);

            if (parser == null)
            {
                return NotFound();
            }

            var result = parser.Law.PreliminaryPart.FirstOrDefault(x => x.ContentType == LawContentType.Ementa);

            return result != null ? Ok(result) : BadRequest();
        }

        [HttpGet("{lei:int}/preambulo")]
        public async Task<ActionResult<LawText>> GetPreambulo(int lei)
        {
            var parser = await Parse(lei);

            if (parser == null)
            {
                return NotFound();
            }

            var result = parser.Law.PreliminaryPart.FirstOrDefault(x => x.ContentType == LawContentType.Preambulo);

            return result != null ? Ok(result) : BadRequest();
        }

        /// <summary>Obtém todas as ocorrências da divisão Parte na parte normativa da lei.</summary>
        [HttpGet("{lei:int}/partes")]
        public async Task<ActionResult<IEnumerable<LawText>>> GetAllPartes(int lei)
        {
            return await GetNormativeAll(lei, LawContentType.Parte);
        }

        /// <summary>Obtém todas as ocorrências da divisão Livro na parte normativa da lei.</summary>
        [HttpGet("{lei:int}/livros")]
        public async Task<ActionResult<IEnumerable<LawText>>> GetAllLivros(int lei)
        {
            return await GetNormativeAll(lei, LawContentType.Livro);
        }

        //public async Task<ActionResult<IEnumerable<LawText>>> GetAllLivros(int lei, int parte) { }

        /// <summary>Obtém todas as ocorrências da divisão Titulo na parte normativa da lei.</summary>
        [HttpGet("{lei:int}/titulos")]
        public async Task<ActionResult<IEnumerable<LawText>>> GetAllTitulos(int lei)
        {
            return await GetNormativeAll(lei, LawContentType.Titulo);
        }

        /// <summary>Obtém todas as ocorrências da divisão Capítulo na parte normativa da lei.</summary>
        [HttpGet("{lei:int}/capitulos")]
        public async Task<ActionResult<IEnumerable<LawText>>> GetAllCapitulos(int lei)
        {
            return await GetNormativeAll(lei, LawContentType.Capitulo);
        }

        /// <summary>Obtém todas as ocorrências da divisão Artigos na parte normativa da lei.</summary>
        [HttpGet("{lei:int}/artigos")]
        public async Task<ActionResult<IEnumerable<LawText>>> GetAllArtigos(int lei)
        {
            var parser = await Parse(lei);

            if(parser == null)
            {
                return NotFound();
            }

            List<LawText> list = new List<LawText>();
            foreach(var item in parser.Law.NormativePart)
            {
                var result = item.GetAll(LawContentType.Artigo);

                if (result.Any())
                {
                    list.AddRange(result);
                }
            }

            return Ok(list);
        }


        //[HttpGet("{lei:int}/titulo/{titulo:int}")]
        //public ActionResult<LawText> GetTitulo(int lei, int titulo)
        //{
        //    if (titulo < 1)
        //    {
        //        return NotFound();
        //    }

        //    string fileName = $"{filesPath}\\{lei}.json";
        //    var parser = LawParser.Load(LawParserSaveType.Json, fileName);

        //    if (parser == null)
        //    {
        //        return NotFound();
        //    }

        //    var list = parser.Law.NormativePart.Where(x => x.ContentType == LawContentType.Titulo).ToList();

        //    if (!list.Any())
        //    {
        //        foreach (var item in parser.Law.NormativePart)
        //        {
        //            var all = item.GetAll(LawContentType.Titulo);

        //            if (all.Any())
        //                list.AddRange(all);
        //        }
        //    }

        //    return list[titulo - 1];
        //}

        //[HttpGet("{lei:int}/titulo/{titulo:int}/capitulos")]
        //public ActionResult<List<LawText>> GetAllCapitulos(int lei, int titulo)
        //{
        //    if (titulo < 0)
        //    {
        //        return NotFound();
        //    }

        //    string fileName = $"{filesPath}\\{lei}.json";
        //    var parser = LawParser.Load(LawParserSaveType.Json, fileName);

        //    if (parser == null)
        //    {
        //        return NotFound();
        //    }

        //    //Considerar que uma lei possa não ter títulos como a LEI COMPLEMENTAR Nº 123, DE 14 DE DEZEMBRO DE 2006            
        //    if (titulo == 0)
        //    {
        //        var list = parser.Law.NormativePart.Where(x => x.ContentType == LawContentType.Capitulo).ToList();

        //        if (!list.Any())
        //        {
        //            foreach (var item in parser.Law.NormativePart)
        //            {
        //                var all = item.GetAll(LawContentType.Capitulo);

        //                if (all.Any())
        //                    list.AddRange(all);
        //            }
        //        }

        //        return Ok(list);
        //    }
        //    else
        //    {
        //        var t = GetTitulo(lei, titulo).Value;

        //        if (t == null)
        //        {
        //            return NotFound();
        //        }

        //        return t.GetAll(LawContentType.Capitulo);
        //    }
        //}

        //[HttpGet("{lei:int}/titulo/{titulo:int}/capitulo/{capitulo:int}")]
        //public ActionResult<LawText> GetCapitulo(int lei, int titulo, int capitulo)
        //{
        //    if (titulo < 0)
        //    {
        //        return NotFound();
        //    }

        //    string fileName = $"{filesPath}\\{lei}.json";
        //    var parser = LawParser.Load(LawParserSaveType.Json, fileName);

        //    if (parser == null)
        //    {
        //        return NotFound();
        //    }

        //    if (titulo == 0)
        //    {
        //        var list = parser.Law.NormativePart.Where(x => x.ContentType == LawContentType.Capitulo).ToList();

        //        if (!list.Any())
        //        {
        //            foreach (var item in parser.Law.NormativePart)
        //            {
        //                var all = item.GetAll(LawContentType.Capitulo);

        //                if (all.Any())
        //                    list.AddRange(all);
        //            }
        //        }

        //        return list[capitulo - 1];
        //    }
        //    else
        //    {
        //        var t = GetTitulo(lei, titulo).Value;

        //        if (t == null)
        //        {
        //            return NotFound();
        //        }

        //        return t.GetAll(LawContentType.Capitulo)[capitulo - 1];
        //    }
        //}
    }
}
