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
    public class DaysController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Days
        //this should not exist, since there is a one to one relationship with attend
        //public IQueryable<Day> GetDays()
        //{

        //    return db.Days;
        //}

        // GET: api/Days/5
        //working as intended
        //we do not need a second value to validate this request since there is a one to one 
        //relationship with attend and they share primary keys
        [ResponseType(typeof(Day))]
        public async Task<IHttpActionResult> GetDay(int id)
        {
            Day day = await db.Days.FindAsync(id);
            if (day == null)
            {
                return NotFound();
            }

            return Ok(day);
        }

        //PATCH: api/Days/5
        //increments the interaction count for the day by 1
        [ResponseType(typeof(Day))]
        public async Task<IHttpActionResult> PatchDay(int id)
        {
            Day day = await db.Days.FindAsync(id);
            if (day == null)
            {
                return NotFound();
            }

            day.Day_Interactions = ++day.Day_Interactions;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DayExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(day);
        }

        // PUT: api/Days/5
        //working as intended
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDay(int id, [FromBody] Day day)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if requested day is not in a class the user owns
            if (!IdService.isValidDayId(id, db, User))
            {
                return BadRequest("invalid day id for the given user");
            }

            db.Entry(day).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DayExists(id))
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

        // POST: api/Days
        // working as intended
        [ResponseType(typeof(Day))]
        public async Task<IHttpActionResult> PostDay(Day day)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Days.Add(day);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DayExists(day.Att_Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = day.Att_Id }, day);
        }

        // DELETE: api/Days/5
        //working as intended
        [ResponseType(typeof(Day))]
        public async Task<IHttpActionResult> DeleteDay(int id)
        {
            Day day = await db.Days.FindAsync(id);
            if (day == null)
            {
                return NotFound();
            }

            //if requested day is not in a class the user owns
            if (!IdService.isValidDayId(id, db, User))
            {
                return BadRequest("invalid day id for the given user");
            }

            db.Days.Remove(day);
            await db.SaveChangesAsync();

            return Ok(day);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DayExists(int id)
        {
            return db.Days.Count(e => e.Att_Id == id) > 0;
        }
    }
}