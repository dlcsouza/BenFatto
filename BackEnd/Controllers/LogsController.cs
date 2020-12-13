using BackEnd.Models;
using BackEnd.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly LogContext _context;
        public LogsController(LogContext context)
        {
            _context = context;
        }

        // GET: api/logs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LogViewModel>>> GetAllLogs()
        {
            return await _context.Logs.Select(log => new LogViewModel
            {
                Id = log.Id,
                IPAddress = log.IPAddress,
                LogDate = log.LogDate,
                LogMessage = log.LogMessage
            }).ToListAsync();
        }

        // GET: api/logs
        [HttpGet("{ipAddress}/{initialLogDate}/{endLogDate}")]
        public ActionResult<IEnumerable<LogViewModel>> SearchLog(string ipAddress, string initialLogDate, string endLogDate)
        {
            List<LogViewModel> logs = _context.Logs.Select(l => 
                new LogViewModel
                {
                    IPAddress = l.IPAddress,
                    LogDate = l.LogDate,
                    LogMessage = l.LogMessage
                }
            ).ToList();

            DateTime initDate;
            DateTime.TryParse(initialLogDate, out initDate);

            DateTime endDate;
            DateTime.TryParse(endLogDate, out endDate);

            DateTime defaultValue = new DateTime();

            bool isSetInitDate = initDate != defaultValue;
            bool isSetEndDate = endDate != defaultValue;

            if (DateTime.Compare(initDate, endDate) > 0) {
                endDate = initDate;
            }

            if (!string.IsNullOrEmpty(ipAddress))
            {
                logs = logs.Where(l => l.IPAddress.Contains(ipAddress)).ToList();
            }

            if (isSetInitDate || isSetEndDate)
            {
                if (isSetInitDate && isSetEndDate) {
                    logs = logs.Where(l => l.LogDate >= initDate && l.LogDate <= endDate).ToList();

                } else if (isSetInitDate) {
                    logs = logs.Where(l => l.LogDate >= initDate).ToList();

                } else {
                    logs = logs.Where(l => l.LogDate <= endDate).ToList();
                } 
            }
                
            return logs;
        }

        // GET api/logs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LogViewModel>> GetLogById(long id)
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
        public async Task<IActionResult> PutLog(long id, LogViewModel logViewModel)
        {
            if (id != logViewModel.Id)
            {
                return BadRequest();
            }

            var log = await _context.Logs.FindAsync(id);
            if (log == null)
            {
                return NotFound();
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
        public async Task<ActionResult<Log>> PostLog(LogViewModel logViewModel)
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

                return CreatedAtAction(nameof(GetLogById), new { id = logs[0].Id }, logs[0]);
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