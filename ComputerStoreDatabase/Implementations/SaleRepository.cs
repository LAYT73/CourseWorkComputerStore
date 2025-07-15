using ComputerStoreContracts.Repositories;
using ComputerStoreModels.Models;

namespace ComputerStoreDatabase.Implementations;

public class SaleRepository : Repository<Sale>, ISaleRepository
{
    public SaleRepository(AppDbContext context) : base(context) { }
}
