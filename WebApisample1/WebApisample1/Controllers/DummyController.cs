using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApisample1.Entities;  

namespace WebApisample1.Controllers
{
    public class DummyController : Controller
    {
        private CityInfoContext _context;  

        public DummyController(CityInfoContext context)
        {
            _context = context; 
        }


        [HttpGet] 
        [Route("api/testdatabase")]
        public IActionResult TestDatabase()
        {
            return Ok(); 
        }
    }
}
