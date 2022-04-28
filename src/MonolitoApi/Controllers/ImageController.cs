#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonolitoApi;
using MonolitoApi.Data;
using MonolitoApi.Models;

namespace MonolitoApi.Controllers
{
    [Route("api_v1/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly MonolitoDbContext _context;
        private readonly ImageData _mongoDb;

        public ImageController(MonolitoDbContext context, ImageData mongoDb)
        {
            _context = context;
            _mongoDb = mongoDb;
        }

        // GET: api/Image
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Image>>> GetImage()
        {
            var images = await _context.Image.ToListAsync();
            for (var i = 0; i < images.Count(); i++)
            {
                var fileImage = await _mongoDb.GetAsync(images[i].UUID);
                images[i].fileImageBase64 = fileImage?.fileContent;
            }
            return images;
        }

        // GET: api/Image/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Image>> GetImage(int id)
        {
            var image = await _context.Image.FindAsync(id);

            if (image == null)
            {
                return NotFound();
            }

            var fileImage = await _mongoDb.GetAsync(image.UUID);
            image.fileImageBase64 = fileImage?.fileContent;

            return image;
        }

        // PUT: api/Image/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImage(int id, Image image)
        {
            var imageDb = await GetImage(id);
            // var imageEntry = new Image() {
            //     Name = image.Name, 
            //     PersonId = image.PersonId, 
            //     fileImageBase64 = image.fileImageBase64
            //     };

            if (id != image.Id)
            {
                return BadRequest();
            }

            // var entry = _context.Entry(imageEntry);
            var entry = _context.Entry(image);
            entry.CurrentValues.SetValues(image.UUID);
            entry.State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                if (!string.IsNullOrWhiteSpace(image.fileImageBase64))
                {
                    var fileImage = new FileImage() {fileContent = image.fileImageBase64};
                    await _mongoDb.UpdateAsync(imageDb.Value.UUID, fileImage);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(id))
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

        // POST: api/Image
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Image>> PostImage(Image image, [FromServices] FileImage fileImage)
        {
            var guid = Guid.NewGuid().ToString();
            try
            {
                fileImage.Id = guid;
                fileImage.fileContent = image.fileImageBase64;
                await _mongoDb.CreateAsync(fileImage);
                image.UUID = guid;
                _context.Image.Add(image);
                await _context.SaveChangesAsync();
            }
            catch
            {
                await _mongoDb.RemoveAsync(guid);
                return BadRequest();
            }

            return CreatedAtAction("GetImage", new { id = image.Id }, image);
        }

        // DELETE: api/Image/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _context.Image.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            _context.Image.Remove(image);
            await _context.SaveChangesAsync();
            await _mongoDb.RemoveAsync(image.UUID);

            return NoContent();
        }

        private bool ImageExists(int id)
        {
            return _context.Image.Any(e => e.Id == id);
        }
    }
}
