using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSD_Lab1_TeamsWithMembership.Data;
using SSD_Lab1_TeamsWithMembership.Models;

namespace SSD_Lab1_TeamsWithMembership.Controllers
{
    [Authorize(Roles = "Manager")]
    public class PlayersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PlayersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Players
        public async Task<IActionResult> Index()
        {
            var players = await _userManager.GetUsersInRoleAsync("Player");

            var playerViewModels = new List<PlayerViewModel>();

            foreach(var player in players)
            {
                playerViewModels.Add(new PlayerViewModel()
                {
                    Id = player.Id,
                    FirstName = player.FirstName,
                    LastName = player.LastName,
                    BirthDate = player.BirthDate,
                    Email = player.Email,
                    PhoneNumber = player.PhoneNumber
                });
            }

            return View(playerViewModels);
        }

        // GET: Players/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _userManager.FindByIdAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            var playerViewModel = new PlayerViewModel()
            {
                Id = player.Id,
                FirstName = player.FirstName,
                LastName = player.LastName,
                BirthDate = player.BirthDate,
                Email = player.Email,
                PhoneNumber = player.PhoneNumber
            };

            return View(playerViewModel);
        }

        // GET: Players/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,BirthDate,Email,ConfirmPassword,Password,PhoneNumber")] FullPlayerViewModel playerViewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser existingUser = await _userManager.FindByNameAsync(playerViewModel.Email);

                if(existingUser != null)
                {
                    ModelState.AddModelError("", "Email already registered.");
                    return View(playerViewModel);
                }

                ApplicationUser user = new ApplicationUser()
                {
                    FirstName = playerViewModel.FirstName,
                    LastName = playerViewModel.LastName,
                    BirthDate = playerViewModel.BirthDate,
                    Email = playerViewModel.Email,
                    EmailConfirmed = true,
                    PhoneNumber = playerViewModel.PhoneNumber,
                    UserName = playerViewModel.Email,
                };

                IdentityResult result = await _userManager.CreateAsync(user, playerViewModel.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Player");
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                } else
                {
                    foreach(IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

            }
            return View(playerViewModel);
        }

        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _userManager.FindByIdAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            var playerViewModel = new PlayerViewModel()
            {
                Id = player.Id,
                FirstName = player.FirstName,
                LastName = player.LastName,
                BirthDate = player.BirthDate,
                Email = player.Email,
                PhoneNumber = player.PhoneNumber
            };

            return View(playerViewModel);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FirstName,LastName,BirthDate,Email,PhoneNumber")] PlayerViewModel playerViewModel)
        {
            if (id != playerViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var player = await _userManager.FindByIdAsync(id);

                    if (player == null)
                    {
                        return NotFound();
                    }

                    player.FirstName = playerViewModel.FirstName;
                    player.LastName = playerViewModel.LastName;
                    player.BirthDate = playerViewModel.BirthDate;
                    player.PhoneNumber = playerViewModel.PhoneNumber;

                    _context.Update(player);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await PlayerViewModelExists(playerViewModel.Id))
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
            return View(playerViewModel);
        }

        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _userManager.FindByIdAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            var playerViewModel = new PlayerViewModel()
            {
                Id = player.Id,
                FirstName = player.FirstName,
                LastName = player.LastName,
                BirthDate = player.BirthDate,
                Email = player.Email,
                PhoneNumber = player.PhoneNumber
            };

            return View(playerViewModel);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_userManager.Users == null)
            {
                return Problem("Entity set is null.");
            }
            var player = await _userManager.FindByIdAsync(id);
            if (player != null)
            {
                await _userManager.DeleteAsync(player);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> PlayerViewModelExists(string id)
        {
            return await _userManager.FindByIdAsync(id) != null;
        }
    }
}
