using ComputerStoreContracts.Repositories;
using ComputerStoreContracts.Services;
using ComputerStoreDatabase;
using ComputerStoreModels.Models;
using ComputerStoreModels.Reports;
using Microsoft.EntityFrameworkCore;

namespace ComputerStoreServices.Implementations;

public class StorekeeperService : IStorekeeperService
{
    private readonly AppDbContext _context;
    private readonly IComponentRepository _componentRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly WordExcelReportService _wordExcelService;
    private readonly PdfReportService _pdfReportService;
    private readonly IEmailService _emailService;

    public StorekeeperService(
        AppDbContext context,
        IComponentRepository componentRepository,
        IProductRepository productRepository,
        IOrderRepository orderRepository,
        IUserRepository userRepository,
        WordExcelReportService wordExcelService,
        PdfReportService pdfReportService,
        IEmailService emailService)
    {
        _context = context;
        _componentRepository = componentRepository;
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _wordExcelService = wordExcelService;
        _pdfReportService = pdfReportService;
        _emailService = emailService;
    }

    // ----------------------- COMPONENT -----------------------

    public async Task<Component?> GetComponentByIdAsync(Guid id)
        => await _componentRepository.GetByIdAsync(id);

    public async Task<IEnumerable<Component>> GetAllComponentsByUserIdAsync(Guid userId)
        => await _componentRepository.GetAllByUserIdAsync(userId);

    public async Task CreateComponentAsync(Component component)
        => await _componentRepository.AddAsync(component);

    public async Task UpdateComponentAsync(Component component)
        => await _componentRepository.UpdateAsync(component);

    public async Task DeleteComponentAsync(Guid id)
        => await _componentRepository.DeleteAsync(id);

    public async Task AttachToAssemblyAsync(Guid componentId, List<Guid> assemblyIds)
    {
        foreach (var assemblyId in assemblyIds)
        {
            await _componentRepository.AttachToAssemblyAsync(componentId, assemblyId);
        }
    }

    public async Task DetachFromAssemblyAsync(Guid componentId, List<Guid> assemblyIds)
    {
        foreach (var assemblyId in assemblyIds)
        {
            await _componentRepository.DetachFromAssemblyAsync(componentId, assemblyId);
        }
    }

    // ----------------------- PRODUCT -----------------------

    public async Task<Product?> GetProductByIdAsync(Guid id)
        => await _productRepository.GetByIdAsync(id);

    public async Task<IEnumerable<Product>> GetAllProductsByUserIdAsync(Guid userId)
        => await _productRepository.GetAllByUserIdAsync(userId);

    public async Task CreateProductAsync(Product product)
        => await _productRepository.AddAsync(product);

    public async Task UpdateProductAsync(Product product)
        => await _productRepository.UpdateAsync(product);

    public async Task DeleteProductAsync(Guid id)
        => await _productRepository.DeleteAsync(id);

    public async Task AddComponentsToProductAsync(Guid productId, List<Guid> componentIds)
    {
        foreach (var componentId in componentIds)
        {
            await _productRepository.AddComponentToProductAsync(productId, componentId);
        }
    }

    public async Task RemoveComponentsFromProductAsync(Guid productId, List<Guid> componentIds)
    {
        foreach (var componentId in componentIds)
        {
            await _productRepository.RemoveComponentFromProductAsync(productId, componentId);
        }
    }

    // ----------------------- ORDER -----------------------

    public async Task<Order?> GetOrderByIdAsync(Guid id)
        => await _orderRepository.GetByIdAsync(id);

    public async Task<IEnumerable<Order>> GetAllOrdersByUserIdAsync(Guid userId)
        => await _orderRepository.GetAllByUserIdAsync(userId);

    public async Task CreateOrderAsync(Order order)
        => await _orderRepository.AddAsync(order);

    public async Task UpdateOrderAsync(Order order)
        => await _orderRepository.UpdateAsync(order);

    public async Task DeleteOrderAsync(Guid id)
        => await _orderRepository.DeleteAsync(id);

    public async Task AddProductsToOrderAsync(Guid orderId, List<Guid> productIds)
    {
        foreach (var productId in productIds)
        {
            await _orderRepository.AddProductToOrderAsync(orderId, productId);
        }
    }

    public async Task RemoveProductsFromOrderAsync(Guid orderId, List<Guid> productIds)
    {
        foreach (var productId in productIds)
        {
            await _orderRepository.RemoveProductFromOrderAsync(orderId, productId);
        }
    }

    // ----------------------- REPORTS -----------------------

    public async Task<byte[]> GenerateAssembliesByProductsReportAsync(List<Guid> productIds, string format)
    {
        var reportData = await _context.Assemblies
            .SelectMany(a => a.AssemblyComponents, (a, ac) => new { a, ac })
            .SelectMany(join1 => join1.ac.Component!.ProductComponents, (join1, pc) => new AssemblyByProductReportView
            {
                AssemblyId = join1.a.Id,
                AssemblyName = join1.a.Name,
                AssemblyDescription = join1.a.Description,
                AssemblyCreatedAt = join1.a.CreatedAt,

                ComponentId = join1.ac.Component!.Id,
                ComponentName = join1.ac.Component.Name,
                ComponentPrice = join1.ac.Component.Price,

                ProductId = pc.Product!.Id,
                ProductName = pc.Product.Name,
                ProductPrice = pc.Product.Price
            })
            .Where(view => productIds.Contains(view.ProductId))
            .ToListAsync();

        return format.ToLower() switch
        {
            "docx" => await _wordExcelService.GenerateAssemblyListByProductsWordAsync(reportData),
            "xlsx" => await _wordExcelService.GenerateAssemblyListByProductsExcelAsync(reportData),
            _ => throw new ArgumentException("Unsupported format")
        };
    }

    public async Task<byte[]> GenerateComponentDetailReportAsync(List<Guid> componentIds, DateTime startDate, DateTime endDate, string format)
    {
        var reportData = await GetComponentDetailReportData(componentIds, startDate, endDate);

        return format.ToLower() switch
        {
            "pdf" => await _pdfReportService.GenerateComponentDetailReportAsync(reportData),
            _ => throw new ArgumentException("Unsupported format")
        };
    }

    public async Task<List<ComponentDetailReport>> GetComponentDetailReportData(List<Guid> componentIds, DateTime startDate, DateTime endDate)
    {
        var result = await _context.Components
            .Where(c => componentIds.Contains(c.Id))
            .Select(c => new ComponentDetailReport
            {
                ComponentName = c.Name,
                ReportDate = DateTime.UtcNow,

                PurchaseDescription = string.Join(", ",
                    c.ProductComponents
                        .SelectMany(pc => pc.Product!.SaleProducts)
                        .Where(sp => sp.Sale!.CreatedAt >= startDate && sp.Sale.CreatedAt <= endDate)
                        .Select(sp => $"Продан товар '{sp.Product!.Name}' в продаже '{sp.Sale!.Name}' ({sp.Sale.CreatedAt:dd.MM.yyyy})")
                        .Distinct()),

                OrderDescription = string.Join(", ",
                    c.ProductComponents
                        .SelectMany(pc => pc.Product!.OrderProducts)
                        .Where(op => op.Order!.CreatedAt >= startDate && op.Order.CreatedAt <= endDate)
                        .Select(op => $"Заказан товар '{op.Product!.Name}' в заказе '{op.Order!.Name}' ({op.Order.CreatedAt:dd.MM.yyyy})")
                        .Distinct()),

                Details = $"Цена: {c.Price}, Описание: {c.Description}"
            })
            .ToListAsync();

        return result;
    }

    public async Task SendComponentDetailReportByEmailAsync(string email, List<Guid> componentIds, DateTime startDate, DateTime endDate)
    {
        var reportData = await GenerateComponentDetailReportAsync(componentIds, startDate, endDate, "pdf");
        await _emailService.SendEmailAsync(email, "Отчет по комплектующим", reportData, $"report_components_{startDate}-{endDate}.pdf");
    }

    // ----------------------- AUTH -----------------------

    public async Task<User?> LoginAsync(string login, string password) 
    {
        if (await _userRepository.ValidateUserAsync(login, password))
        {
            return await _userRepository.GetByLoginAsync(login);
        }
        return null;
    }

    public async Task RegisterAsync(User user)
        => await _userRepository.CreateAsync(user);
}
