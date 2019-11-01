using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Felipe_Pagliosa_PB.Models {

    public class Publicacao{

        [Key]
        public int IdPost { get; set; }

        public int IdUsuario { get; set; }
        public string TextoPub { get; set; }
        public int NumCurtidasPost { get; set; }
        public string ImagemPost { get; set; }
        public List<Comentario> Comentario { get; set; }
    }
}