using CesiZen.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Ui.Controllers
{
    public class InformationController : Controller
    {
        private readonly CesiZenDbContext _context;

        public InformationController(CesiZenDbContext context)
        {
            _context = context;
        }

        // GET: Information
        public async Task<IActionResult> Index()
        {
            return View(await _context.InformationModels.ToListAsync());
        }

        // GET: Information/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var informationModel = await _context.InformationModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (informationModel == null)
            {
                return NotFound();
            }

            return View(informationModel);
        }

        // GET: Information/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Information/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Texte,Date")] InformationModel informationModel)
        {
            if (ModelState.IsValid)
            {
                informationModel.Date = DateTime.Now;
                _context.Add(informationModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(informationModel);
        }

        // GET: Information/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var informationModel = await _context.InformationModels.FindAsync(id);
            if (informationModel == null)
            {
                return NotFound();
            }
            return View(informationModel);
        }

        // POST: Information/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Texte,Date")] InformationModel informationModel)
        {
            if (id != informationModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    informationModel.Date = DateTime.Now;
                    _context.Update(informationModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InformationModelExists(informationModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(informationModel);
        }

        // GET: Information/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var informationModel = await _context.InformationModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (informationModel == null)
            {
                return NotFound();
            }

            return View(informationModel);
        }

        // POST: Information/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var informationModel = await _context.InformationModels.FindAsync(id);
            if (informationModel != null)
            {
                _context.InformationModels.Remove(informationModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InformationModelExists(int id)
        {
            return _context.InformationModels.Any(e => e.Id == id);
        }
    }
}
