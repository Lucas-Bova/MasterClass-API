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
    public class ClassroomsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Classrooms
        //working as intended
        public IQueryable<Classroom> GetClassrooms()
        {
            var User_Id = IdService.getUserId(db, User);

            var result = from c in db.Classrooms
                         where c.User_Id == User_Id
                         select c;

            return result;
        }

        // GET: api/Classrooms/5
        //working as intended
        [ResponseType(typeof(Classroom))]
        public async Task<IHttpActionResult> GetClassroom(int id)
        {
            var User_Id = IdService.getUserId(db, User);

            Classroom classroom = await db.Classrooms.Where((c) => c.User_Id == User_Id && c.Cls_Id == id).FirstOrDefaultAsync();

            if (classroom == null)
            {
                return NotFound();
            }

            return Ok(classroom);
        }

        // PUT: api/Classrooms/5
        //working as intended
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutClassroom(int id, [FromBody] Classroom classroom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if the requested room does not belong to the current user
            if (db.Classrooms.AsNoTracking().Where((c) => c.Cls_Id == id).FirstOrDefault().User_Id != IdService.getUserId(db, User))
            {
                return BadRequest("invalid class id for the logged in user");
            }

            if (id != classroom.Cls_Id)
            {
                return BadRequest();
            }

            db.Entry(classroom).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassroomExists(id))
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

        // POST: api/Classrooms
        //working as intended
        [ResponseType(typeof(Classroom))]
        public async Task<IHttpActionResult> PostClassroom(Classroom classroom)
        {
            classroom.User_Id = IdService.getUserId(db, User); //sets the user id for the new classroom

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Classrooms.Add(classroom);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = classroom.Cls_Id }, classroom);
        }

        // DELETE: api/Classrooms/5
        //working as intended
        [ResponseType(typeof(Classroom))]
        public async Task<IHttpActionResult> DeleteClassroom(int id)
        {
            Classroom classroom = await db.Classrooms.FindAsync(id);
            if (classroom == null)
            {
                return NotFound();
            }

            //if the requested room does not belong to the current user
            if (db.Classrooms.AsNoTracking().Where((e) => e.Cls_Id == id).FirstOrDefault().User_Id != IdService.getUserId(db, User))
            {
                return BadRequest("invalid class id for the logged in user");
            }

            db.Classrooms.Remove(classroom);
            await db.SaveChangesAsync();

            return Ok(classroom);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClassroomExists(int id)
        {
            return db.Classrooms.Count(e => e.Cls_Id == id) > 0;
        }
    }
}