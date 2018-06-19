using MySqlASPNetMVC.Application;
using MySqlASPNetMVC.Models;
using System.Web.Mvc;

namespace MySqlASPNetMVC.Controllers
{
    public class HomeController : Controller
    {
        private PersonApplication personApplication;

        public HomeController()
        {
            personApplication = new PersonApplication();
        }

        public ActionResult Index()
        {
            var list = personApplication.ListAll();
            return View ();
        }

        public ActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Insert(Person person)
        {
            if (ModelState.IsValid) 
            {
                personApplication.Save(person);
                return RedirectToAction("Index");
            }
            return View(person);
        }

        public ActionResult Update(int Id)
        {
            var person = personApplication.ListPersonForId(Id);

            if (person == null)
                return HttpNotFound();

            return View(person);
        }

        [HttpPost]
        public ActionResult Update(Person person)
        {
            if (ModelState.IsValid) 
            {
                personApplication.Save(person);
                return RedirectToAction("Index");
            }

            return View(person);
        }

        public ActionResult Details(int Id)
        {
            var person = personApplication.ListPersonForId(Id);

            if (person == null)
                return HttpNotFound();

            return View(person);
        }

        public ActionResult Delete(int Id)
        {
            var person = personApplication.ListPersonForId(Id);

            if (person == null) {
                return HttpNotFound();
            }
            return View(person);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult ConfirDelete(int Id)
        {
            personApplication.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}
