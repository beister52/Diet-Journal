using System.ComponentModel.DataAnnotations;

namespace dj_backend.Models.dto {
    public class FoodEntryCreateDTO {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public DateOnly Date { get; set; }
        [Required]
        public List<string> Ingredients { get; set; }
        [Required]
        public bool GotSick { get; set; }
    }
}
