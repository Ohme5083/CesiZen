namespace CesiZen.Data.Models
{
    public class InformationModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Texte { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public UserModel? ModifiedBy { get; set; }
    }
}
