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
    public class VocabulariesController : Controller
    {
        private readonly EnglishmaniaContext _context;

        public VocabulariesController(EnglishmaniaContext context)
        {
            _context = context;
        }

        // GET: Vocabularies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Vocabularies.ToListAsync());
        }

        // GET: Vocabularies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vocabulary = await _context.Vocabularies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vocabulary == null)
            {
                return NotFound();
            }

            return View(vocabulary);
        }

        // GET: Vocabularies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vocabularies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,IsPrivate,LevelId")] Vocabulary vocabulary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vocabulary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vocabulary);
        }

        // GET: Vocabularies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vocabulary = await _context.Vocabularies.FindAsync(id);
            if (vocabulary == null)
            {
                return NotFound();
            }
            return View(vocabulary);
        }

        // POST: Vocabularies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IsPrivate,LevelId")] Vocabulary vocabulary)
        {
            if (id != vocabulary.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vocabulary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VocabularyExists(vocabulary.Id))
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
            return View(vocabulary);
        }

        // GET: Vocabularies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vocabulary = await _context.Vocabularies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vocabulary == null)
            {
                return NotFound();
            }

            return View(vocabulary);
        }

        // POST: Vocabularies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vocabulary = await _context.Vocabularies.FindAsync(id);
            _context.Vocabularies.Remove(vocabulary);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VocabularyExists(int id)
        {
            return _context.Vocabularies.Any(e => e.Id == id);
        }
    }
}
