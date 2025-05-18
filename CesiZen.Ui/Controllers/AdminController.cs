using CesiZen.Data.Models;
using CesiZen.Ui.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly CesiZenDbContext _context;
    private readonly UserManager<UserModel> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public AdminController(
        CesiZenDbContext context,
        UserManager<UserModel> userManager,
        RoleManager<IdentityRole<int>> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // === DASHBOARD ===
    public IActionResult Index()
    {
        return View(); // Vue Dashboard avec liens vers Users, Roles, Questionnaires
    }

    // === UTILISATEURS ===
    // Affiche la liste des utilisateurs avec leurs rôles
    public async Task<IActionResult> Users()
    {
        var users = await _userManager.Users.ToListAsync();

        var userWithRolesList = new List<UserWithRolesViewModel>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userWithRolesList.Add(new UserWithRolesViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles.ToList()
            });
        }

        // Récupérer tous les rôles disponibles pour le dropdown
        var allRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
        ViewBag.AllRoles = allRoles;

        return View(userWithRolesList);
    }


    // Change le rôle d'un utilisateur
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeUserRole(int userId, string newRole)
    {
        if (string.IsNullOrWhiteSpace(newRole))
        {
            ModelState.AddModelError("", "Le rôle doit être sélectionné.");
            return RedirectToAction(nameof(Users));
        }

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return NotFound();

        var currentRoles = await _userManager.GetRolesAsync(user);

        // Retirer tous les rôles actuels
        var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
        if (!removeResult.Succeeded)
        {
            // Gérer l'erreur (optionnel)
            ModelState.AddModelError("", "Erreur lors de la suppression des anciens rôles.");
            return RedirectToAction(nameof(Users));
        }

        // Ajouter le nouveau rôle
        var addResult = await _userManager.AddToRoleAsync(user, newRole);
        if (!addResult.Succeeded)
        {
            // Gérer l'erreur (optionnel)
            ModelState.AddModelError("", "Erreur lors de l'ajout du nouveau rôle.");
        }

        return RedirectToAction(nameof(Users));
    }


    public async Task<IActionResult> EditUser(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) return NotFound();
        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(UserModel model)
    {
        var user = await _userManager.FindByIdAsync(model.Id.ToString());
        if (user == null) return NotFound();

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Email = model.Email;

        await _userManager.UpdateAsync(user);
        return RedirectToAction(nameof(Users));
    }

    // === RÔLES ===
    public async Task<IActionResult> Roles()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return View(roles);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        if (!string.IsNullOrWhiteSpace(roleName))
        {
            await _roleManager.CreateAsync(new IdentityRole<int>
            {
                Name = roleName,
                NormalizedName = roleName.ToUpper()
            });
        }
        return RedirectToAction(nameof(Roles));
    }

    // === QUESTIONNAIRES DE STRESS ===
    public async Task<IActionResult> StressEvents()
    {
        var events = await _context.StressEvents
            .Include(e => e.StressQuestions)
            .ToListAsync();
        return View(events);
    }

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
            return RedirectToAction(nameof(StressEvents));
        }
        return View(model);
    }

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
            _context.Update(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(StressEvents));
        }
        return View(model);
    }

    public IActionResult AddQuestion(int eventId)
    {
        ViewBag.EventId = eventId;
        return View(new StressQuestionModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddQuestion(StressQuestionModel question, int eventId)
    {
        if (ModelState.IsValid)
        {
            var evt = await _context.StressEvents
                .Include(e => e.StressQuestions)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (evt == null) return NotFound();

            evt.StressQuestions ??= new List<StressQuestionModel>();
            evt.StressQuestions.Add(question);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(EditEvent), new { id = eventId });
        }

        ViewBag.EventId = eventId;
        return View(question);
    }
}
