using Data.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Model;
using SunRise.Models;

namespace SunRise.Controllers
{
    public class CityController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly ICityRepository _city;

        public CityController(ILogger<HomeController> logger, ICityRepository city)
        {
            _logger = logger;
            _city = city;
        }
        // GET: CityController
        public ActionResult Index()
        {
            return View(_city.GetAll(orderBy: r=>r.OrderBy(c=>c.Name)));
        }

      // GET: CityController/Create
        public ActionResult Create()
        {
            CityDtoModel model = new ();
            return View(model);
        }

        // POST: CityController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CityDtoModel model)
        {
            if (ModelState.IsValid)
            {
                City city = new()
                {
                    Name = model.Name,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude
                };
                if (_city.Add(city) && _city.Save())
                {
                    return RedirectToAction(nameof(Index));
                }
                _logger.LogError("Не удалось добавить город " + model.Name);
            }
            return View(model);
        }

        // GET: CityController/Edit/5
        public ActionResult Edit(int id)
        {
            City city = _city.FirstOrDefault(r => r.Id == id);
            if (city != null)
            {
                CityDtoModel model = new ()
                {
                    Id = city.Id,
                    Name = city.Name,
                    Latitude = city.Latitude,
                    Longitude = city.Longitude
                };
                return View(model);
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: CityController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CityDtoModel model)
        {
            if (ModelState.IsValid)
            {
                City city = _city.FirstOrDefault(r => r.Id == model.Id);
                city.Name = model.Name;
                city.Latitude = model.Latitude;
                city.Longitude= model.Longitude;
                if (_city.Update(city) && _city.Save())
                {
                    return RedirectToAction(nameof(Index));
                }
                _logger.LogError("Не удалось обновить данные города" + city.Name);
            }
            return View(model);
        }

        // GET: CityController/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            City city = _city.FirstOrDefault(r => r.Id == id);
            if (city != null)
            {
                if (_city.Remove(city) && _city.Save())
                {
                    return RedirectToAction(nameof(Index));
                }
                _logger.LogError("Не удалось удалить данные о городе " + city.Name);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
