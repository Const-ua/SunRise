using Data.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Model;
using SunRise.Models;
using System.Diagnostics;
using System.Text.Json;


namespace SunRise.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICityRepository _city;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, ICityRepository city, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _city = city;
        }


        public IActionResult Index()
        {
            _logger.LogError("Just check log4net logging to file.");
            ViewBag.ErrorMessage = "";
            ViewBag.SunRise = "Восход";
            ViewBag.SunSet = "Закат";
            ViewBag.Both = "Восход/Закат";
            ViewBag.CurrentCity =0;
            return View(_city.GetAll(orderBy: r => r.OrderBy(t => t.Name)));
        }

        public async Task<IActionResult> EventTime(int id, int action) //id- айди города, action - 1-восход, 2-закат, 3-восход/закат
        {
            ViewBag.ErrorMessage = "";
            ViewBag.SunRise = "Восход";
            ViewBag.SunSet = "Закат";
            ViewBag.Both = "Восход/Закат";
            ViewBag.CurrentCity = id;

            City city = _city.FirstOrDefault(r => r.Id == id);
            if (city == null)
            {
                _logger.LogError("Не найден город с id=" + id);
                return RedirectToAction(nameof(Index));
            }

            string apiUrl = _configuration.GetValue<string>("SunSetApi:Url");
            string latitudeKey=_configuration.GetValue<string>("SunSetApi:LatitudeKey");
            string longitudeKey=_configuration.GetValue<string>("SunSetApi:LongitudeKey");
            string url = apiUrl+"?"+latitudeKey+"=" + city.Latitude.Trim() +
                         "&"+longitudeKey+"=" + city.Longitude.Trim();

            if (string.IsNullOrEmpty(apiUrl) || string.IsNullOrEmpty(longitudeKey) || string.IsNullOrEmpty(latitudeKey))
            {
                 ViewBag.ErrorMessage = "Can't read configuration values";
                _logger.LogError((string)ViewBag.ErrorMessage);
                return View("Index",_city.GetAll(orderBy: r => r.OrderBy(t => t.Name)));
            }
      
            HttpClient client = new HttpClient();
            try
            {
                string responseBody = await client.GetStringAsync(url);
                GetResponse response = JsonSerializer.Deserialize<GetResponse>(responseBody);
                if (response.status == "OK")
                {
                    TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("E. Europe Standard Time");
                    DateTime sunRiseLocalTime = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(response.results.sunrise), cstZone);
                    DateTime sunSetLocalTime = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(response.results.sunset), cstZone);                        
                    switch(action)
                    {
                        case AppConstants.SunRise:
                            ViewBag.SunRise += " " + sunRiseLocalTime.ToString("HH:mm");
                            break;
                        case AppConstants.SunSet:
                            ViewBag.SunSet += " " + sunSetLocalTime.ToString("HH:mm");
                            break;
                        case AppConstants.Both:
                            ViewBag.Both += " "+ sunRiseLocalTime.ToString("HH:mm")+" / " +  sunSetLocalTime.ToString("HH:mm");
                            break;
                    }
                }
                else
                {
                    _logger.LogError("Ошибка: сервер вернул статус "+response.status);
                }
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e.Message);
            }
          
            return View("Index",_city.GetAll(orderBy: r => r.OrderBy(t => t.Name)));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}