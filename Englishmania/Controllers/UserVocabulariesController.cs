using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Englishmania.DAL.EF;
using Englishmania.DAL.Entities;

namespace Englishmania.Web.Controllers
{
    public class UserVocabulariesController : Controller
    {
        private readonly EnglishmaniaContext _context;

        public UserVocabulariesController(EnglishmaniaContext context)
        {
            _context = context;
        }

        // GET: UserVocabularies
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserVocabularies.ToListAsync());
        }

        // GET: UserVocabularies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userVocabulary = await _context.UserVocabularies
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userVocabulary == null)
            {
                return NotFound();
            }

            return View(userVocabulary);
        }

        // GET: UserVocabularies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserVocabularies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,VocabularyId")] UserVocabulary userVocabulary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userVocabulary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userVocabulary);
        }

        // GET: UserVocabularies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userVocabulary = await _context.UserVocabularies.FindAsync(id);
            if (userVocabulary == null)
            {
                return NotFound();
            }
            return View(userVocabulary);
        }

        // POST: UserVocabularies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,VocabularyId")] UserVocabulary userVocabulary)
        {
            if (id != userVocabulary.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userVocabulary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserVocabularyExists(userVocabulary.UserId))
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
            return View(userVocabulary);
        }

        // GET: UserVocabularies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userVocabulary = await _context.UserVocabularies
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userVocabulary == null)
            {
                return NotFound();
            }

            return View(userVocabulary);
        }

        // POST: UserVocabularies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userVocabulary = await _context.UserVocabularies.FindAsync(id);
            _context.UserVocabularies.Remove(userVocabulary);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserVocabularyExists(int id)
        {
            return _context.UserVocabularies.Any(e => e.UserId == id);
        }
    }
}
