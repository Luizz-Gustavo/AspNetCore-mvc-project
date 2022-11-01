using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjetoMVC.Models;
using ProjetoMVC.context;

namespace ProjetoMVC.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UsuariosContext _context;
        public UsuariosController(UsuariosContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var usuarios = _context.Usuarios.ToList();
            return View(usuarios);
        }

        [HttpGet]
        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Criar(Usuarios usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [HttpPost]
        public IActionResult Editar(Usuarios usuario)
        {
            var usuarioBanco = _context.Usuarios.Find(usuario.Id);

            usuarioBanco.Nome = usuario.Nome;
            usuarioBanco.Email = usuario.Email;
            usuarioBanco.Telefone = usuario.Telefone;
            usuarioBanco.Senha = usuario.Senha;

            _context.Usuarios.Update(usuarioBanco);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Deletar(int id)
        {
            var usuario = _context.Usuarios.Find(id);

            if (usuario == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }

        [HttpPost]
        public IActionResult Deletar(Usuarios usuarios)
        {
            var usuarioBanco = _context.Usuarios.Find(usuarios.Id);

            _context.Usuarios.Remove(usuarioBanco);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}