namespace CesiZen.Data.Models
{
    public class StressQuestionModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int Point { get; set; }
        public int StressEventModelId { get; set; }
        public StressEventModel? StressEvent { get; set; }
    }
}