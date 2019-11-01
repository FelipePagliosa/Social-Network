using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Felipe_Pagliosa_PB.Models {

    public class Post{

        [Key]
        public int IdPost { get; set; }

        public int IdUsuario { get; set; }

        public int IdPostOrigem { get; set; }

        [Required(ErrorMessage = "Escreva algo")]
        [Display(Name = "Texto da publicação")]
        public string TextoPub { get; set; }

        [Display(Name = "Curtidas")]
        public int NumCurtidasPost { get; set; }

        [Required(ErrorMessage = "Escreva algo")]
        [Display(Name = "Imagem da publicação")]
        public string ImagemPost { get; set; }

        [Display(Name = "Usuario")]
        public string NomeUsuario { get; set; }

        public List<Comentario> Comentario { get; set; }

        [Display(Name = "Data em que foi postado")]
        [DataType(DataType.DateTime)]
        public DateTime DataPost { get; set; }

    }
}