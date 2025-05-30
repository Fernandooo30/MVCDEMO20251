using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCDEMO20251.Models;

namespace MVCDEMO20251.Controllers
{
    public class CoursesController : Controller
    {
        private universitydbEntities db = new universitydbEntities();

        // GET: Courses
        public ActionResult Index()
        {
            return View(db.Course.Where(c => c.IsActive).ToList()); // Solo cursos activos
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Course.Find(id);
            if (course == null || !course.IsActive)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,Title,Credits")] Course course)
        {
            if (ModelState.IsValid)
            {
                course.IsActive = true; // Curso activo por defecto
                db.Course.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Course.Find(id);
            if (course == null || !course.IsActive)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseID,Title,Credits")] Course course)
        {
            if (ModelState.IsValid)
            {
                var existingCourse = db.Course.Find(course.CourseID);
                if (existingCourse == null)
                {
                    return HttpNotFound();
                }

                existingCourse.Title = course.Title;
                existingCourse.Credits = course.Credits;
                // No modificamos IsActive aquí

                db.Entry(existingCourse).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Course.Find(id);
            if (course == null || !course.IsActive)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Course.Find(id);
            if (course != null)
            {
                course.IsActive = false; // Eliminación lógica
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Courses/Inactive
        public ActionResult Inactive()
        {
            return View(db.Course.Where(c => !c.IsActive).ToList());
        }

        // GET: Courses/Reactivate/5
        public ActionResult Reactivate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Course course = db.Course.Find(id);
            if (course == null || course.IsActive)
            {
                return HttpNotFound();
            }

            return View(course);
        }

        // POST: Courses/Reactivate/5
        [HttpPost, ActionName("Reactivate")]
        [ValidateAntiForgeryToken]
        public ActionResult ReactivateConfirmed(int id)
        {
            Course course = db.Course.Find(id);
            if (course != null)
            {
                course.IsActive = true;
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
            }
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
