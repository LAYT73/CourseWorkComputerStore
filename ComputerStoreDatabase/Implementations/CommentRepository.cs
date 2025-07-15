using ComputerStoreContracts.Repositories;
using ComputerStoreModels.Models;

namespace ComputerStoreDatabase.Implementations;

public class CommentRepository : Repository<Comment>, ICommentRepository
{
    public CommentRepository(AppDbContext context) : base(context) { }
}
