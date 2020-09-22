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
    public class AttendsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Attends
        // working as intended
        public IQueryable<Attend> GetAttends(int Classroom_Id)
        {
            var result = db.Attends.Where((a) => a.Cls_Id == Classroom_Id);

            return result;
        }

        // GET: api/Attends/5
        // api/Attends/5?classroom_Id={int}
        // working as intended
        [ResponseType(typeof(Attend))]
        public async Task<IHttpActionResult> GetAttend(int id, int Classroom_Id)
        {
            Attend attend = await db.Attends
                .Where((a) => a.Cls_Id == Classroom_Id && a.Att_Id == id)
                .FirstOrDefaultAsync();
            if (attend == null)
            {
                return NotFound();
            }

            return Ok(attend);
        }

        // PUT: api/Attends/5
        // api/Attends/5?classroom_Id={int}
        // working as intended
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAttend(int id, int classroom_Id, [FromBody] Attend attend)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if requested attend is not in the given class
            if (db.Attends.AsNoTracking().Where((a) => a.Att_Id == id).FirstOrDefault().Cls_Id != classroom_Id)
            {
                return BadRequest("invalid attend id for the given class in user");
            }

            if (id != attend.Att_Id)
            {
                return BadRequest();
            }

            db.Entry(attend).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendExists(id))
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

        // POST: api/Attends
        //working as intended
        [ResponseType(typeof(Attend))]
        public async Task<IHttpActionResult> PostAttend(Attend attend)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Attends.Add(attend);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = attend.Att_Id }, attend);
        }

        // DELETE: api/Attends/5
        //working as intended
        [ResponseType(typeof(Attend))]
        public async Task<IHttpActionResult> DeleteAttend(int id, int classroom_Id)
        {
            Attend attend = await db.Attends.FindAsync(id);
            if (attend == null)
            {
                return NotFound();
            }

            //if requested attend is not in the given class
            if (db.Attends.AsNoTracking().Where((a) => a.Att_Id == id).FirstOrDefault().Cls_Id != classroom_Id)
            {
                return BadRequest("invalid attend id for the given class in user");
            }

            db.Attends.Remove(attend);
            await db.SaveChangesAsync();

            return Ok(attend);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AttendExists(int id)
        {
            return db.Attends.Count(e => e.Att_Id == id) > 0;
        }
    }
}