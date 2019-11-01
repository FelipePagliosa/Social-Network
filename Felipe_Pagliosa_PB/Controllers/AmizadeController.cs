using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Felipe_Pagliosa_PB.Models;
using Felipe_Pagliosa_PB.Repository;
using System.Configuration;
using System.IO;

namespace Felipe_Pagliosa_PB.Controllers
{
    public class AmizadeController : Controller
    {
        // GET: Amizade/Create
        public ActionResult ListConvites()
        {
            try {
                Usuario usuario = (Usuario)Session["object"];
                List<Amizade> convitesAmizade = AmizadeRepository.ListAllConvitesAmizade(usuario);
                Session["object"] = UsuariosRepository.SelectUsuario(usuario.Id);
                return View(convitesAmizade);
            }
            catch {
                return View("Error");
            }
        }

        public ActionResult AceitarConvite(int id) {
            try {
                Usuario usuario = (Usuario)Session["object"];
                AmizadeRepository.AceitarConviteAmizade(id, usuario);
                Session["object"] = UsuariosRepository.SelectUsuario(usuario.Id);
                TempData["alert"] = "Convite de amizade aceito";
                return RedirectToAction("ListConvites", "Amizade");
            }
            catch {
                return View("Error");
            }
        }

        public ActionResult ListAmigos() {
            try {
                Usuario usuario = (Usuario)Session["object"];
                List<Amizade> amigos = AmizadeRepository.ListAllAmigos(usuario);
                Session["object"] = UsuariosRepository.SelectUsuario(usuario.Id);
                return View(amigos);
            }
            catch {
                return View("Error");
            }
        }

        public ActionResult ExcluirAmizade(int ido, int idd) {
            try {
                AmizadeRepository.ExcluirAmizade(ido, idd);
                Usuario usuario = (Usuario)Session["object"];
                Session["object"] = UsuariosRepository.SelectUsuario(usuario.Id);
                TempData["alert"] = "Amigo excluido";
                return RedirectToAction("ListAmigos", "Amizade");
            }
            catch {
                return View("Error");
            }
        }
    }
}
