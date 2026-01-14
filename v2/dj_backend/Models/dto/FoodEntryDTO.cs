using System.ComponentModel.DataAnnotations;

namespace dj_backend.Models.dto {
    public class FoodEntryDTO {
        [Required]
        public int Id { get; set; }
        public string Title { get; set; }
        public DateOnly Date { get; set; }        
        public List<string> Ingredients { get; set; }        
        public bool GotSick { get; set; }

    }
}
