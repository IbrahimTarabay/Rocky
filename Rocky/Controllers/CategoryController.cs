using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rocky_DataAccess;
using Rocky_Models;
using Rocky_Utility;
using Microsoft.AspNetCore.Authorization;
using Rocky_DataAccess.Repository.IRepository;


namespace Rocky.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class CategoryController : Controller{
        private readonly ICategoryRepository _catRepo;
        public CategoryController(ICategoryRepository catRepo) {
            _catRepo = catRepo;
        }
        public IActionResult Index(){
            IEnumerable<Category> objList = _catRepo.GetAll();
            return View(objList);
        }

        //GET - CREATE
        public IActionResult Create(){   
            return View();
        }

        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj) {
            if (ModelState.IsValid) {//this define if rules you write in category model is applied 
                _catRepo.Add(obj);
                _catRepo.Save();
                TempData[WC.Success] = "Category Created Successfully";
                return RedirectToAction("Index");
            }
            TempData[WC.Error] = "Error while creating category";
            return View(obj);
        }

        //GET - Edit
        public IActionResult Edit(int? id) {
            if (id==null || id==0) {
                return NotFound();
            }
            var obj = _catRepo.Find(id.GetValueOrDefault());//Find() work only with primary key
            if (obj==null) {
                return NotFound();
            }

            return View(obj);
        }

        //POST - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj){
            if (ModelState.IsValid)
            {//this define if rules you write in category model is applied 
                _catRepo.Update(obj);
                _catRepo.Save();
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
            var obj = _catRepo.Find(id.GetValueOrDefault());//Find() work only with primary key
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
             _catRepo.Remove(obj);
             _catRepo.Save();
             return RedirectToAction("Index");
        }
    }
}
