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
    public class PostController : Controller
    {
        // GET: Publicacao
        public ActionResult ListPosts()           
        {
            try {
                Usuario usuario = (Usuario)Session["object"];
                List<Post> listPost = PostsRepository.ListAllPostsFromUser(usuario);               
                return View(listPost.OrderByDescending(post => post.DataPost));
            }
            catch {
                return View("Error");
            }

        }

        // GET: Publicacao/Post
        public ActionResult CreatePost() {
            return View();
        }

        // POST: Publicacao/CreatePost
        [HttpPost]
        public ActionResult CreatePost(FormCollection collection, Post post, HttpPostedFileBase file) {
            try {
                Usuario teste = (Usuario)Session["object"];
                post.IdUsuario = teste.Id;
                PostsRepository.AddPost(post, file);
                return RedirectToAction("Welcome","Usuario");
            }
            catch {
                return View("Error");
            }
        }

        // GET: Publicacao/Edit/5
        public ActionResult EditPost(int id) {
            Post post=PostsRepository.SelectPost(id);
            return View(post);
        }

        // POST: Publicacao/Edit/5
        [HttpPost]
        public ActionResult EditPost(Post post, FormCollection collection, HttpPostedFileBase file)
        {
            try
            {
                // TODO: Add update logic here
                if (file != null) {
                    PostsRepository.EditPost(post, file);
                }
                else {
                    PostsRepository.EditPostSemMudarImagem(post);
                }
                Usuario sessao = (Usuario)Session["object"];
                Session["object"] = UsuariosRepository.SelectUsuario(sessao.Id);
                return RedirectToAction("ListPosts", "Post");
            }
            catch
            {
                return View("Error");
            }
        }

        public ActionResult CompartilharPost(int id)
        {
            try
            {
                Usuario sessao = (Usuario)Session["object"];

                Post post = PostsRepository.SelectPost(id);
                post.IdUsuario = sessao.Id;
                post.IdPostOrigem = id;
                PostsRepository.CompartilharPost(post);

                Session["object"] = UsuariosRepository.SelectUsuario(sessao.Id);
                TempData["alert"] = "Post Compartilhado";
                return RedirectToAction("Welcome", "Usuario");
            }
            catch
            {
                return View("Error");
            }
        }

        public ActionResult VerPostOriginal(int id)
        {
            try
            {
                Usuario sessao = (Usuario)Session["object"];
                Post post = PostsRepository.SelectPost(id);
                if (post.IdPost > 0)
                {
                    Session["object"] = UsuariosRepository.SelectUsuario(sessao.Id);
                    return View(post);
                }
                else
                {
                    Session["object"] = UsuariosRepository.SelectUsuario(sessao.Id);
                    return View("PostJaDeletado");
                }
            }
            catch
            {
                return View("Error");
            }
        }
        // GET: Publicacao/Delete/5
        public ActionResult DeletePost(int id)
        {
            try {
                CurtidasPostsRepository.DeleteAllCurtidasPost(PostsRepository.SelectPost(id));
                ComentariosRepository.DeleteAllComentariosDoPost(PostsRepository.SelectPost(id));
                PostsRepository.DeletePost(id);
                Usuario sessao = (Usuario)Session["object"];
                Session["object"] = UsuariosRepository.SelectUsuario(sessao.Id);
                TempData["alert"] = "Post Deletado";
                return RedirectToAction("ListPosts");
                //return RedirectToAction("ListPosts", "Post");
            }
            catch {
                return View("Error");
            }
        }

        public ActionResult LikePost(int idp, bool kek) {
            try {
                var resultado = PostsRepository.SelectPost(idp);
                Usuario teste = (Usuario)Session["object"];

                if (CurtidasPostsRepository.ChecarCurtidaPost(idp,teste.Id) == false) {
                    PostsRepository.LikePost(resultado);
                    CurtidasPostsRepository.AddCurtidaPostBanco(idp,teste.Id);
                }
                else {
                    PostsRepository.DislikePost(resultado);
                    CurtidasPostsRepository.DeleteCurtidaPostBanco(idp, teste.Id);
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
