namespace CesiZen.Data.Models
{
    public class StressEventModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Date { get; set; }
        public List<StressQuestionModel>? StressQuestions { get; set; }
    }

}
