using ContactsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ContactsApp.Repositories;

namespace ContactsApp.Controllers
{
    public class HomeController : Controller
    {
        public readonly IContactRepository _contactRepository;
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(IContactRepository contactRepository, ILogger<HomeController> logger)
        {
            _contactRepository = contactRepository;
            _logger = logger;
        }


        public IActionResult Index()
        {
            //_logger.LogTrace("This is trace log");
            //_logger.LogDebug("THis is debug log");
            //_logger.LogInformation("This is first logging info");
            //_logger.LogWarning("This is warninig log");
            //_logger.LogError("THis is error log");
            //_logger.LogCritical("This is critical log");

            var result = _contactRepository.AllContacts();
            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ContactModel contactToCreate)
        {
            _contactRepository.CreateContact(contactToCreate);
            
            return RedirectToAction("Index");
            
        }

        // GET: Contact/Details/5
        public ActionResult Details(int id)
        {
            var model = _contactRepository.ContactDetails(id);

            return View(model);
        }

        // DET Edit/id
        public IActionResult Edit(int id)
        {
            var model = _contactRepository.ContactDetails(id);
            return View(model);
        }

        // POST: Edit/id
        [HttpPost]
        public IActionResult Edit(int id, ContactModel contactToCreate)
        {
            _contactRepository.Update(id, contactToCreate);

            return RedirectToAction("Index");
            
        }

        // GET: Delete/id
        public IActionResult Delete(int id)
        {
            var model = _contactRepository.ContactDetails(id);

            return View(model);
        }

        // POST: Delete/5
        [HttpPost]
        public IActionResult Delete(int id, ContactModel contactToDelete)
        {
            _contactRepository.DeleteContact(id);
            
            return RedirectToAction("Index");
        }
    }
}
