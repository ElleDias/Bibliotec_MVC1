using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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


        //metodo que retorna a tela de cadastro:
        [Route("Cadastro")]
        public IActionResult Cadastro()
        {
            ViewBag.Admin = HttpContext.Session.GetString("Admin")!;
            ViewBag.Categorias = context.Categoria.ToList();
            return View();
        }

        // metodo para cadastrar um livro:
        [Route("Cadastrar")]
        public IActionResult Cadastrar(IFormCollection form)
        {
            Livro novolivro = new Livro();


            novolivro.Nome = form["Nome"].ToString();
            novolivro.Descricao = form["Descricao"].ToString();
            novolivro.Editora = form["Editora"].ToString();
            novolivro.Escritor = form["Escritor"].ToString();
            novolivro.Idioma = form["Idioma"].ToString();

            //trabalhar com imagens:
            if (form.Files.Count > 0)
            {

                var arquivo = form.Files[0];

                var pasta = Path.Combine(Directory.GetCurrentDirectory(), "wwroot/imagens/Livros");

                if (Directory.Exists(pasta))
                {
                    //criar a pasta: 
                    Directory.CreateDirectory(pasta);
                }

                //Terceiro passo
                //criar a variavel para armazenar o caminho em quer meu aruivo estara , alem do nome dele.

                var caminho = Path.Combine(pasta, arquivo.FileName);
                using (var stream = new FileStream(caminho, FileMode.Create))
                {
                    //copiou o arquivo para o meu diretorio
                    arquivo.CopyTo(stream);
                }
                novolivro.Imagem = arquivo.FileName;
            }
            else
            {
                novolivro.Imagem = "padrao.png";
            }
            context.Livro.Add(novolivro);
            context.SaveChanges();

            //segunda parte é adicionar dentro de livrocategoria a cateforia que ´pertence ao nobo livro

            List<LivroCategoria> listalivrocategorias = new List<LivroCategoria>(); //lista as categorias
            string[] categoriasSelecionadas = form["Categoria"].ToString().Split(",");

            foreach (string categoria in categoriasSelecionadas)
            {
                LivroCategoria livroCategoria = new LivroCategoria();
                livroCategoria.CategoriaID = int.Parse(categoria);
                livroCategoria.LivroID = novolivro.LivroID;

            }
            context.LivroCategoria.AddRange(listalivrocategorias);
            context.SaveChanges();
            return LocalRedirect("/Cadastro");
        }

        Context context = new Context();
        private object form;

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