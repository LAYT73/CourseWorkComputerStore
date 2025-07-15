using ComputerStoreContracts.Services;
using ComputerStoreDatabase;
using ComputerStoreModels.Enums;
using ComputerStoreModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerStoreWebStorekeeper.Controllers
{
    public class OrderController : Controller
    {
        private readonly IStorekeeperService _storekeeperService;
        private readonly AppDbContext _context;
        public OrderController(AppDbContext context, IStorekeeperService storekeeperService)
        {
            _context = context;
            _storekeeperService = storekeeperService;
        }

        // GET: /Order
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = GetCurrentUser();

            var orders = await _context.Orders
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .Where(o => o.UserId == user.Id)
                .ToListAsync();

            return View(orders);
        }

        // GET: /Order/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = GetCurrentUser();
            var products = await _context.Products
                .Where(p => p.UserId == user.Id)
                .ToListAsync();

            ViewBag.Products = products;
            return View();
        }

        // POST: /Order/Create
        [HttpPost]
        public async Task<IActionResult> Create(string name, OrderType status, List<Guid> productIds)
        {
            var user = GetCurrentUser();

            var order = new Order
            {
                Id = Guid.NewGuid(),
                Name = name,
                Status = status,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow
            };

            foreach (var productId in productIds)
            {
                var product = await _context.Products.FindAsync(productId);
                if (product != null)
                {
                    order.OrderProducts.Add(new OrderProduct
                    {
                        Id = Guid.NewGuid(),
                        OrderId = order.Id,
                        ProductId = productId
                    });
                }
            }

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: /Order/Edit?id=...
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            var user = GetCurrentUser();

            var userProducts = await _context.Products
                .Where(p => p.UserId == user.Id)
                .ToListAsync();

            ViewBag.Products = userProducts;
            return View(order);
        }

        // POST: /Order/Edit
        [HttpPost]
        public async Task<IActionResult> Update(Guid id, string name, OrderType status, List<Guid> productIds)
        {

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id);


            if (order == null) return NotFound();

            order.Name = name;
            order.Status = status;

            var existingOrderProducts = await _context.OrderProducts
                .Where(op => op.OrderId == id)
                .ToListAsync();

            // Удалить старые связи
            _context.OrderProducts.RemoveRange(existingOrderProducts);

            //// Добавить новые связи
            foreach (var productId in productIds.Distinct())
            {
                _context.OrderProducts.Add(new OrderProduct
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ProductId = productId
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: /Order/Delete?id=...
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            _context.OrderProducts.RemoveRange(order.OrderProducts);
            _context.Orders.Remove(order);

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
