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
    public class WordUsersController : Controller
    {
        private readonly EnglishmaniaContext _context;

        public WordUsersController(EnglishmaniaContext context)
        {
            _context = context;
        }

        // GET: WordUsers
        public async Task<IActionResult> Index()
        {
            return View(await _context.WordUsers.ToListAsync());
        }

        // GET: WordUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wordUser = await _context.WordUsers
                .FirstOrDefaultAsync(m => m.WordId == id);
            if (wordUser == null)
            {
                return NotFound();
            }

            return View(wordUser);
        }

        // GET: WordUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WordUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WordId,UserId,Count,LastUse")] WordUser wordUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wordUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wordUser);
        }

        // GET: WordUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wordUser = await _context.WordUsers.FindAsync(id);
            if (wordUser == null)
            {
                return NotFound();
            }
            return View(wordUser);
        }

        // POST: WordUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WordId,UserId,Count,LastUse")] WordUser wordUser)
        {
            if (id != wordUser.WordId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wordUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WordUserExists(wordUser.WordId))
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
            return View(wordUser);
        }

        // GET: WordUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wordUser = await _context.WordUsers
                .FirstOrDefaultAsync(m => m.WordId == id);
            if (wordUser == null)
            {
                return NotFound();
            }

            return View(wordUser);
        }

        // POST: WordUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wordUser = await _context.WordUsers.FindAsync(id);
            _context.WordUsers.Remove(wordUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WordUserExists(int id)
        {
            return _context.WordUsers.Any(e => e.WordId == id);
        }
    }
}
