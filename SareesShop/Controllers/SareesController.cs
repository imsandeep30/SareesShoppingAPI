using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SareesShop.Data;
using SareesShop.Models;
using SareesShop.Services;
namespace SareesShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SareesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly CloudnaryService _cloudinary;
        public SareesController(AppDbContext context, CloudnaryService cloudinary)
        {
            _context = context;
            _cloudinary = cloudinary;
        }

        // ADD SAREE
        [HttpPost("/addSaree")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddSaree(
        [FromForm] string sareeName,
        [FromForm] decimal price,
        [FromForm] IFormFile image)
        {
            var imageUrl = await _cloudinary.UploadImageAsync(image);

            var saree = new Saree
            {
                SareeName = sareeName,
                Price = price,
                ImageUrl = imageUrl
            };

            _context.Sarees.Add(saree);
            await _context.SaveChangesAsync();

            return Ok("Saree added successfully" + imageUrl);
        }

        // SEARCH SAREE
        [HttpGet("/getSaree")]
        public async Task<IActionResult> Search([FromQuery] string? search)
        {
            var query = _context.Sarees.AsQueryable();
            var ans=_context.Sarees.ToList();
            foreach(var item in ans)
            {
                Console.WriteLine(item.SareeName);
            }

            //var query = _context.Sarees.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(s => EF.Functions.ILike(s.SareeName, $"%{search}%")); // case-insensitive for Postgres

            var result = await query.ToListAsync();
            return Ok(result);
        }
    }
}
