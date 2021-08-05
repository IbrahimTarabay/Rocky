﻿using Microsoft.AspNetCore.Mvc;
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

        //GET - CREATE
        public IActionResult Create(){   
            return View();
        }

        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj) {
            if (ModelState.IsValid) {//this define if rules you write in category model is applied 
                _db.Category.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //GET - Edit
        public IActionResult Edit(int? id) {
            if (id==null || id==0) {
                return NotFound();
            }
            var obj = _db.Category.Find(id);//Find() work only with primary key
            if (obj==null) {
                return NotFound();
            }

            return View(obj);
        }

        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj){
            if (ModelState.IsValid)
            {//this define if rules you write in category model is applied 
                _db.Category.Update(obj);
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