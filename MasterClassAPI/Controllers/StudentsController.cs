using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MasterClassAPI.Models;
using MasterClassAPI.Services;

namespace MasterClassAPI.Controllers
{
    [Authorize]
    public class StudentsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Students
        //working as intended
        public IQueryable<Student> GetStudents()
        {
            var User_Id = IdService.getUserId(db, User);

            var result = db.Students.Where((s) => s.User_Id == User_Id);

            return result;
        }

        // GET: api/Students/5
        //working as intended
        [ResponseType(typeof(Student))]
        public async Task<IHttpActionResult> GetStudent(int id)
        {
            var User_Id = IdService.getUserId(db, User);

            Student student = await db.Students.Where((s) => s.User_Id == User_Id && s.Stu_Id == id).FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        // PUT: api/Students/5
        // working as intended
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStudent(int id, [FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if the requested student does not belong to the current user
            if (db.Students.AsNoTracking().Where((s) => s.Stu_Id == id).FirstOrDefault().User_Id != IdService.getUserId(db, User))
            {
                return BadRequest("invalid student id for the logged in user");
            }

            if (id != student.Stu_Id)
            {
                return BadRequest();
            }

            db.Entry(student).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/Students
        //working as intended
        [ResponseType(typeof(Student))]
        public async Task<IHttpActionResult> PostStudent(Student student)
        {
            student.User_Id = IdService.getUserId(db, User);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Students.Add(student);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = student.Stu_Id }, student);
        }

        // DELETE: api/Students/5
        //working as intended
        [ResponseType(typeof(Student))]
        public async Task<IHttpActionResult> DeleteStudent(int id)
        {
            Student student = await db.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            //if the requested student does not belong to the current user
            if (db.Students.AsNoTracking().Where((e) => e.Stu_Id == id).FirstOrDefault().User_Id != IdService.getUserId(db, User))
            {
                return BadRequest("invalid student id for the logged in user");
            }

            db.Students.Remove(student);
            await db.SaveChangesAsync();

            return Ok(student);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudentExists(int id)
        {
            return db.Students.Count(e => e.Stu_Id == id) > 0;
        }
    }
}