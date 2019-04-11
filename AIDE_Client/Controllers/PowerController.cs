using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AIDE_Client.Controllers
{
    [Route("api/power")]
    [ApiController]
    public class PowerController : ControllerBase
    {
        [HttpGet("lock")]
        public ActionResult Lock()
        {
            try
            {
                lockPc();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("shutdown")]
        public ActionResult Shutdown()
        {
            try
            {
                shutdown();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("restart")]
        public ActionResult Restart()
        {
            try
            {
                restart();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("sleep")]
        public ActionResult Sleep()
        {
            try
            {
                sleep();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //---------- privates ----------

        private void lockPc()
        {
            Process.Start(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation");
        }

        private void shutdown()
        {
            //Process.Start("shutdown", " -s -t 00");
        }

        private void restart()
        {
            //Process.Start("shutdown", " -r -t 00");
        }

        private void sleep()
        {
            Process.Start(@"C:\WINDOWS\system32\rundll32.exe", "powrprof.dll,SetSuspendState 0,1,0");
        }
    }
}