using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawLibrary
{
    /// <summary>
    /// Representa a estrutura formal de uma lei brasileira.
    /// </summary>
    public class Law
    {
        public List<LawText> PreliminaryPart { get; set; } = new();
        public List<LawText> NormativePart { get; set; } = new();
        public List<LawText> FinalPart { get; set; } = new();
    }
}