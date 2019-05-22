using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AIDE_Client.Models;
using Microsoft.AspNetCore.Mvc;

namespace AIDE_Client.Controllers
{
    [Route("api/processes")]
    [ApiController]
    public class ProcessesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Models.Process>> GetAllProcesses()
        {
            try
            {
                var res = getAll();

                return Ok(res);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("important")]
        public ActionResult<IEnumerable<Models.Process>> GetImportantProcesses()
        {
            try
            {
                var res = getImportant();

                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("killNotepad")]
        public ActionResult KillNotepad()
        {
            try
            {
                killNotepad();
                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("exec")]
        public ActionResult ExecuteComand([FromBody] Command command)
        {
            try
            {
                string result = executeCommand(command);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //---------- privates ----------

        private List<Models.Process> getAll()
        {
            var badList = System.Diagnostics.Process.GetProcesses();

            List<Models.Process> goodList = new List<Models.Process>();

            foreach(System.Diagnostics.Process p in badList)
            {
                var name = p.ProcessName;
                //if (name.Equals("update.exe"))
                goodList.Add(new Models.Process
                {
                    ProcessName = name,
                    PID = p.Id,
                    PrivateMemorySize = p.PrivateMemorySize64,
                    RespondingState = p.Responding
                    });
            }

            goodList = goodList.OrderBy(Process => Process.ProcessName).ToList(); ;

            return goodList;
        }

        private List<Models.Process> getImportant()
        {
            var all = getAll();
            List<string> importantNames = new List<string> {
                "update",
                "notepad",
                "devenv"
            };
            List<Models.Process> importantOnes = new List<Models.Process>();

            foreach(Models.Process p in all)
            {
                if (importantNames.Contains(p.ProcessName))
                    importantOnes.Add(p);
            }

            return importantOnes;
        }

        private void killNotepad()
        {
            var badList = System.Diagnostics.Process.GetProcesses();

            foreach (System.Diagnostics.Process p in badList)
            {
                if (p.ProcessName.Equals("notepad"))
                    p.Kill();
            }
        }

        private string executeCommand(Command command)
        {

            System.Diagnostics.Process process = new System.Diagnostics.Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = command.FileName,
                    Arguments = command.Arguments,
                    RedirectStandardOutput = true
                }
            };

            int times, timeBetweenChecks;
            int counter = 0;

            if (command.Times > 0)
                times = command.Times;
            else
                times = 10;

            if (command.TimeBetweenChecks > 0)
                timeBetweenChecks = command.TimeBetweenChecks;
            else
                timeBetweenChecks = 1000;

            process.Start();

            while (!process.HasExited & counter != times)
            {
                process.WaitForExit(timeBetweenChecks);
                counter++;
            }

            string output = "";

            if (process.HasExited)
                while (!process.StandardOutput.EndOfStream)
                    output += $"{process.StandardOutput.ReadLine()}\n";
            else
            {
                process.Kill();
                output = $"Process has been terminated because it was taking too long. Time spent: {(float)(timeBetweenChecks * times)/1000} seconds.";
            }

            process.Dispose();

            return output;
        }
    }
}
