using ComputerStoreContracts.Services;
using ComputerStoreDatabase;
using ComputerStoreModels.Enums;
using ComputerStoreModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerStoreWebStorekeeper.Controllers
{
    public class ComponentController : Controller
    {
        private readonly IStorekeeperService _storekeeperService;
        private readonly AppDbContext _context;

        public ComponentController(AppDbContext context, IStorekeeperService storekeeperService)
        {
            _context = context;
            _storekeeperService = storekeeperService;
        }

        // GET: /Component
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var components = await _context.Components
                .Include(a => a.AssemblyComponents)
                    .ThenInclude(ac => ac.Assembly)
                .ToListAsync();

            return View(components);
        }

        // GET: /Component/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var assemblies = await _context.Assemblies
                .ToListAsync();

            ViewBag.Assemblies = assemblies;
            return View();
        }

        // POST: /Component/Create
        [HttpPost]
        public async Task<IActionResult> Create(string name, string description, decimal price, List<Guid> assemblyIds)
        {
            var user = GetCurrentUser();

            var component = new Component
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                Price = price,
                UserId = user.Id,
            };

            foreach (var assemblyId in assemblyIds)
            {
                var assembly = await _context.Assemblies.FindAsync(assemblyId);
                if (assembly != null)
                {
                    component.AssemblyComponents.Add(new AssemblyComponent
                    {
                        Id = Guid.NewGuid(),
                        AssemblyId = assemblyId,
                        ComponentId = component.Id,
                    });
                }
            }

            await _context.Components.AddAsync(component);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: /Component/Edit?id=...
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var component = await _context.Components
                .Include(a => a.AssemblyComponents)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (component == null) return NotFound();

            var assemblies = await _context.Assemblies
                .ToListAsync();

            ViewBag.Assemblies = assemblies;
            return View(component);
        }

        // POST: /Component/Update
        [HttpPost]
        public async Task<IActionResult> Update(Guid id, string name, string description, decimal price, List<Guid> assemblyIds)
        {
            var component = await _context.Components
                .FirstOrDefaultAsync(a => a.Id == id);

            if (component == null) return NotFound();

            component.Name = name;
            component.Description = description;
            component.Price = price;

            var existingAssemblyComponents = await _context.AssemblyComponents
                .Where(ac => ac.ComponentId == id)
                .ToListAsync();

            _context.AssemblyComponents.RemoveRange(existingAssemblyComponents);

            foreach (var assemblyId in assemblyIds.Distinct())
            {
                _context.AssemblyComponents.Add(new AssemblyComponent
                {
                    Id = Guid.NewGuid(),
                    ComponentId = component.Id,
                    AssemblyId = assemblyId
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: /Component/Delete?id=...
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var component = await _context.Components
                .Include(a => a.AssemblyComponents)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (component == null) return NotFound();

            _context.AssemblyComponents.RemoveRange(component.AssemblyComponents);
            _context.Components.Remove(component);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private User GetCurrentUser()
        {
            var userData = HttpContext.Session.GetString("User");
            return userData != null ?
                System.Text.Json.JsonSerializer.Deserialize<User>(userData)! :
                new User();
        }
    }
}
