using ComputerStoreContracts.Services;
using ComputerStoreDatabase;
using ComputerStoreModels.Enums;
using ComputerStoreModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerStoreWebStorekeeper.Controllers
{
    public class ProductController : Controller
    {
        private readonly IStorekeeperService _storekeeperService;
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context, IStorekeeperService storekeeperService)
        {
            _context = context;
            _storekeeperService = storekeeperService;
        }

        // GET: /Product
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = GetCurrentUser();

            var products = await _context.Products
                .Include(p => p.ProductComponents)
                    .ThenInclude(pc => pc.Component)
                .Where(p => p.UserId == user.Id)
                .ToListAsync();

            return View(products);
        }

        // GET: /Product/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = GetCurrentUser();
            var components = await _context.Components
                .Where(p => p.UserId == user.Id)
                .ToListAsync();

            ViewBag.Components = components;
            return View();
        }

        // POST: /Product/Create
        [HttpPost]
        public async Task<IActionResult> Create(string name, string description, decimal price, List<Guid> componentIds)
        {
            var user = GetCurrentUser();

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                Price = price,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow
            };

            foreach (var componentId in componentIds)
            {
                var component = await _context.Components.FindAsync(componentId);
                if (component != null)
                {
                    product.ProductComponents.Add(new ProductComponent
                    {
                        Id = Guid.NewGuid(),
                        ProductId = product.Id,
                        ComponentId = componentId
                    });
                }
            }

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        // GET: /Product/Edit?id=...
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var product = await _context.Products
                .Include(p => p.ProductComponents)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            var user = GetCurrentUser();

            var userComponents = await _context.Components
                .Where(c => c.UserId == user.Id)
                .ToListAsync();

            ViewBag.Components = userComponents;
            return View(product);
        }

        // POST: /Product/Edit
        [HttpPost]
        public async Task<IActionResult> Update(Guid id, string name, decimal price, List<Guid> componentIds)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();

            product.Name = name;
            product.Price = price;

            var existingLinks = await _context.ProductComponents
                .Where(pc => pc.ProductId == id)
                .ToListAsync();

            _context.ProductComponents.RemoveRange(existingLinks);

            foreach (var componentId in componentIds.Distinct())
            {
                _context.ProductComponents.Add(new ProductComponent
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    ComponentId = componentId
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: /Product/Delete?id=...
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _context.Products
                .Include(p => p.ProductComponents)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            _context.ProductComponents.RemoveRange(product.ProductComponents);
            _context.Products.Remove(product);

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
