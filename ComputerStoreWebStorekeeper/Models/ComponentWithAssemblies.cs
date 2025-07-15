using ComputerStoreModels.Models;

namespace ComputerStoreWebStorekeeper.Models
{
    public class ComponentWithAssemblies
    {
        public Component Component { get; set; }
        public IEnumerable<Assembly> LinkedAssemblies { get; set; }
    }
}
