using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhonebookBL.Data;
using PhonebookBL.Helpers;
using PhonebookBL.Model;


//Chris: I have generated this controller unlike the PhonebookController which I build from the ground up
namespace PhonebookBL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PbEntriesController : ControllerBase
    {
        private PbDBContext _context;
        private PbLogDbContext _logDbContext;

        public PbEntriesController(PbDBContext context, PbLogDbContext pbLogDbContext)
        {
            _context = context;
            _logDbContext = pbLogDbContext;
        }

        // GET: api/PbEntries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PbEntry>>> GetPbEntry()
        {
            return await _context.PbEntry.ToListAsync();
        }

        // GET: api/PbEntries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PbEntry>> GetPbEntry(int id)
        {
            var pbEntry = await _context.PbEntry.FindAsync(id);

            if (pbEntry == null)
            {
                return NotFound();
            }

            return pbEntry;
        }

        // GET: api/PbEntries/GetByPhonebook/5/Susan
        [HttpGet("[action]/{phonebookId}/{namesearch?}")]
        public async Task<ActionResult<IEnumerable<PbEntry>>> GetByPhonebook(int phonebookId, string? namesearch = "")
        {
            List<PbEntry> pbEntries = null;
            if (namesearch == null || namesearch == "")
            {
                pbEntries = await _context.PbEntry.Where(x => x.PhonebookId == phonebookId).ToListAsync();
            }
            else
            {
                pbEntries = await _context.PbEntry.Where(x => x.PhonebookId == phonebookId && x.Name.Contains(namesearch)).ToListAsync();
            }

            if (pbEntries == null)
            {
                return NotFound();
            }

            return pbEntries;
        }

        // GET: api/PbEntries/GetAllByName/Susan
        [HttpGet("[action]/{namesearch}/{exclPhonebookid}")]
        public async Task<ActionResult<IEnumerable<PbEntry>>> GetAllByName(string namesearch,int? exclPhonebookid)
        {
            if (exclPhonebookid == null) { exclPhonebookid = 0;}

            List<PbEntry> pbEntries = null;
            if (namesearch != null || namesearch != "")
            {
                pbEntries = await _context.PbEntry.Where(x => x.Name.Contains(namesearch) && x.PhonebookId != exclPhonebookid).ToListAsync();
            }
            
            if (pbEntries == null)
            {
                return NotFound();
            }

            return pbEntries;
        }
        // PUT: api/PbEntries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPbEntry(int id, PbEntry pbEntry)
        {
            try
            {

                if (id != pbEntry.EntryId)
                {
                    return BadRequest();
                }

                _context.Entry(pbEntry).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PbEntryExists(id))
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
            catch (System.Exception ex)
            {
                await LogHelper.CreateLog(ex.InnerException.Message, Severity.Error, _logDbContext, ex.Source);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: api/PbEntries
        [HttpPost]
        public async Task<ActionResult<PbEntry>> PostPbEntry(PbEntry pbEntry)
        {
            try
            {
                _context.PbEntry.Add(pbEntry);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetPbEntry", new { id = pbEntry.EntryId }, pbEntry);
            }
            catch (System.Exception ex)
            {
                await LogHelper.CreateLog(ex.InnerException.Message, Severity.Error, _logDbContext, "PbEntries.PostPbEntry");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/PbEntries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePbEntry(int id)
        {
            var pbEntry = await _context.PbEntry.FindAsync(id);
            if (pbEntry == null)
            {
                return NotFound();
            }

            _context.PbEntry.Remove(pbEntry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PbEntryExists(int id)
        {
            return _context.PbEntry.Any(e => e.EntryId == id);
        }
    }
}
