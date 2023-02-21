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
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class usuariosController : ApiController
    {
        private BDPruebasMisaEntities db = new BDPruebasMisaEntities();

        // GET: api/usuarios
        public IQueryable<usuarios> Getusuarios()
        {
            return db.usuarios;
        }

        // GET: api/usuarios/5
        [ResponseType(typeof(usuarios))]
        public async Task<IHttpActionResult> Getusuarios(string id)
        {
            usuarios usuarios = await db.usuarios.FindAsync(id);
            if (usuarios == null)
            {
                return NotFound();
            }

            return Ok(usuarios);
        }

        // PUT: api/usuarios/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putusuarios(string id, usuarios usuarios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuarios.user_id)
            {
                return BadRequest();
            }

            db.Entry(usuarios).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!usuariosExists(id))
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

        // POST: api/usuarios
        [ResponseType(typeof(usuarios))]
        public async Task<IHttpActionResult> Postusuarios(usuarios usuarios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.usuarios.Add(usuarios);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (usuariosExists(usuarios.user_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = usuarios.user_id }, usuarios);
        }

        // DELETE: api/usuarios/5
        [ResponseType(typeof(usuarios))]
        public async Task<IHttpActionResult> Deleteusuarios(string id)
        {
            usuarios usuarios = await db.usuarios.FindAsync(id);
            if (usuarios == null)
            {
                return NotFound();
            }

            db.usuarios.Remove(usuarios);
            await db.SaveChangesAsync();

            return Ok(usuarios);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool usuariosExists(string id)
        {
            return db.usuarios.Count(e => e.user_id == id) > 0;
        }
    }
}