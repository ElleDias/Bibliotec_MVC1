using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bibliotec.Contexts;
using Bibliotec.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bibliotec_mvc.Controllers
{
    [Route("[controller]")]
    public class UsuarioController : Controller
    {
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(ILogger<UsuarioController> logger)
        {
            _logger = logger;
        }
        // CRIANDO UMA OBJ DA CLASSE CONTEXT:
        Context context = new Context();

        // ESTE METODO ESTA RETORNANDO  A VIEW AO USUARIO/INDEX.CSHTML:
        public IActionResult Index()
        {
            //pegar as iHnformacoes de session que sao necessarias para que aparece os detalhes do meu usuario.
            int id = int.Parse(HttpContext.Session.GetString("UsuarioID")!);
            ViewBag.Admin = HttpContext.Session.GetString("Admin")!;

            //id = 1 
            Usuario usuarioEncontrado = context.Usuario.FirstOrDefault(usuario => usuario.UsuarioID == id)!;

            if (usuarioEncontrado == null)
            {
                return NotFound();
            }
            //buscar o curso que esta cadastrado no meu usuario.
            Curso cursoEncontrado = context.Curso.FirstOrDefault(Curso => Curso.CursoID == usuarioEncontrado.CursoID)!;

            //tabela Usuario -> FK CursoID
            //tabela Curso -> PK CursoID
            //Dev integral -> CursoID = 6
            //Dev integral -> CursoID = 6
            //Hiorhanna -> CursoID = 6
            if (cursoEncontrado == null)
            {
                // Console.WriteLine($"O usuario nao possui curso cadastrado");
                 ViewBag.Curso = "O usuario nao possui curso cadastrado";
            }
            else
            {
                // Console.WriteLine($"O usuario possui o curso XX");
                 ViewBag.Curso = cursoEncontrado.Nome!; 

            }

            ViewBag.Nome = usuarioEncontrado.Nome;
            ViewBag.Email = usuarioEncontrado.Email;
            ViewBag.Contato = usuarioEncontrado.Contato;
            ViewBag.DtNasc = usuarioEncontrado.DtNascimento.ToString("dd/MM/yyyy");

            return View();
        }

        //     [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //     public IActionResult Error()
        //     {
        //         return View("Error!");
        //     }
    }
}