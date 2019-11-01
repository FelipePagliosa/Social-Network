using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Felipe_Pagliosa_PB.Models {

    public class Grupo {
        public string NomeGrupo { get; set; }
        public int CriadorGrupo { get; set; }
        public List<Usuario> ListaMembros { get; set; }
        public List<Post> Publicacoes { get; set; }
    }
}
