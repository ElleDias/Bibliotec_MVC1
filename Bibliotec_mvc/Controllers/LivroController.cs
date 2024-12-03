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
    public class LivroController : Controller
    {
        private readonly ILogger<LivroController> _logger;

        public LivroController(ILogger<LivroController> logger)
        {
            _logger = logger;
        }
 [Route("Cadastro")]
        public IActionResult Cadastro()
        {
             ViewBag.Admin = HttpContext.Session.GetString("Admin")!;
             ViewBag.Categorias = context.Categoria.ToList();
            return View();
        }
        Context context = new Context();

        public IActionResult Index()
        {
            ViewBag.Admin = HttpContext.Session.GetString("Admin")!;


            //criar uma lista de livros
            List<Livro> ListaLivros = context.Livro.ToList();


            var LivroReservados = context.LivroReserva.ToDictionary(Livro => Livro.LivroID, livror => livror.DtReserva);

            ViewBag.Livros = ListaLivros;
            ViewBag.LivroComReservas = LivroReservados;

            return View();
        }

        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View("Error!");
        // }
    }
}