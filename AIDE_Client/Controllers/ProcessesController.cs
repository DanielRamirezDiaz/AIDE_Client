using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AIDE_Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Models.Process>> GetImportantProcesses()
        {
            try
            {
                var ie = Get();

                return Ok(ie);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        //---------- private ----------

        private List<Models.Process> Get()
        {
            var badList = System.Diagnostics.Process.GetProcesses();

            List<Models.Process> goodList = new List<Models.Process>();

            foreach(System.Diagnostics.Process p in badList)
            {
                var name = p.ProcessName;
                //if (name.Equals("update.exe"))
                    goodList.Add(new Models.Process
                    {
                        Name = name,
                        PID = p.Id,
                        RespondingState = p.Responding,
                        Memory = (int)p.PagedMemorySize64
                    });
            }

            goodList = goodList.OrderBy(Process => Process.Name).ToList(); ;

            return goodList;
        }

    }
}
