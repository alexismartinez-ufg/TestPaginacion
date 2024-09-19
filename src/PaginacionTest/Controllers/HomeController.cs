using LlenadoDataFake;
using Microsoft.AspNetCore.Mvc;
using PaginacionTest.dbcontext;
using PaginacionTest.Models;
using System.Data.SqlClient;
using System.Diagnostics;

namespace PaginacionTest.Controllers
{
    public class HomeController(PruebaDbContext _dbContext, ILogger<HomeController> _logger) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PaginacionBasica()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ObtenerClientes()
        {
            var clientes = _dbContext.Clientes.ToList(); // Carga toda la data
            return Json(clientes); // Devuelve en formato JSON
        }


        public IActionResult PaginacionOptimizada(int pagina = 1, int tamanoPagina = 10)
        {
            var stopwatch = Stopwatch.StartNew();

            // Total de registros
            var totalRegistros = _dbContext.Clientes.Count();

            // Total de páginas
            var totalPaginas = (int)Math.Ceiling(totalRegistros / (double)tamanoPagina);

            // Clientes paginados
            var clientesPaginados = _dbContext.Clientes
                .OrderBy(c => c.ClienteId)
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToList();

            stopwatch.Stop();
            ViewBag.TiempoCarga = stopwatch.ElapsedMilliseconds;
            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;

            return View(clientesPaginados);
        }


        public IActionResult PaginacionSqlDirecta(int pagina = 1, int tamanoPagina = 10)
        {
            var stopwatch = Stopwatch.StartNew();
            var totalRegistros = 0;

            var clientesPaginados = new List<Cliente>();
            using (var connection = new SqlConnection("Server=localhost;Database=paginacion;integrated security=true;TrustServerCertificate=true;"))
            {
                connection.Open();

                // Obtener el total de registros
                var countCommand = new SqlCommand("SELECT COUNT(*) FROM Clientes", connection);
                totalRegistros = (int)countCommand.ExecuteScalar();

                // Total de páginas
                var totalPaginas = (int)Math.Ceiling(totalRegistros / (double)tamanoPagina);

                // Obtener clientes paginados
                var command = new SqlCommand("SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY ClienteId) AS RowNum, * FROM Clientes) AS RowConstrainedResult WHERE RowNum >= @StartRow AND RowNum <= @EndRow", connection);
                command.Parameters.AddWithValue("@StartRow", (pagina - 1) * tamanoPagina + 1);
                command.Parameters.AddWithValue("@EndRow", pagina * tamanoPagina);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var cliente = new Cliente
                        {
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString(),
                            Direccion = reader["Direccion"].ToString(),
                            Email = reader["Email"].ToString(),
                            FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]),
                            FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]),
                            Empresa = reader["Empresa"].ToString()
                        };
                        clientesPaginados.Add(cliente);
                    }
                }

                stopwatch.Stop();
                ViewBag.TiempoCarga = stopwatch.ElapsedMilliseconds;
                ViewBag.PaginaActual = pagina;
                ViewBag.TotalPaginas = totalPaginas;
            }

            return View(clientesPaginados);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
