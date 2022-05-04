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
                var fileImage = await _mongoDb.GetAsync(images[i].Uuid);
                images[i].FileImageBase64 = fileImage?.FileContent;
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

            var fileImage = await _mongoDb.GetAsync(image.Uuid);
            image.FileImageBase64 = fileImage?.FileContent;

            return image;
        }

        // PUT: api/Image/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImage(int id, Image image, [FromServices] FileImage fileImage)
        {
            if (id != image.Id)
            {
                return BadRequest();
            }

            var imageDb = _context.Image.Find(id);
            imageDb.Name = image.Name;
            imageDb.FileImageBase64 = image.FileImageBase64;
            imageDb.PersonId = image.PersonId;
            imageDb.Uuid = await DoCreateOrUpdateFileImage(imageDb.Uuid, image.FileImageBase64, fileImage);

            try
            {
                await _context.SaveChangesAsync();
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
            try
            {
                image.Uuid = await DoCreateOrUpdateFileImage(image.Uuid, image.FileImageBase64, fileImage);
                _context.Image.Add(image);
                await _context.SaveChangesAsync();
            }
            catch
            {
                await _mongoDb.RemoveAsync(image.Uuid);
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
            await _mongoDb.RemoveAsync(image.Uuid);

            return NoContent();
        }

        private async Task<string> DoCreateOrUpdateFileImage(string uuid, string contentImage, FileImage fileImage)
        {
            fileImage.Id = uuid;
            fileImage.FileContent = contentImage;
            if (!string.IsNullOrWhiteSpace(contentImage) && !string.IsNullOrWhiteSpace(uuid))
            {
                await _mongoDb.UpdateAsync(uuid, fileImage);
            }
            else if (!string.IsNullOrWhiteSpace(contentImage) && string.IsNullOrWhiteSpace(uuid))
            {
                fileImage.Id = Guid.NewGuid().ToString();;
                await _mongoDb.CreateAsync(fileImage);
            }

            return fileImage.Id;
        }

        private bool ImageExists(int id)
        {
            return _context.Image.Any(e => e.Id == id);
        }
    }
}
