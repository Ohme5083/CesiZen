using CesiZen.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly CesiZenDbContext _context;

    public AdminController(CesiZenDbContext context)
    {
        _context = context;
    }

    // Liste des questionnaires
    public async Task<IActionResult> Index()
    {
        var events = await _context.StressEvents
            .Include(e => e.StressQuestions)
            .ToListAsync();
        return View(events);
    }

    // Créer un nouveau questionnaire
    public IActionResult CreateEvent()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateEvent(StressEventModel model)
    {
        if (ModelState.IsValid)
        {
            _context.StressEvents.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    // Modifier un questionnaire + ses questions
    public async Task<IActionResult> EditEvent(int id)
    {
        var evt = await _context.StressEvents
            .Include(e => e.StressQuestions)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (evt == null) return NotFound();

        return View(evt);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditEvent(StressEventModel model)
    {
        if (ModelState.IsValid)
        {
            // MàJ event
            _context.Update(model);

            // Pour simplifier, tu peux gérer les questions via des actions séparées

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    // Actions pour gérer les questions (Add/Edit/Delete) liées à un événement
    public IActionResult AddQuestion(int eventId)
    {
        var question = new StressQuestionModel { };
        ViewBag.EventId = eventId;
        return View(question);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddQuestion(StressQuestionModel question, int eventId)
    {
        if (ModelState.IsValid)
        {
            var evt = await _context.StressEvents.Include(e => e.StressQuestions)
                            .FirstOrDefaultAsync(e => e.Id == eventId);

            if (evt == null) return NotFound();

            evt.StressQuestions ??= new List<StressQuestionModel>();
            evt.StressQuestions.Add(question);

            await _context.SaveChangesAsync();
            return RedirectToAction("EditEvent", new { id = eventId });
        }
        ViewBag.EventId = eventId;
        return View(question);
    }

    // EditQuestion, DeleteQuestion à prévoir aussi...
}
