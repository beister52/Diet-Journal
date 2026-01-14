using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dj_backend.Models {
    public class FoodEntry {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
      
        public DateOnly Date { get; set; }
        public List<string> Ingredients { get; set; }
        public bool GotSick { get; set; }
    }
}
