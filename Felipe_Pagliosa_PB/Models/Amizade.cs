using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Felipe_Pagliosa_PB.Models {

    public class Amizade {
        public int IdUsuarioOrigem { get; set; }

        public int IdUsuarioDestino { get; set; }

        public bool Aceito { get; set; }

        [Display(Name = "Usuario")]
        public string NomeUsuario { get; set; }

        public string Preferencia1 { get; set; }

        public string Preferencia2 { get; set; }

        public string Preferencia3 { get; set; }
    }
}