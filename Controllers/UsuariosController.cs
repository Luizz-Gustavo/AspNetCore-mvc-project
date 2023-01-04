using Microsoft.AspNetCore.Mvc;
using ProjetoMVC.Models;
using ProjetoMVC.context;
using System.Text.RegularExpressions;

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

        //As soon as it validates the fields, the method creates a new user in the database.
        [HttpPost]
        public async Task<IActionResult> Criar(Usuarios usuario)
        {
            if (string.IsNullOrEmpty(usuario.Nome) || string.IsNullOrEmpty(usuario.Email) || string.IsNullOrEmpty(usuario.Telefone) ||
                string.IsNullOrEmpty(usuario.Senha))
            {
                throw new ArgumentException("Nome, Email, Telefone e Senha são campos obrigatórios, verifique se não deixou algum campo vazio.");
            }
            else if (!Regex.IsMatch(usuario.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                throw new ArgumentException("Email", "Email está em um formato inválido");
            }

            if (!usuario.Telefone.All(char.IsDigit))
            {
                throw new ArgumentException("Telefone", "O telefone deve conter apenas dígitos.");
            }

            if (usuario.Senha != usuario.ConfirmarSenha)
            {
                throw new ArgumentException("Senhas", "As senhas não são iguais, verifique se digitou corretamente");
            }

            if (ModelState.IsValid)
            {
                await _context.Usuarios.AddAsync(usuario);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        //This method is for when accesses the application URL with the id wants.
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        //This method is for when you want to edit a user's data.
        [HttpPost]
        public async Task<IActionResult> Editar(Usuarios usuario)
        {
            var usuarioBanco = await _context.Usuarios.FindAsync(usuario.Id);

            if (usuario == null)
            {
                return NotFound();
            }

            if (!Regex.IsMatch(usuario.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                throw new ArgumentException("Email", "Email está em formato inválido");
            }

            if (!usuario.Telefone.All(char.IsDigit))
            {
                throw new ArgumentException("Telefone", "O telefone deve conter apenas dígitos");
            }
            
            if (usuario.Senha != usuario.ConfirmarSenha)
            {
                throw new ArgumentException("Senhas", "As senhas não são iguais, verifique se digitou corretamente");
            }

            usuarioBanco.Nome = usuario.Nome;
            usuarioBanco.Email = usuario.Email;
            usuarioBanco.Telefone = usuario.Telefone;
            usuarioBanco.Senha = usuario.Senha;
            usuarioBanco.ConfirmarSenha = usuario.ConfirmarSenha;


            _context.Usuarios.Update(usuarioBanco);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //Search user by id for after delete option.
        [HttpGet]
        public async Task<IActionResult> Deletar(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }

        //This method delete a user in database.
        [HttpPost]
        public async Task<IActionResult> Deletar(Usuarios usuarios)
        {
            var usuarioBanco = await _context.Usuarios.FindAsync(usuarios.Id);

            if (usuarioBanco != null)
            {
                _context.Usuarios.Remove(usuarioBanco);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}