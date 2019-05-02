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
    public class TopicVocabulariesController : Controller
    {
        private readonly EnglishmaniaContext _context;

        public TopicVocabulariesController(EnglishmaniaContext context)
        {
            _context = context;
        }

        // GET: TopicVocabularies
        public async Task<IActionResult> Index()
        {
            return View(await _context.TopicVocabularies.ToListAsync());
        }

        // GET: TopicVocabularies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topicVocabulary = await _context.TopicVocabularies
                .FirstOrDefaultAsync(m => m.TopicId == id);
            if (topicVocabulary == null)
            {
                return NotFound();
            }

            return View(topicVocabulary);
        }

        // GET: TopicVocabularies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TopicVocabularies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TopicId,VocabularyId")] TopicVocabulary topicVocabulary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(topicVocabulary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(topicVocabulary);
        }

        // GET: TopicVocabularies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topicVocabulary = await _context.TopicVocabularies.FindAsync(id);
            if (topicVocabulary == null)
            {
                return NotFound();
            }
            return View(topicVocabulary);
        }

        // POST: TopicVocabularies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TopicId,VocabularyId")] TopicVocabulary topicVocabulary)
        {
            if (id != topicVocabulary.TopicId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(topicVocabulary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TopicVocabularyExists(topicVocabulary.TopicId))
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
            return View(topicVocabulary);
        }

        // GET: TopicVocabularies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topicVocabulary = await _context.TopicVocabularies
                .FirstOrDefaultAsync(m => m.TopicId == id);
            if (topicVocabulary == null)
            {
                return NotFound();
            }

            return View(topicVocabulary);
        }

        // POST: TopicVocabularies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var topicVocabulary = await _context.TopicVocabularies.FindAsync(id);
            _context.TopicVocabularies.Remove(topicVocabulary);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TopicVocabularyExists(int id)
        {
            return _context.TopicVocabularies.Any(e => e.TopicId == id);
        }
    }
}
