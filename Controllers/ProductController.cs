using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rocky.Data;
using Rocky.Models;

namespace Rocky.Controllers
{
    public class ProductController : Controller{
        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db) {
            _db = db;
        }
        public IActionResult Index(){
            IEnumerable<Product> objList = _db.Product;

            foreach (var obj in objList) {
                obj.Category = _db.Category.FirstOrDefault(u => u.Id == obj.CategoryId);
                //it's way to load product and related category
            }
            return View(objList);
        }

        //GET - Upsert
        public IActionResult Upsert(int? id){

            //so we are retrieving all of the categories from the database,
            //but we are converting them to a SelectListItem so that we can have enumerable object and can display them in a dropdown
            IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            ViewBag.CategoryDropDown = CategoryDropDown;

            Product product = new Product();
            if (id == null)
            {
                //This is for create
                return View(product);
            }else {
                product = _db.Product.Find(id);
                if (product == null) {
                    return NotFound();
                }
                return View(product);
            }
        }

        //POST - Upsert-->to update or insert product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category obj) {
            if (ModelState.IsValid) {//this define if rules you write in category model is applied 
                _db.Category.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //GET - Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Category.Find(id);//Find() work only with primary key
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //POST - Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(Category obj){
            if (obj==null) {
                return NotFound();
            }
             _db.Category.Remove(obj);
             _db.SaveChanges();
             return RedirectToAction("Index");
        }
    }
}
