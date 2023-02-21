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
using PracticaBD_Misa;
using EntityState = System.Data.Entity.EntityState;
using System.Web.Http.Cors;

namespace PracticaBD_Misa.Controllers
{
    [EnableCors(origins: "*", headers:"*",methods:"*")]
    public class puertasController : ApiController
    {
        private BDPruebasMisaEntities db = new BDPruebasMisaEntities();

        // GET: api/puertas
        public IQueryable<puertas> Getpuertas()
        {
            return db.puertas;
        }

        // GET: api/puertas/5
        [ResponseType(typeof(puertas))]
        public async Task<IHttpActionResult> Getpuertas(string id)
        {
            puertas puertas = await db.puertas.FindAsync(id);
            if (puertas == null)
            {
                return NotFound();
            }

            return Ok(puertas);
        }

        // PUT: api/puertas/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putpuertas(string id, puertas puertas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != puertas.door_id)
            {
                return BadRequest();
            }

            db.Entry(puertas).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!puertasExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/puertas
        [ResponseType(typeof(puertas))]
        public async Task<IHttpActionResult> Postpuertas(puertas puertas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.puertas.Add(puertas);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (puertasExists(puertas.door_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = puertas.door_id }, puertas);
        }

        // DELETE: api/puertas/5
        [ResponseType(typeof(puertas))]
        public async Task<IHttpActionResult> Deletepuertas(string id)
        {
            puertas puertas = await db.puertas.FindAsync(id);
            if (puertas == null)
            {
                return NotFound();
            }

            db.puertas.Remove(puertas);
            await db.SaveChangesAsync();

            return Ok(puertas);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool puertasExists(string id)
        {
            return db.puertas.Count(e => e.door_id == id) > 0;
        }
    }
}