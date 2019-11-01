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
    public class PublicacaoController : Controller
    {
        // GET: Publicacao
        public ActionResult ListPosts()           
        {
            Usuario usuario = (Usuario)Session["object"];
            List<Publicacao> listPost = PostsRepository.ListAllPostsFromUser(usuario);
            List<Comentario> listComentarios = new List<Comentario>();
            //foreach(var item in listPost) {
            //    foreach(var coment in item.Comentario) {
            //        listComentarios.Add(coment);

            //    }
            //}
            return View(listPost);
        }

        // GET: Publicacao/Post
        public ActionResult CreatePost() {
            return View();
        }

        // POST: Publicacao/CreatePost
        [HttpPost]
        public ActionResult CreatePost(FormCollection collection, Publicacao post, HttpPostedFileBase file) {
            try {
                Usuario teste = (Usuario)Session["object"];
                post.IdUsuario = teste.Id;
                PostsRepository.AddPost(post, file);
                return RedirectToAction("Welcome","User");
            }
            catch {
                return View();
            }
        }

        // GET: Publicacao/Edit/5
        public ActionResult EditPost(int id) {
            Publicacao post=PostsRepository.SelectPost(id);
            return View(post);
        }

        // POST: Publicacao/Edit/5
        [HttpPost]
        public ActionResult EditPost(Publicacao post, FormCollection collection, HttpPostedFileBase file)
        {
            try
            {
                // TODO: Add update logic here
                PostsRepository.EditPost(post,file);
                return RedirectToAction("ListPosts", "Publicacao");
            }
            catch
            {
                return View();
            }
        }

        // GET: Publicacao/Delete/5
        public ActionResult DeletePost(int id)
        {
            PostsRepository.DeletePost(id);
            return RedirectToAction("ListPosts", "Publicacao");
        }

        public ActionResult LikePost(int id) {
            var resultado=PostsRepository.SelectPost(id);
            if (CurtidasPostsRepository.ChecarCurtidaPost(resultado)==false) {
                PostsRepository.LikePost(resultado);
                CurtidasPostsRepository.AddCurtidaPostBanco(resultado);
            }
            else {
                PostsRepository.DislikePost(resultado);
                CurtidasPostsRepository.DeleteCurtidaPostBanco(resultado);
            }
            return RedirectToAction("ListPosts", "Publicacao");
        }
    }
}
