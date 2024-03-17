using GestionalePizzeria.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
namespace GestionalePizzeria.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.User);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        [Authorize(Roles = "User,Admin")]
        public ActionResult Create()
        {
            ViewBag.Carrello = (List<Product>)Session["Carrello"];
            ViewBag.IdUser = new SelectList(db.Users, "IdUser", "Name");
            return View();
        }

        // POST: Orders/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Create([Bind(Include = "Note")] Order order)
        {
            ModelState.Remove("IdUser,OrarioOrdine,Address,isDelivered,Total");

            if (!ModelState.IsValid)
            {

                order.IdUser = ((User)Session["User"]).IdUser;
                order.OrarioOrdine = System.DateTime.Now;
                order.isDelivered = false;
                order.Address = db.Users.Find(order.IdUser).Address;
                db.Orders.Add(order);

                foreach (var item in (List<Product>)Session["Carrello"])
                {
                    db.OrderDetails.Add(new OrderDetail
                    {
                        IdOrder = order.IdOrder,
                        IdProduct = item.IdProduct
                    });
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdUser = new SelectList(db.Users, "IdUser", "Name", order.IdUser);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdUser = new SelectList(db.Users, "IdUser", "Name", order.IdUser);
            return View(order);
        }

        // POST: Orders/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdOrder,OrarioOrdine,Address,Note,isDelivered,IdUser")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdUser = new SelectList(db.Users, "IdUser", "Name", order.IdUser);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
        //   _     ___  __   __  _  _    ___ 
        //  /_\   / __| \ \ / / | \| |  / __|
        // / _ \  \__ \  \ V /  | .` | | (__ 
        ///_/ \_\ |___/   |_|   |_|\_|  \___|
        [AllowAnonymous]
        public JsonResult addToCart(int id)
        {
            // recupero il prodotto dal database
            Product product = db.Products.Find(id);

            // definisco solo le proprietà che mi interessano
            // passaggio necessario per evitare problemi di conversione in json
            // in quanto il modello Product ha una lista di ProductDetail che
            // a sua volta ha una lista di Product
            Product productToJson = new Product
            {
                IdProduct = product.IdProduct,
                Nome = product.Nome,
                Immagine = product.Immagine,
                Prezzo = product.Prezzo,
                TempoPreparazione = product.TempoPreparazione
            };

            // se il carrello non esiste lo inizializzo come lista di prodotti
            Session["Carrello"] = Session["Carrello"] ?? new List<Product>();

            // aggiungo il prodotto al carrello in sessione
            ((List<Product>)Session["Carrello"]).Add(productToJson);

            // ritorno il carrello in json
            return Json(Session["Carrello"], JsonRequestBehavior.AllowGet);
        }
    }
}
