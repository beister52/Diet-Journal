using dj_backend.Data;
using dj_backend.Models;
using dj_backend.Models.dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace dj_backend.Controllers {

    [Route("api/foodentry")]
    [ApiController]
    public class FoodEntryController : ControllerBase {

        private readonly AppDbContext _db;
        private readonly ILogger<FoodEntryController> _logger;
        public FoodEntryController(AppDbContext db, ILogger<FoodEntryController> logger) {
            _db = db;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<FoodEntryDTO>> GetEntries() {

            _logger.LogInformation("Getting all food entries");
            return Ok(_db.Entries.ToList());

        }

        [HttpGet("{id:int}", Name = "GetEntry")]

        [ProducesResponseType(200, Type = typeof(FoodEntryDTO))] // OK
        [ProducesResponseType(400)] // Bad Request
        [ProducesResponseType(404)] // Not Found
        public ActionResult<FoodEntryDTO> GetEntry(int id) {

            if (id == 0) {
                _logger.LogError("Get Entry failed with Id" + id);
                return BadRequest();
            }

            var entry = _db.Entries.FirstOrDefault(u => u.Id == id);

            if (entry == null) {
                return NotFound();
            }

            return Ok(entry);

        }

        [HttpPost]
        [ProducesResponseType(201)] // OK
        [ProducesResponseType(400)] // Bad Request
        [ProducesResponseType(500)] // Internal Server Error
        public async Task<ActionResult<FoodEntryDTO>> CreateEntry([FromBody] FoodEntryCreateDTO foodEntryCreateDTO) {

            if (foodEntryCreateDTO == null) {
                return BadRequest();
            }

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (_db.Entries.Any(u => u.Title.ToLower() == foodEntryCreateDTO.Title.ToLower())) {
                // Custom validation, added to ModelState
                ModelState.AddModelError("TitleError", "This food entry already exists.");
                return BadRequest(ModelState);
            }

            FoodEntry model = new() {
                Title = foodEntryCreateDTO.Title,
                Date = foodEntryCreateDTO.Date,
                Ingredients = foodEntryCreateDTO.Ingredients,
                GotSick = foodEntryCreateDTO.GotSick
            };

            _db.Entries.Add(model);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("GetEntry", new { id = model.Id }, new FoodEntryDTO {
                Id = model.Id,
                Title = model.Title,
                Date = model.Date,
                Ingredients = model.Ingredients,
                GotSick = model.GotSick
            });
        }

        [HttpDelete("{id:int}", Name = "DeleteEntry")]
        [ProducesResponseType(204)] // No Content
        [ProducesResponseType(400)] // Bad Request
        [ProducesResponseType(404)] // Not Found
        public async Task<IActionResult> DeleteEntry(int id) {

            if (id == 0) {
                return BadRequest();
            }

            var entry = await _db.Entries.FindAsync(id);
            if (entry == null) {
                return NotFound();
            }

            _db.Entries.Remove(entry);
            await _db.SaveChangesAsync();

            return NoContent();

        }

        // Don't need this for dj, implementing for now
        [HttpPut]
        [ProducesResponseType(204)] // No Content
        [ProducesResponseType(400)] // Bad Request
        public async Task<IActionResult> UpdateEntry(int id, [FromBody] FoodEntryUpdateDTO foodEntryDTO) {

            if (foodEntryDTO == null) {
                return BadRequest();
            }

            var entry = await _db.Entries.FindAsync(id);
            if (entry == null) {
                return NotFound();
            }

            if (foodEntryDTO.Title is null)
                ModelState.AddModelError("Title", "Title is required.");

            if (foodEntryDTO.Ingredients is null)
                ModelState.AddModelError("Ingredients", "Ingredients are required.");

            if (foodEntryDTO.GotSick is null)
                ModelState.AddModelError("GotSick", "GotSick is required.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            entry.Title = foodEntryDTO.Title;
            entry.Date = foodEntryDTO.Date.Value;
            entry.Ingredients = foodEntryDTO.Ingredients;
            entry.GotSick = foodEntryDTO.GotSick.Value;

            await _db.SaveChangesAsync();

            return NoContent();

        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(204)] // No Content
        [ProducesResponseType(400)] // Bad Request
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<FoodEntryUpdateDTO> patchDTO) {

            if (patchDTO == null) {
                return BadRequest();
            }

            var entry = await _db.Entries.FindAsync(id);
            if (entry == null) {
                return NotFound();
            }

            var foodEntryDTO = new FoodEntryUpdateDTO() {
                Title = entry.Title,
                Date = entry.Date,
                Ingredients = entry.Ingredients,
                GotSick = entry.GotSick
            };
            patchDTO.ApplyTo(foodEntryDTO, ModelState);

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            entry.Title = foodEntryDTO.Title ?? entry.Title;
            entry.Date = foodEntryDTO.Date ?? entry.Date;
            entry.Ingredients = foodEntryDTO.Ingredients ?? entry.Ingredients;
            entry.GotSick = foodEntryDTO.GotSick ?? entry.GotSick;

            _db.Entries.Update(entry);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }

}
