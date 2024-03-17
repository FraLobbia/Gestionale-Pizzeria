using GestionalePizzeria.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace GestionalePizzeria.Controllers
{
    public class ProductsController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Products
        public ActionResult Index()
        {
            var ingredients = db.Ingredients.ToList();
            ViewBag.Ingredients = new MultiSelectList(ingredients, "IdIngredient", "Name");

            return View(db.Products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            var ingredients = db.Ingredients.ToList();
            ViewBag.Ingredients = new MultiSelectList(ingredients, "IdIngredient", "Name");
            return View(new Product { SelectedIngredientIDs = new int[] { } });
        }

        // POST: Products/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdProduct,Nome,Immagine,Prezzo,TempoPreparazione,SelectedIngredientIDs")] Product product)
        {
            if (ModelState.IsValid)
            {
                // Aggiungi il nuovo prodotto al database e salva le modifiche
                db.Products.Add(product);

                // Itera sugli identificatori degli ingredienti selezionati
                foreach (var id in product.SelectedIngredientIDs ?? new int[] { })
                {

                    // Aggiungo gli ingredienti alla tabella associativa
                    db.ProductDetails.Add(
                        new ProductDetail
                        {
                            IdProduct = product.IdProduct,
                            IdIngredient = id
                        });

                }
                db.SaveChanges();


                // Reindirizza all'azione Index
                return RedirectToAction("Index");
            }

            // Se il modello non è valido, torna alla vista "Create" con il modello
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdProduct,Nome,Immagine,Prezzo,TempoPreparazione")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
    }
}
