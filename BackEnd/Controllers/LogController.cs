using BackEnd.Models;
using BackEnd.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        public async Task<ActionResult<IEnumerable<LogViewModel>>> Get()
        {
            return await _context.Logs.Select(log => new LogViewModel
            {
                Id = log.Id,
                IPAddress = log.IPAddress,
                LogDate = log.LogDate,
                LogMessage = log.LogMessage
            }).ToListAsync();
        }

        // GET api/logs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LogViewModel>> Get(long id)
        {
            var log = await _context.Logs.FindAsync(id);

            if (log == null)
            {
                return NotFound();
            }

            return new LogViewModel
            {
                Id = log.Id,
                IPAddress = log.IPAddress,
                LogDate = log.LogDate,
                LogMessage = log.LogMessage
            };
        }


        // PUT: api/logs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, LogViewModel logViewModel)
        {
            if (id != logViewModel.Id)
            {
                return BadRequest();
            }

            Log log = GetLogById(id);

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
        public async Task<ActionResult<Log>> Post(LogViewModel logViewModel)
        {
            try
            {
                IList<Log> logs = new List<Log>();

                if (logViewModel.files != null && logViewModel.files.Length > 0)
                {
                    foreach (IFormFile source in logViewModel.files)
                    {
                        using (var reader = new StreamReader(source.OpenReadStream()))
                        {
                            while (reader.Peek() >= 0)
                            {
                                string[] content = GetLineContent(reader.ReadLine());

                                logs.Add(new Log
                                {
                                    IPAddress = content[0],
                                    LogDate = DateTime.Parse(content[1]),
                                    LogMessage = content[2]
                                });
                            }
                        }
                    }
                }
                else
                {
                    logs.Add(new Log
                    {
                        IPAddress = logViewModel.IPAddress,
                        LogDate = logViewModel.LogDate,
                        LogMessage = logViewModel.LogMessage
                    });
                }

                _context.Logs.AddRange(logs);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { id = logs[0].Id }, logs[0]);
            }
            catch (Exception)
            {
                return BadRequest("Log file is in invalid format.");
            }
        }

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

        private Log GetLogById(long id)
        {
            return _context.Logs.FirstOrDefault(e => e.Id == id);
        }

        /**
        * Extract the line content into an array
        * This function expects a log file that respects the following pattern:
        * " IP - - [DateTimeGMT] \"Message\" "
        * Example: 
            // string line = "216.239.46.60 - - [04/Jan/2003:14:56:50 +0200] \"GET /~lpis/curriculum/C+Unix/Ergastiria/Week-7/filetype.c.txt HTTP/1.0\"  304 -";
        */
        private string[] GetLineContent(string line)
        {

            string ipAddress = line.Substring(0, line.IndexOf("- -")).Trim();

            string logDate = line.Substring(line.IndexOf("- - ") + 3).Trim();
            logDate = logDate.Substring(1, logDate.IndexOf("]") - 1).Trim();

            string logTime = line.Substring(line.IndexOf(":")).Trim();
            logTime = logTime.Substring(1, logTime.IndexOf("+") - 1).Trim();

            string logMessage = line.Substring(line.IndexOf("]") + 1).Trim();

            return new string[] { ipAddress, $"{logDate} ${logTime}", logMessage };
        }
    }
}