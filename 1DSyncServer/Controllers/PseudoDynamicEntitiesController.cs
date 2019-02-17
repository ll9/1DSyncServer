using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _1DSync.Models;
using _1DSyncServer.Data;

namespace _1DSyncServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PseudoDynamicEntitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PseudoDynamicEntitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PseudoDynamicEntities
        [HttpGet]
        public IEnumerable<PseudoDynamicEntity> GetPseudoDynamicEntities()
        {
            return _context.PseudoDynamicEntities;
        }

        // GET: api/PseudoDynamicEntities/synchronize
        [HttpPost("synchronize")]
        public IEnumerable<PseudoDynamicEntity> Synchronize(IEnumerable<PseudoDynamicEntity> entities, [FromQuery(Name = "lastModified")] DateTime? lastModified)
        {
            foreach (var entity in entities)
            {
                entity.LastModified = DateTime.Now;

                if (_context.PseudoDynamicEntities.Any(e => e.Id == entity.Id))
                {
                    _context.Entry(entity).State = EntityState.Modified;
                }
                else
                {
                    _context.PseudoDynamicEntities.Add(entity);
                }
            }
            _context.SaveChanges();

            if (lastModified == null)
            {
                return _context.PseudoDynamicEntities;
            }
            else
            {
                var entriesSinceLastSync = _context.PseudoDynamicEntities.Where(e => e.LastModified > lastModified);
                return _context.PseudoDynamicEntities;
            }
        }

        // GET: api/PseudoDynamicEntities/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPseudoDynamicEntity([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pseudoDynamicEntity = await _context.PseudoDynamicEntities.FindAsync(id);

            if (pseudoDynamicEntity == null)
            {
                return NotFound();
            }

            return Ok(pseudoDynamicEntity);
        }

        // PUT: api/PseudoDynamicEntities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPseudoDynamicEntity([FromRoute] string id, [FromBody] PseudoDynamicEntity pseudoDynamicEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pseudoDynamicEntity.Id)
            {
                return BadRequest();
            }

            _context.Entry(pseudoDynamicEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PseudoDynamicEntityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PseudoDynamicEntities
        [HttpPost]
        public async Task<IActionResult> PostPseudoDynamicEntity([FromBody] PseudoDynamicEntity pseudoDynamicEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.PseudoDynamicEntities.Add(pseudoDynamicEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPseudoDynamicEntity", new { id = pseudoDynamicEntity.Id }, pseudoDynamicEntity);
        }

        // DELETE: api/PseudoDynamicEntities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePseudoDynamicEntity([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pseudoDynamicEntity = await _context.PseudoDynamicEntities.FindAsync(id);
            if (pseudoDynamicEntity == null)
            {
                return NotFound();
            }

            _context.PseudoDynamicEntities.Remove(pseudoDynamicEntity);
            await _context.SaveChangesAsync();

            return Ok(pseudoDynamicEntity);
        }

        private bool PseudoDynamicEntityExists(string id)
        {
            return _context.PseudoDynamicEntities.Any(e => e.Id == id);
        }
    }
}