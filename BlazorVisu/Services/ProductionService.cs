using BlazorVisu.Models;

namespace BlazorVisu.Services
{
    public interface IProductionService
    {
        ProductionSystem GetCurrentState();
    }

    public class ProductionService : IProductionService
    {
        private readonly ProductionSystem _productionSystem;

        public ProductionService()
        {
            _productionSystem = InitializeProductionSystem();
        }

        public ProductionSystem GetCurrentState() => _productionSystem;


        private ProductionSystem InitializeProductionSystem()
        {
            var system = new ProductionSystem
            {
                Machine = new Machine { Status = StationStatus.Running },
                Switch = new Switch { Status = StationStatus.Running }
            };

            // Initialize 3 consumers
            for (int i = 1; i <= 3; i++)
            {
                system.Consumers.Add(new Consumer { Status = StationStatus.Running });
            }

            return system;
        }

    }
}