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
    public class HistoriesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Histories
        // working as intended
        public IQueryable<History> GetHistories(int Att_Id)
        {
            var result = db.Histories.Where((h) => h.Att_Id == Att_Id);

            return result;
        }

        // GET: api/Histories/5
        // api/Attends/5?Att_Id={int}
        //working as intended
        [ResponseType(typeof(History))]
        public async Task<IHttpActionResult> GetHistory(int id, int Att_Id)
        {
            History history = await db.Histories
                .Where((h) => h.Att_Id == Att_Id && h.Hist_Id == id)
                .FirstOrDefaultAsync();
            if (history == null)
            {
                return NotFound();
            }

            return Ok(history);
        }

        // PUT: api/Histories/5
        //working as intended
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutHistory(int id, [FromBody] History history)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if requested history is not in a class the user owns
            if (!IdService.isValidHistoryId(id, db, User))
            {
                return BadRequest("invalid history id for the given user");
            }

            if (id != history.Hist_Id)
            {
                return BadRequest();
            }

            db.Entry(history).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HistoryExists(id))
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

        // POST: api/Histories
        //working as intended
        [ResponseType(typeof(History))]
        public async Task<IHttpActionResult> PostHistory(History history)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Histories.Add(history);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = history.Hist_Id }, history);
        }

        // DELETE: api/Histories/5
        [ResponseType(typeof(History))]
        public async Task<IHttpActionResult> DeleteHistory(int id)
        {
            History history = await db.Histories.FindAsync(id);
            if (history == null)
            {
                return NotFound();
            }

            //if requested history is not in a class the user owns
            if (!IdService.isValidHistoryId(id, db, User))
            {
                return BadRequest("invalid history id for the given user");
            }

            db.Histories.Remove(history);
            await db.SaveChangesAsync();

            return Ok(history);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HistoryExists(int id)
        {
            return db.Histories.Count(e => e.Hist_Id == id) > 0;
        }
    }
}