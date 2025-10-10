using Econorte.WebSite.Models;
using Econorte.WebSite.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Econorte.WebSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Econorte_DevContext _db;
        private readonly IWebHostEnvironment _env;

        public HomeController(
            ILogger<HomeController> logger,
            Econorte_DevContext db,
            IWebHostEnvironment env
            )
        {
            _logger = logger;
            _db = db;
            _env = env;
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Método para conseguir las urls de las Apis que se vayan a consumir
        public Api? FetchAPI(Config config)
        {
            Api? api = new Api();
            if (config.IdApi > 0 && config.IdApi > 0)
            {
                api = _db.APIs
                    .Where(a => a.Id_API == config.IdApi)
                    .Select(a => new Api
                    {
                        Id_API = a.Id_API,
                        Name = a.Name,
                        URL = !_env.IsDevelopment() ? a.URL_Prod : a.URL_Dev,
                        IsGet = a.IsGet,
                        IsPost = a.IsPost,
                        Param = a.IsGet ? (config.Param != null ? config.Param : "") : "",
                        BodyParams = a.IsPost ? config.BodyParams : null,
                    })
                    .FirstOrDefault();
            }

            return api;
        }

        //Método genérico para consumir APIs Get y Post
        [HttpPost]
        public async Task<ActionResult> ConsumeApi([FromBody] Config config)
        {
            try
            {
                //Buscar la api y mapear sus parámetros
                Api? api = FetchAPI(config);

                //Declarar el objeto que se devolverá
                object? result = null;

                //Validar que api no sea nulo
                if (api != null)
                {
                    using var client = new HttpClient();
                    using var request = new HttpRequestMessage(api.IsGet ? HttpMethod.Get : HttpMethod.Post, api.Param != "" ? (api.URL + api.Param) : api.URL);

                    if (api.IsPost && api.BodyParams != null)
                    {
                        var json = JsonSerializer.Serialize(api.BodyParams);
                        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    }

                    using var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();

                    // Leer la respuesta como string
                    var responseString = await response.Content.ReadAsStringAsync();

                    // Deserializar la respuesta a un objeto genérico
                    result = JsonSerializer.Deserialize<object>(responseString);
                }

                // Devuelve el objeto deserializado como JSON
                return Json(result == null ? null : result);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message } );
            }
        }
    }
}
