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
    public class WordVocabulariesController : Controller
    {
        private readonly EnglishmaniaContext _context;

        public WordVocabulariesController(EnglishmaniaContext context)
        {
            _context = context;
        }

        // GET: WordVocabularies
        public async Task<IActionResult> Index()
        {
            return View(await _context.WordVocabularies.ToListAsync());
        }

        // GET: WordVocabularies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wordVocabulary = await _context.WordVocabularies
                .FirstOrDefaultAsync(m => m.WordId == id);
            if (wordVocabulary == null)
            {
                return NotFound();
            }

            return View(wordVocabulary);
        }

        // GET: WordVocabularies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WordVocabularies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WordId,VocabularyId")] WordVocabulary wordVocabulary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wordVocabulary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wordVocabulary);
        }

        // GET: WordVocabularies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wordVocabulary = await _context.WordVocabularies.FindAsync(id);
            if (wordVocabulary == null)
            {
                return NotFound();
            }
            return View(wordVocabulary);
        }

        // POST: WordVocabularies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WordId,VocabularyId")] WordVocabulary wordVocabulary)
        {
            if (id != wordVocabulary.WordId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wordVocabulary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WordVocabularyExists(wordVocabulary.WordId))
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
            return View(wordVocabulary);
        }

        // GET: WordVocabularies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wordVocabulary = await _context.WordVocabularies
                .FirstOrDefaultAsync(m => m.WordId == id);
            if (wordVocabulary == null)
            {
                return NotFound();
            }

            return View(wordVocabulary);
        }

        // POST: WordVocabularies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wordVocabulary = await _context.WordVocabularies.FindAsync(id);
            _context.WordVocabularies.Remove(wordVocabulary);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WordVocabularyExists(int id)
        {
            return _context.WordVocabularies.Any(e => e.WordId == id);
        }
    }
}
