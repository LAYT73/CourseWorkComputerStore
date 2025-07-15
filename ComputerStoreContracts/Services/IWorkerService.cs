using ComputerStoreModels.Models;

namespace ComputerStoreContracts.Services;

public interface IWorkerService
{
    // Sale
    Task<Sale> GetSaleByIdAsync(Guid id);
    Task<IEnumerable<Sale>> GetAllSalesByUserIdAsync(Guid userId);
    Task CreateSaleAsync(Sale sale);
    Task UpdateSaleAsync(Sale sale);
    Task DeleteSaleAsync(Guid id);
    Task AddProductsToSaleAsync(Guid saleId, List<Guid> productIds); // Привязка товаров к продаже
    Task RemoveProductsFromSaleAsync(Guid saleId, List<Guid> productIds);

    // Assembly
    Task<Assembly> GetAssemblyByIdAsync(Guid id);
    Task<IEnumerable<Assembly>> GetAllAssembliesByUserIdAsync(Guid userId);
    Task CreateAssemblyAsync(Assembly assembly);
    Task UpdateAssemblyAsync(Assembly assembly);
    Task DeleteAssemblyAsync(Guid id);
    Task AddSalesToAssemblyAsync(Guid assemblyId, List<Guid> saleIds); // Привязка продаж к сборке
    Task RemoveSalesFromAssemblyAsync(Guid assemblyId, List<Guid> saleIds);

    // Comment
    Task<Comment> GetCommentByIdAsync(Guid id);
    Task<IEnumerable<Comment>> GetAllCommentsByUserIdAsync(Guid userId);
    Task CreateCommentAsync(Comment comment); 
    Task UpdateCommentAsync(Comment comment);
    Task DeleteCommentAsync(Guid id);
    Task AddAssembliesToCommentAsync(Guid commentId, List<Guid> assemblyIds); // Привязка сборок к коментарию
    Task RemoveAssembliesFromCommentAsync(Guid commentId, List<Guid> assemblyIds);

    // Reports
    Task<byte[]> GenerateProductsByAssembliesReportAsync(List<Guid> assemblyIds, string format);
    Task<byte[]> GenerateSaleDetailReportAsync(Guid saleId, DateTime startDate, DateTime endDate);
    Task SendSaleDetailReportByEmailAsync(Guid saleId, DateTime startDate, DateTime endDate);

    // Auth
    Task<User> LoginAsync(string login, string password);
    Task RegisterAsync(User user);
}
