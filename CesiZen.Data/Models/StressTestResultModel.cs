namespace CesiZen.Data.Models
{
    public class StressTestResultModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TotalScore { get; set; }
        public string? Description { get; set; }
        public int? CreatedById { get; set; }
        public UserModel? CreatedBy { get; set; }
    }

}
