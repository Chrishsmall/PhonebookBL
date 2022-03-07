using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhonebookBL.Data;
using PhonebookBL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;
using System.Linq;
using PhonebookBL.Helpers;

//Chris: I have build this controller from the ground up unlike the PbEntriesController which is generated
namespace PhonebookBL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhonebookController : ControllerBase
    {
        private PbDBContext _dbContext;
        private PbLogDbContext _logDbContext;
        public PhonebookController(PbDBContext pbDBContext, PbLogDbContext pbLogDbContext)
        {
            _dbContext = pbDBContext;
            _logDbContext = pbLogDbContext;
        }
        #region Get
        // GET: api/<PhonebookController>
        [HttpGet]
        public async Task<IActionResult> Get() 
        {
            try
            {
                var Phonebook = await (from phonebook in _dbContext.Phonebook
                                        select new 
                                        { 
                                            Id = phonebook.Id, 
                                            Name = phonebook.Name 
                                        }).ToListAsync();
                return Ok(Phonebook);
            }
            catch (System.Exception ex)
            {
                await LogHelper.CreateLog(ex.InnerException.Message, Severity.Error, _logDbContext, ex.Source);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }
        // GET api/<PhonebookController>/GetDetailByID/5
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetDetailByID (int id)
        {
            try
            {
                List<Phonebook> phonebooks = await _dbContext.Phonebook.Where(x => x.Id == id).Include(x => x.Entries).ToListAsync();
                if (phonebooks == null)
                {
                    return NotFound($"Record not found against the id: {id}");
                }

                return Ok(phonebooks[0]);
            }
            catch (System.Exception ex)
            {
                await LogHelper.CreateLog(ex.InnerException.Message, Severity.Error, _logDbContext, ex.Source);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
        // GET api/<PhonebookController>/GetHeader/5
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetHeader(int id)
        {
            try
            {
                Phonebook phonebook = await _dbContext.Phonebook.FirstAsync(x => x.Id == id);
                if (phonebook == null)
                {
                    return NotFound($"Record not found against the id: {id}");
                }

                return Ok(phonebook);
            }
            catch (System.Exception ex)
            {
                await LogHelper.CreateLog(ex.InnerException.Message, Severity.Error, _logDbContext, ex.Source);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
        // GET api/<PhonebookController>/GetByName/MainList
        [HttpGet("[action]/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                Phonebook phonebook = await _dbContext.Phonebook.FirstAsync(x => x.Name == name);
                if (phonebook == null)
                {
                    return NotFound($"Record not found against the name: {name}");
                }

                return Ok(phonebook);
            }
            catch (System.Exception ex)
            {
                await LogHelper.CreateLog(ex.InnerException.Message, Severity.Error, _logDbContext, ex.Source);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
        #endregion
        #region Post
        // POST api/<PhonebookController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Phonebook phonebook)
        {
            try
            {
                await _dbContext.Phonebook.AddAsync(phonebook);
                await _dbContext.SaveChangesAsync();
                
                return Ok(phonebook);
            }
            catch (System.Exception ex)
            {
                
                await LogHelper.CreateLog(ex.InnerException.Message, Severity.Error, _logDbContext, "Post Phonebook" );
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
            
        }
        #endregion
        #region PUT
        // PUT api/<PhonebookController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Phonebook phonebook)
        {
            try
            {
                Phonebook selPhonebook = await _dbContext.Phonebook.FindAsync(id);//_dbContext.Phonebook.Find(id);
                if (selPhonebook == null)
                {
                    return NotFound($"Phonebook not found against the id: {id}");
                }
                selPhonebook.Name = phonebook.Name;
                await _dbContext.SaveChangesAsync();
                return Ok(selPhonebook);
            }
            catch (System.Exception ex)
            {
                await LogHelper.CreateLog(ex.InnerException.Message, Severity.Error, _logDbContext, ex.Source);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
        #endregion
        #region Delete
        // DELETE api/<PhonebookController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Phonebook selPhonebook = await _dbContext.Phonebook.FindAsync(id);
                if (selPhonebook == null)
                {
                    await LogHelper.CreateLog($"Phonebook not found against the id: {id}", Severity.Error, _logDbContext, "PhonebookController.Delete");
                    return NotFound($"Phonebook not found against the id: {id}");
                }
                _dbContext.Remove(selPhonebook);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (System.Exception ex)
            {
                await LogHelper.CreateLog(ex.InnerException.Message, Severity.Error, _logDbContext, "PhonebookController.Delete");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion
    }
}
