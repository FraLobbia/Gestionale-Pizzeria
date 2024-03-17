
using GestionalePizzeria.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;

namespace GestionalePizzeria.Controllers
{

    public class UsersController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Users
        public ActionResult Index()
        {

            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/SignIn
        [AllowAnonymous]
        public ActionResult SignIn()
        {
            return View();
        }

        // POST: Users/SignIn
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult SignIn([Bind(Include = "IdUser,Name,Surname,Address,Email,Password,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdUser,Name,Surname,Address,Email,Password,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        //      _         ____     _____   _____   _   _ 
        //     | |       / __ \   / ____| |_   _| | \ | |
        //     | |      | |  | | | |  __    | |   |  \| |
        //     | |      | |  | | | | |_ |   | |   | . ` |
        //     | |____  | |__| | | |__| |  _| |_  | |\  |
        //     |______|  \____/   \_____| |_____| |_| \_|

        // GET: Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(User user)
        {
            ModelState.Remove("Name");
            ModelState.Remove("Surname");
            ModelState.Remove("Address");
            ModelState.Remove("Role");

            if (ModelState.IsValid)
            {
                User usr = db.Users.Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefault();
                if (usr != null)
                {
                    FormsAuthentication.SetAuthCookie(usr.Email, false);


                    Session["User"] = usr.IdUser;


                    TempData["Message"] = "Login come " + usr.Role + " effettuato con successo";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Message"] = "Email o password errati";
                    return View(user);
                }
            }
            return View();
        }




        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            TempData["msgSuccess"] = "Logout effettuato con successo";
            return RedirectToAction("Index", "Home");
        }
    }
}
