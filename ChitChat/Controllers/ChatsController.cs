using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ChitChat.Data;
using ChitChat.Models;
using Microsoft.AspNetCore.Authorization;

namespace ChitChat.Controllers
{
    public class ChatsController : Controller
    {
        private readonly ChitChatContext _context;

        public ChatsController(ChitChatContext context)
        {
            _context = context;
        }

        // GET: Chats
        public async Task<IActionResult> Index()
        {
                return _context.Chat != null ?
            View(await _context.Chat.ToListAsync()) :
            Problem("Entity set 'ChitChatContext.Chat'  is null.");
     

        }

        // GET: Chats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Chat == null)
            {
                return NotFound();
            }

            var chat = await _context.Chat
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chat == null)
            {
                return NotFound();
            }

            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "Users");
            }
            else
            {
                return View(chat);
            }
  
        }

        // GET: Chats/Create
        //[Authorize]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "Users");
            }
            else
            {
                return View();
            }
        }

        // POST: Chats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name,ChatContent")] Chat chat)
        {
            if (ModelState.IsValid)
            {
                if (HttpContext.Session.GetString("username") == null)
                {
                    return RedirectToAction("Login", "Users");
                }
                else
                {
                    _context.Add(chat);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), "Chats");
                }
            }
            return View(chat);
        }

        // GET: Chats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Chat == null)
            {
                return NotFound();
            }

            var chat = await _context.Chat.FindAsync(id);
            if (chat == null)
            {
                return NotFound();
            }
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "Users");
            }
            else
            {
                return View(chat);
            }
        }

        // POST: Chats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ChatContent")] Chat chat)
        {
            if (id != chat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChatExists(chat.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (HttpContext.Session.GetString("username") == null)
                {
                    return RedirectToAction("Login", "Users");
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(chat);
        }

        // GET: Chats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Chat == null)
            {
                return NotFound();
            }

            var chat = await _context.Chat
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chat == null)
            {
                return NotFound();
            }
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "Users");
            }
            else
            {
                return View(chat);
            }
        }

        // POST: Chats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Chat == null)
            {
                return Problem("Entity set 'ChitChatContext.Chat'  is null.");
            }
            var chat = await _context.Chat.FindAsync(id);
            if (chat != null)
            {
                _context.Chat.Remove(chat);
            }
            
            await _context.SaveChangesAsync();
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "Users");
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        private bool ChatExists(int id)
        {
          return (_context.Chat?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
