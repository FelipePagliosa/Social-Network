using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Felipe_Pagliosa_PB.Models {

    public class Comentario {

        [Key]
        public int IdComentario { get; set; }

        public int IdUsuario { get; set; }

        public int IdPost { get; set; }

        [Display(Name = "Usuario")]
        public string NomeUsuario { get; set; }

        [Required(ErrorMessage = "Escreva algo")]
        [Display(Name = "Texto da publicação")]
        public string TextoComent { get; set; }

        [Display(Name = "Número de curtidas")]
        public int NumCurtidasComent { get; set; }

        [Display(Name = "Data em que foi postado")]
        public DateTime DataComentario { get; set; }
    }
}