using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Aphroditus.Data;
using Aphroditus.Models;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Aphroditus.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly string _imgFolder;

        public PostsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _imgFolder = Path.Combine(environment.WebRootPath, "images");
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
           List<PostsViewModel> allPosts = new();
            var Posts = await _context.Post.ToListAsync();
            try
            {

                foreach (Post P in Posts)
                {
                    allPosts.Add(new()
                    {
                        Title = P.Title,
                        Description = P.Description,
                        PostDate = P.PostDate,
                        FullPath = Path.Combine("/images", P.ImgName!)
                    });
                }
                return View(allPosts);
            } 
            catch
            {
                return NotFound();
            }
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Image,Description")] CreatePostViewModel post)
        {
            if (ModelState.IsValid)
            {
              
                string? UniqueFilename;
                
               
                if (!Directory.Exists(_imgFolder))
                {
                    Directory.CreateDirectory(_imgFolder);
                }
                
                UniqueFilename = Guid.NewGuid().ToString() + "_" + post.Image!.FileName;
                string filepath = Path.Combine(_imgFolder, UniqueFilename);
                using (var filestream = new FileStream(filepath, FileMode.Create))
                {
                    post.Image.CopyTo(filestream);
                }

                Post newPost = new()
                {
                    Title = post.Title,
                    Description = post.Description,
                    ImgName = UniqueFilename,
                    PostDate = DateTime.Now
                };
               
                _context.Add(newPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,PostDate")] EditPostViewModel post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // _context.Update(post);
                    // await _context.SaveChangesAsync();
                    Post currPost = _context.Post.Find(id)!;
                    currPost.Title = post.Title;
                    currPost.Description = post.Description;
                    currPost.PostDate = post.PostDate;

                    _context.Update(currPost);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Details), new { id });
                   


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return View(post);
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Post.FindAsync(id);
            if (post != null)
            {
                _context.Post.Remove(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.Id == id);
        }
    }
}
