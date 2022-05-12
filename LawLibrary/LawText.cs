using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawLibrary
{
    /// <summary>
    /// Representa um texto da lei sem função específica.
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
        public List<LawText>? SubContents { get; set; } = null;

        /// <summary>
        /// Adiciona um conteúdo a lista SubContents.
        /// </summary>
        /// <param name="lawText"></param>
        public void Add(LawText lawText)
        {
            if(SubContents == null)
                SubContents = new List<LawText>();

            SubContents.Add(lawText);
        }

        public override string ToString()
        {
            return $"{ContentType}: {Text}";
        }
    }
}
