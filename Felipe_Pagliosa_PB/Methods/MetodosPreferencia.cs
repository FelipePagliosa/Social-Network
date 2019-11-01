using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Felipe_Pagliosa_PB.Models;
using Felipe_Pagliosa_PB.Repository;
using System.Configuration;
using System.IO;

namespace Felipe_Pagliosa_PB.Methods {
    public class MetodosPreferencia {
        public static List<Usuario> SearchAmigo(string pesquisa,Usuario usuario) {


            var resultado = new List<Usuario>();
            if(pesquisa != "") {
                foreach (Usuario s in UsuariosRepository.ListAllUsers(usuario)) {
                    if (s.Preferencia1.Contains(pesquisa) || (s.Preferencia2.Contains(pesquisa) || (s.Preferencia3.Contains(pesquisa) || s.NomeUsuario.Contains(pesquisa)))) {

                        resultado.Add(s);
                    }
                }               
            }
            return resultado;
        }
    }
}