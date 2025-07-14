namespace dj_backend.Models {
    public class FoodEntry {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateOnly Date { get; set; }
        public List<string> Ingredients { get; set; }
        public bool GotSick { get; set; }
    }
}
