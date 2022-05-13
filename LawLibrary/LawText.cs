using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawLibrary
{
    /// <summary>
    /// Representa um texto da lei.
    /// </summary>
    public class LawText
    {
        /// <summary>
        /// Obtém ou define o texto da lei.
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Obtém ou define o tipo de conteúdo que representa este texto da lei.
        /// </summary>
        public LawContentType ContentType { get; set; } = LawContentType.Texto;

        /// <summary>
        /// Obtém ou define os conteúdos seguintes que pertecem a esse texto.
        /// </summary>
        public List<LawText>? Contents { get; set; } = null;

        /// <summary>
        /// Adiciona um conteúdo a lista SubContents.
        /// </summary>
        /// <param name="lawText"></param>
        public void Add(LawText lawText)
        {
            if (Contents == null)
                Contents = new List<LawText>();

            Contents.Add(lawText);
        }        

        public List<LawText> GetAll(LawContentType content)
        {
            List<LawText> result = new List<LawText>();

            if (Contents == null)
                return result;

            GetAll(content, result, Contents);

            return result;
        }

        private void GetAll(LawContentType content, List<LawText> result, List<LawText>? list)
        {
            if (list == null)
                return;

            foreach (LawText lawText in list)
            {
                if (lawText.ContentType == content)
                {
                    result.Add(lawText);
                }
                else if ((int)lawText.ContentType < (int)content)
                {
                    GetAll(content, result, lawText.Contents);
                }
            }
        }

        public override string ToString()
        {
            return $"{ContentType}: {Text}";
        }
    }
}
