using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlockchainDemonstratorApi.Data;
using BlockchainDemonstratorApi.Models.Classes;

namespace BlockchainDemonstratorApi.Controllers
{
    /// <summary>
    /// The admin controller is used to handle back-end admin functionalities, such as creating, getting and logging in as a admin.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly BeerGameContext _context;

        public AdminController(BeerGameContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/Admin/AdminExists
        /// </summary>
        [HttpGet("AdminExists")]
        public async Task<ActionResult<bool>> AdminExists()
        {
            return await _context.Admins.CountAsync() > 0;
        }

        /// <summary>
        /// GET: api/Admin
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admin>>> GetAdmins()
        {
            return await _context.Admins.ToListAsync();
        }

        /// <summary>
        /// GET: api/Admin/5
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Admin>> GetAdmin(string id)
        {
            var admin = await _context.Admins.FindAsync(id);

            if (admin == null)
            {
                return NotFound();
            }

            return admin;
        }

        /// <summary>
        /// PUT: api/Admin/5
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdmin(string id, Admin admin)
        {
            if (id != admin.Id)
            {
                return BadRequest();
            }

            _context.Entry(admin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminExistsFunc(id))
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

        /// <summary>
        /// POST: api/Admin
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Admin>> PostAdmin(Admin admin)
        {
            //if (data.Id.Value == "" || data.Id.Password.Value == "") return BadRequest();
            //admin = new Admin() { };
            Tuple<string, string> hashWithSalt = Cryptography.ComputeHashWithSalt(admin.Password);
            admin.Password = hashWithSalt.Item1;
            admin.Salt = hashWithSalt.Item2;

            _context.Admins.Add(admin);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AdminExistsFunc(admin.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAdmins", new { id = admin.Id }, admin);
        }

        /// <summary>
        /// POST: api/Admin/Create
        /// </summary>
        [HttpPost("Create")]
        public ActionResult Create([FromBody] dynamic data)
        {
            if (data.Id.Value == null || data.Password.Value == null ||
                data.Id.Value == "" || data.Password.Value == "") return BadRequest();
            Admin admin = new Admin() { Id = (string)data.Id, Password = (string)data.Password };
            
            Tuple<string, string> hashWithSalt = Cryptography.ComputeHashWithSalt(admin.Password);
            admin.Password = hashWithSalt.Item1;
            admin.Salt = hashWithSalt.Item2;

            _context.Admins.Add(admin);
            return (_context.SaveChanges() > 0) ? StatusCode(200) : StatusCode(500);
        }


        /// <summary>
        /// DELETE: api/Admin/5
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Admin>> DeleteAdmin(string id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }

            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();

            return admin;
        }

        /// <summary>
        /// POST: api/Admin/AdminExists
        /// </summary>
        [HttpPost("AdminExists")]
        public ActionResult<bool> AdminExists([FromBody] string id)
        {
            return AdminExistsFunc(id);
        }

        private bool AdminExistsFunc(string id)
        {
            return _context.Admins.Any(e => e.Id == id);
        }
    }
}
