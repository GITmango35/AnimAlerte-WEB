using System;
using System.Collections.Generic;

#nullable disable

namespace AnimAlerte.Models
{
    public partial class Image
    {
        public int IdImage { get; set; }
        public string TitreImage { get; set; }
        public string PathImage { get; set; }
        public int? IdAnimal { get; set; }

        public virtual Animal IdAnimalNavigation { get; set; }
    }
}
