namespace dj_backend.Models.dto {
    public class FoodEntryUpdateDTO {
        public string? Title { get; set; }
        public DateOnly? Date { get; set; }
        public List<string>? Ingredients { get; set; }
        public bool? GotSick { get; set; }
    }
}
