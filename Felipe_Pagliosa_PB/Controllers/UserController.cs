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
    public class UserController : Controller
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
                bool teste=UsersRepository.Checar(user);
                if (teste==false) {
                    UsersRepository.AddUser(user, file);
                    Usuario login = UsersRepository.Login(user);
                    Session["object"] = login;
                    return RedirectToAction("Welcome");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Home Page
        public ActionResult Welcome() {
            Usuario usuario = (Usuario)Session["object"];
            ViewBag.Usuario = usuario.NomeUsuario;
            return View(usuario);
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
                var login=UsersRepository.Login(usuario);
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
    }
}
