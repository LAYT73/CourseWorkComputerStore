using ComputerStoreContracts.Repositories;
using ComputerStoreModels.Models;

namespace ComputerStoreDatabase.Implementations;

public class AssemblyRepository : Repository<Assembly>, IAssemblyRepository
{
    public AssemblyRepository(AppDbContext context) : base(context) { }
}
