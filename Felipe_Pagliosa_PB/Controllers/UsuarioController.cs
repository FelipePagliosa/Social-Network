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
    public class UsuarioController : Controller
    {
        // GET: Tela Inicial
        public ActionResult Index()
        {
            return View();
        }

        // GET: User/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: User/Register
        [HttpPost]
        public ActionResult Register(FormCollection collection,Usuario user,HttpPostedFileBase file)
        {
            try
            {
                // TODO: Add insert logic here
                bool teste=UsuariosRepository.Checar(user);
                if (teste==false) {
                    if(user.Senha==user.ConfirmSenha){
                        UsuariosRepository.AddUser(user, file);
                        Usuario login = UsuariosRepository.Login(user);
                        Session["object"] = login;
                        return RedirectToAction("Welcome");
                    }
                }
                return View();
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: Home Page
        public ActionResult Welcome() {
            Usuario usuario = (Usuario)Session["object"];//recebe a sessão
            ViewBag.Usuario = usuario.NomeUsuario;

            Session["object"] = UsuariosRepository.SelectUsuario(usuario.Id);// atualiza e manda a sessão atualizada para a view 
            Usuario sessao = (Usuario)Session["object"];
            return View(sessao);
        }


        // GET: User/Login/5
        public ActionResult Login()
        {
            return View();
        }

        // POST: User/Login/5
        [HttpPost]
        public ActionResult Login(Usuario usuario, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var login=UsuariosRepository.Login(usuario);
                if (login.Nome!=null) {
                    Session["object"] = login;
                    return RedirectToAction("Welcome");
                }
                else {
                    ViewData["Message"] = "Login falhou, escreva o login e a senha novamente! ";
                    return View();
                }
            }
            catch
            {
                ViewData["Message"] = "Login falhou, erro desconhecido";
                return View();
            }
        }
        // GET: User/Login/5
        public ActionResult Logout() {
            Session.RemoveAll();
            return View("Index");
        }

        // GET: User/Create
        public ActionResult SearchAmigo() {
            var listUsuario = new List<Usuario>();
            return View(listUsuario);
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult SearchAmigo(string pesquisa) {
                if(pesquisa != "") {
                    Usuario usuario = (Usuario)Session["object"];
                    return View(UsuariosRepository.ListAllUsers(usuario,pesquisa));
                }
                else {
                return View("Error");

                }           
        }

        public ActionResult AddAmigo(int id) {
            try {
                Usuario usuario = (Usuario)Session["object"];
                AmizadeRepository.AddAmizade(id, usuario);
                Session["object"] = UsuariosRepository.SelectUsuario(usuario.Id);
                TempData["alert"] = "Convite de amizade enviado";
                return RedirectToAction("SearchAmigo");

            }
            catch {
                return View("Error");
            }
        }

        public ActionResult PerfilUsuario() {
            try {
                Usuario usuario = (Usuario)Session["object"];                
                return View(usuario);

            }
            catch {
                return View("Error");
            }
        }

        // GET: Usuario/Edit/5
        public ActionResult EditPerfil() {
            Usuario usuario = (Usuario)Session["object"];
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        public ActionResult EditPerfil(Usuario usuario, FormCollection collection, HttpPostedFileBase file) {
            try {
                if(file != null) {
                    if (usuario.Senha == usuario.ConfirmSenha)
                    {
                        UsuariosRepository.EditUsuario(usuario, file);
                        Session["object"] = UsuariosRepository.SelectUsuario(usuario.Id);
                        return RedirectToAction("PerfilUsuario");
                    }
                    else
                    {
                        Session["object"] = UsuariosRepository.SelectUsuario(usuario.Id);
                        var user = (Usuario)Session["object"];
                        return View(user);
                    }
                }
                else {
                    if(usuario.Senha == usuario.ConfirmSenha){
                        UsuariosRepository.EditUsuarioSemMudarFoto(usuario);
                        Session["object"] = UsuariosRepository.SelectUsuario(usuario.Id);
                        return RedirectToAction("PerfilUsuario");
                    }
                    else
                    {
                        Session["object"] = UsuariosRepository.SelectUsuario(usuario.Id);
                        var user = (Usuario)Session["object"];
                        return View(user);
                    }
                }
            }
            catch {
                return View("Error");
            }
        }

        // GET: Usuario/DetalhesAmigo
        public ActionResult DetalhesAmigo(int ido, int idd)
        {
            try
            {
                Usuario usuario = (Usuario)Session["object"];
                Session["object"] = UsuariosRepository.SelectUsuario(usuario.Id);
                Usuario amigo = UsuariosRepository.DetalhesAmigo(ido, idd, usuario.Id);
                return View(amigo);

            }
            catch
            {
                return View("Error");
            }
        }
    }
}
