using BackEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : Controller
    {
        private readonly LogContext _context;
        public LogController(LogContext context)
        {
            _context = context;
        }

        // GET: api/logs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Log>>> Get()
        {
            return await _context.Logs.ToListAsync();
        }

        // GET api/logs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Log>> Get(long id)
        {
            var log = await _context.Logs.FindAsync(id);

            if (log == null)
            {
                return NotFound();
            }

            return log;
        }


        // PUT: api/logs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, Log log)
        {
            if (id != log.Id)
            {
                return BadRequest();
            }

            _context.Entry(log).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogExists(id))
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

        // POST: api/logs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Log>> Post(Log log)
        {
            _context.Logs.Add(log);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = log.Id }, log);
        }

        // //////////////////////////////////////////////////////

        // // POST api/logs
        // [HttpPost]
        // public IActionResult Post([FromBody]Log log)
        // {
        //     _context.Logs.Add(log);
        //     _context.SaveChanges();
        //     // return StatusCode(201, log);
        //     return CreatedAtAction(nameof(Get), new { id = log.Id}, log);
        // }

        [HttpPost]
        public async Task<IActionResult> Post(IList<IFormFile> files)
        {
            IList<Log> logs = new List<Log>();

            foreach (IFormFile source in files)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await source.CopyToAsync(memoryStream);

                    // Only upload if file contains less than 2MB
                    if (memoryStream.Length < 2097152) {
                        var log = new Log()
                        {
IPAddress = 
                             = memoryStream.ToArray();
                        }

                    } else {
                        return BadRequest(new {message = "The size of the files must be lesser than 2MB"});
                    }
                }
            }

            _context.Logs.AddRange(logs);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { Id = logs[0].Id }, logs[0]);
        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

        // DELETE: api/logs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var log = await _context.Logs.FindAsync(id);
            if (log == null)
            {
                return NotFound();
            }

            _context.Logs.Remove(log);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LogExists(long id)
        {
            return _context.Logs.Any(e => e.Id == id);
        }
    }
}