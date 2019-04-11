using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

        //---------- privates ----------

        private List<Models.Process> getAll()
        {
            var badList = System.Diagnostics.Process.GetProcesses();

            List<Models.Process> goodList = new List<Models.Process>();

            foreach(Process p in badList)
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
            var badList = Process.GetProcesses();

            foreach (Process p in badList)
            {
                if (p.ProcessName.Equals("notepad"))
                    p.Kill();
            }
        }
    }
}
