using CesiZen.Data.Models;
using CesiZen.Ui.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CesiZen.Ui.Controllers;

public class StressTestController : Controller
{
    // Simule la récupération du questionnaire complet avec toutes ses questions
    private readonly CesiZenDbContext _context;
    public StressTestController(CesiZenDbContext context)
    {
        _context = context;
    }
    private StressEventModel GetQuestionnaire()
    {
        return new StressEventModel
        {
            Id = 1,
            Title = "Questionnaire Holmes et Rahe",
            StressQuestions = new List<StressQuestionModel>
        {
            new StressQuestionModel { Id = 1, Title = "Décès du conjoint", Point = 100 },
            new StressQuestionModel { Id = 2, Title = "Divorce", Point = 73 },
            new StressQuestionModel { Id = 3, Title = "Séparation conjugale", Point = 65 },
            new StressQuestionModel { Id = 4, Title = "Prison", Point = 63 },
            new StressQuestionModel { Id = 5, Title = "Décès d’un membre proche de la famille", Point = 63 },
            new StressQuestionModel { Id = 6, Title = "Blessure ou maladie grave", Point = 53 },
            new StressQuestionModel { Id = 7, Title = "Mariage", Point = 50 },
            new StressQuestionModel { Id = 8, Title = "Perte d’emploi", Point = 47 },
            new StressQuestionModel { Id = 9, Title = "Réconciliation conjugale", Point = 45 },
            new StressQuestionModel { Id = 10, Title = "Retraite", Point = 45 },
            new StressQuestionModel { Id = 11, Title = "Changement important dans la santé d’un membre de la famille", Point = 44 },
            new StressQuestionModel { Id = 12, Title = "Grossesse", Point = 40 },
            new StressQuestionModel { Id = 13, Title = "Difficultés sexuelles", Point = 39 },
            new StressQuestionModel { Id = 14, Title = "Changement dans les habitudes de sommeil", Point = 39 },
            new StressQuestionModel { Id = 15, Title = "Changement dans les habitudes alimentaires", Point = 38 },
            new StressQuestionModel { Id = 16, Title = "Vacances", Point = 38 },
            new StressQuestionModel { Id = 17, Title = "Fête", Point = 36 },
            new StressQuestionModel { Id = 18, Title = "Changement important dans les responsabilités professionnelles", Point = 29 },
            new StressQuestionModel { Id = 19, Title = "Changement dans les conditions de vie", Point = 25 },
            new StressQuestionModel { Id = 20, Title = "Changement dans les habitudes scolaires", Point = 26 },
            new StressQuestionModel { Id = 21, Title = "Changement dans les activités sociales", Point = 18 },
            new StressQuestionModel { Id = 22, Title = "Changement de résidence", Point = 20 },
            new StressQuestionModel { Id = 23, Title = "Changement dans les fréquentations", Point = 15 },
            new StressQuestionModel { Id = 24, Title = "Changement dans les habitudes personnelles", Point = 15 },
            new StressQuestionModel { Id = 25, Title = "Changement dans les activités récréatives", Point = 13 },
            new StressQuestionModel { Id = 26, Title = "Changement dans les habitudes religieuses", Point = 11 },
            new StressQuestionModel { Id = 27, Title = "Changement dans les habitudes de vacances", Point = 13 },
            new StressQuestionModel { Id = 28, Title = "Vacances", Point = 13 },
            new StressQuestionModel { Id = 29, Title = "Petit emprunt important", Point = 11 },
            new StressQuestionModel { Id = 30, Title = "Grand emprunt important", Point = 38 },
            new StressQuestionModel { Id = 31, Title = "Troubles avec la police", Point = 23 },
            new StressQuestionModel { Id = 32, Title = "Changement dans les habitudes de loisirs", Point = 15 },
            new StressQuestionModel { Id = 33, Title = "Changement dans les activités sociales", Point = 18 },
            new StressQuestionModel { Id = 34, Title = "Changement dans la santé d’un membre de la famille", Point = 44 },
            new StressQuestionModel { Id = 35, Title = "Mariage d’un membre de la famille", Point = 26 },
            new StressQuestionModel { Id = 36, Title = "Nouveau membre dans la famille", Point = 39 },
            new StressQuestionModel { Id = 37, Title = "Départ d’un membre de la famille", Point = 29 },
            new StressQuestionModel { Id = 38, Title = "Changement dans les responsabilités familiales", Point = 20 },
            new StressQuestionModel { Id = 39, Title = "Changement dans la situation financière", Point = 38 },
            new StressQuestionModel { Id = 40, Title = "Changement dans les habitudes sociales", Point = 18 },
            new StressQuestionModel { Id = 41, Title = "Changement dans les habitudes de consommation", Point = 15 },
            new StressQuestionModel { Id = 42, Title = "Changement dans la routine quotidienne", Point = 10 },
            new StressQuestionModel { Id = 43, Title = "Changement dans les loisirs", Point = 13 }
        }
        };
    }

    [HttpGet]
    public IActionResult Index()
    {
        var questionnaires = _context.StressEvents
            .Select(q => new SelectListItem
            {
                Value = q.Id.ToString(),
                Text = q.Title
            }).ToList();

        var model = new StressTestViewModel
        {
            AvailableQuestionnaires = questionnaires
        };

        return View(model);
    }


    [HttpPost]
    public IActionResult Index(StressTestViewModel model, string submitType)
    {
        // Recharge la liste des questionnaires pour le select
        model.AvailableQuestionnaires = _context.StressEvents
            .Select(q => new SelectListItem
            {
                Value = q.Id.ToString(),
                Text = q.Title
            }).ToList();

        // Si l'utilisateur vient de sélectionner un questionnaire
        if (submitType == "loadQuestions")
        {
            var questionnaire = _context.StressEvents
                .Include(e => e.StressQuestions)
                .FirstOrDefault(e => e.Id == model.QuestionnaireId);

            if (questionnaire == null)
            {
                ModelState.AddModelError("", "Questionnaire introuvable.");
                return View(model);
            }

            model.QuestionnaireTitle = questionnaire.Title;
            model.Questions = questionnaire.StressQuestions.Select(q => new StressQuestionAnswerViewModel
            {
                Id = q.Id,
                Title = q.Title,
                Point = q.Point,
                IsSelected = false
            }).ToList();

            return View(model);
        }

        // Sinon, calcul du score
        if (model.Questions == null || model.Questions.All(q => !q.IsSelected))
        {
            ModelState.AddModelError("", "Veuillez sélectionner au moins une question.");
            return View(model);
        }
        model.QuestionnaireTitle = _context.StressEvents
            .Where(e => e.Id == model.QuestionnaireId)
            .Select(e => e.Title)
            .FirstOrDefault();
        model.TotalScore = model.Questions.Where(q => q.IsSelected).Sum(q => q.Point);
        model.Description = GetDescriptionFromScore(model.TotalScore);

        var result = new StressTestResultModel
        {
            Title = model.QuestionnaireTitle,
            CreatedAt = DateTime.UtcNow,
            TotalScore = model.TotalScore,
            Description = model.Description,
            CreatedById = User.FindFirstValue(ClaimTypes.NameIdentifier) is string idStr && int.TryParse(idStr, out var id) ? id : null
        };

        _context.StressTestResults.Add(result);
        _context.SaveChanges();

        return View("Result", model);
    }



    private string GetDescriptionFromScore(int score)
    {
        if (score < 150) return "Faible - faible probabilité de maladie liée au stress.";
        if (score < 300) return "Modéré - risque modéré de problèmes de santé.";
        return "Élevé - risque élevé de problèmes de santé.";
    }

    // GET: /StressTest/History
    public async Task<IActionResult> History()
    {
        string? userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdString == null || !int.TryParse(userIdString, out int userId))
        {
            return Unauthorized();
        }

        var results = await _context.StressTestResults
                            .Where(r => r.CreatedById == userId)
                            .OrderByDescending(r => r.CreatedAt)
                            .ToListAsync();

        return View(results);
    }
}
