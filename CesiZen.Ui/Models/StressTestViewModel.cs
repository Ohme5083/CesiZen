using Microsoft.AspNetCore.Mvc.Rendering;

namespace CesiZen.Ui.Models
{
    public class StressTestViewModel
    {
        public int QuestionnaireId { get; set; }
        public string? QuestionnaireTitle { get; set; }
        public List<StressQuestionAnswerViewModel> Questions { get; set; } = new();
        public int TotalScore { get; set; }
        public string? Description { get; set; }
        public List<SelectListItem> AvailableQuestionnaires { get; set; } = new();
    }
}
