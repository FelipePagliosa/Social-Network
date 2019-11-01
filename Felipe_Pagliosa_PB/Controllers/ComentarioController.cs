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
    public class ComentarioController : Controller
    {


        // GET: Comentario/Create
        public ActionResult Comentar()
        {         
            return View();
        }

        // POST: Comentario/Create
        [HttpPost]
        public ActionResult Comentar(FormCollection collection,Comentario comentario,int id,bool kek)
        {
            try
            {
                Usuario teste = (Usuario)Session["object"];
                comentario.IdPost = id;
                comentario.IdUsuario = teste.Id;
                ComentariosRepository.AddComentario(comentario);
                Session["object"] = UsuariosRepository.SelectUsuario(teste.Id);
                if (kek==false)
                {
                    return RedirectToAction("Welcome", "Usuario");
                }
                else
                {
                    return RedirectToAction("ListPosts", "Post");

                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Comentario/Edit/5
        public ActionResult EditComentario(int id)
        {
            var comentario = ComentariosRepository.SelectComent(id);
            return View(comentario);
        }

        // POST: Comentario/Edit/5
        [HttpPost]
        public ActionResult EditComentario(Comentario comentario, FormCollection collection,bool kek)
        {
            try
            {
                // TODO: Add update logic here
                ComentariosRepository.EditComentario(comentario);
                Usuario sessao = (Usuario)Session["object"];
                Session["object"] = UsuariosRepository.SelectUsuario(sessao.Id);
                if (kek == false)
                {
                    return RedirectToAction("Welcome", "Usuario");
                }
                else
                {
                    return RedirectToAction("ListPosts", "Post");

                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Comentario/Delete/5
        public ActionResult DeleteComentario(int id,bool kek)
        {
            try {
                CurtidasComentariosRepository.DeleteAllCurtidasComentario(ComentariosRepository.SelectComent(id));
                ComentariosRepository.DeleteComentario(id);
                Usuario sessao = (Usuario)Session["object"];
                Session["object"] = UsuariosRepository.SelectUsuario(sessao.Id);
                if (kek == false)
                {
                    return RedirectToAction("Welcome", "Usuario");
                }
                else
                {
                    return RedirectToAction("ListPosts", "Post");

                }
            }
            catch {
                return View("Error");
            }           
        }

        public ActionResult LikeComentario(int idc,bool kek) {
            var resultado = ComentariosRepository.SelectComent(idc);
            Usuario teste = (Usuario)Session["object"];


            try {
                if (CurtidasComentariosRepository.ChecarCurtidaComentario(idc,teste.Id) == false) {
                    ComentariosRepository.LikeComentario(resultado);
                    CurtidasComentariosRepository.AddCurtidaComentarioBanco(idc,teste.Id);
                }
                else {
                    ComentariosRepository.DislikeComentario(resultado);
                    CurtidasComentariosRepository.DeleteCurtidaComentarioBanco(idc, teste.Id);
                }
                Usuario sessao = (Usuario)Session["object"];
                Session["object"] = UsuariosRepository.SelectUsuario(sessao.Id);

                if (kek == false)
                {
                    return RedirectToAction("Welcome", "Usuario");
                }
                else
                {
                    return RedirectToAction("ListPosts", "Post");

                }
            }
            catch {
                return View("Error");
            }
        }
    }
}
