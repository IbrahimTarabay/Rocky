using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rocky_DataAccess;
using Rocky_Models;
using Rocky_Models.ViewModels;
using Rocky_Utility;
using Microsoft.AspNetCore.Authorization;

namespace Rocky.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ProductController : Controller{
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment) {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index(){
            IEnumerable<Product> objList = _db.Product.Include(u => u.Category).Include(u=>u.ApplicationType); ;

            /*foreach (var obj in objList) {
                obj.Category = _db.Category.FirstOrDefault(u => u.Id == obj.CategoryId);
                obj.ApplicationType = _db.ApplicationType.FirstOrDefault(u => u.Id == obj.ApplicationTypeId);
            }*/

            return View(objList);
        }

        //GET - Upsert
        public IActionResult Upsert(int? id) {

            //so we are retrieving all of the categories from the database,
            //but we are converting them to a SelectListItem so that we can have enumerable object and can display them in a dropdown

            //IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(i => new SelectListItem
            //{
            //Text = i.Name,
            //Value = i.Id.ToString()
            //});

            //ViewBag.CategoryDropDown = CategoryDropDown;
            //ViewData["CategoryDropDown"] = CategoryDropDown;

            //Product product = new Product();

            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()

                }),
                ApplicationTypeSelectList = _db.ApplicationType.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()

                })
            };

            if (id == null)
            {
                //This is for create
                return View(productVM);
            }else {
                productVM.Product = _db.Product.Find(id);
                if (productVM.Product == null) {
                    return NotFound();
                }
                return View(productVM);
            }
        }

        //POST - Upsert-->to update or insert product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM) {
            if (ModelState.IsValid) {//this define if rules you write in category model is applied 
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productVM.Product.Id == 0)
                {
                    //Creating
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create)) {
                        files[0].CopyTo(fileStream);
                    }//copy file to the new location

                    productVM.Product.Image = fileName + extension;
                    _db.Product.Add(productVM.Product);
                }
                else {
                    //updating
                    var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(u => u.Id == productVM.Product.Id);
                    //we use AsNoTracking() to make EF upate with productVM.Product.Id not Product.Id because they have the same keys and EF track them both.

                    if (files.Count > 0)
                    {//if new file uploaded
                        //Creating
                        string upload = webRootPath + WC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.Image);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        productVM.Product.Image = fileName + extension;
                    }
                    else {
                        productVM.Product.Image = objFromDb.Image;
                    }

                    _db.Product.Update(productVM.Product);
                }

                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            //to reload CategorySelectList if it failed to create product
            productVM.CategorySelectList = _db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            productVM.ApplicationTypeSelectList = _db.ApplicationType.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(productVM);
        }

        //GET - Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //Eager Loading
            Product product = _db.Product.Include(u => u.Category).Include(u => u.ApplicationType).FirstOrDefault(u => u.Id == id);
            //Product product = _db.Product.Find(id);
            //product.Category = _db.Category.Find(product.CategoryId);
            //Find() work only with primary key

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        //POST - Delete
        [HttpPost,ActionName("Delete")]//instead of writing asp-action="DeletePost" in Delete.cshtml
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id){
            var obj = _db.Product.Find(id);

            if (obj==null) {
                return NotFound();
            }

            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;
            var oldFile = Path.Combine(upload, obj.Image);

            if (System.IO.File.Exists(oldFile)) {
                System.IO.File.Delete(oldFile);
            }
             _db.Product.Remove(obj);
             _db.SaveChanges();
             return RedirectToAction("Index");
        }
    }
}
