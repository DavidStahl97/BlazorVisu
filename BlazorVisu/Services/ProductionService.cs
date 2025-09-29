using BlazorVisu.Models;
using System.Text.Json;

namespace BlazorVisu.Services
{
    public interface IProductionService
    {
        ProductionSystem GetCurrentState();
        void SetTransportRoute(string switchId, string consumerId);
        void ClearTransportRoute();
        event EventHandler<TransportRoute>? TransportRouteChanged;
    }

    public class ProductionService : IProductionService
    {
        private readonly ProductionSystem _productionSystem;
        private readonly IWebHostEnvironment _environment;

        public event EventHandler<TransportRoute>? TransportRouteChanged;

        public ProductionService(IWebHostEnvironment environment)
        {
            _environment = environment;
            _productionSystem = InitializeProductionSystem();
        }

        public ProductionSystem GetCurrentState() => _productionSystem;

        public void SetTransportRoute(string switchId, string consumerId)
        {
            _productionSystem.CurrentTransportRoute = new TransportRoute
            {
                TargetSwitchId = switchId,
                TargetConsumerId = consumerId,
                IsActive = true
            };

            TransportRouteChanged?.Invoke(this, _productionSystem.CurrentTransportRoute);
        }

        public void ClearTransportRoute()
        {
            _productionSystem.CurrentTransportRoute = new TransportRoute
            {
                IsActive = false
            };

            TransportRouteChanged?.Invoke(this, _productionSystem.CurrentTransportRoute);
        }

        private ProductionSystem InitializeProductionSystem()
        {
            var configPath = Path.Combine(_environment.WebRootPath, "production-config.json");

            if (File.Exists(configPath))
            {
                var jsonContent = File.ReadAllText(configPath);
                var config = JsonSerializer.Deserialize<ProductionConfig>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (config != null)
                {
                    return new ProductionSystem
                    {
                        Machine = config.Machine,
                        Switches = config.Switches,
                        Consumers = config.Consumers,
                        ComponentsInTransit = new List<Component>()
                    };
                }
            }

            // Fallback to default configuration
            return CreateDefaultSystem();
        }

        private ProductionSystem CreateDefaultSystem()
        {
            var system = new ProductionSystem
            {
                Machine = new Machine
                {
                    Id = "MACHINE_01",
                    Name = "Machine",
                    Status = StationStatus.Running
                }
            };

            // Create 2 switches
            system.Switches.Add(new Switch
            {
                Id = "SWITCH_01",
                Name = "Switch 1",
                Status = StationStatus.Running
            });
            system.Switches.Add(new Switch
            {
                Id = "SWITCH_02",
                Name = "Switch 2",
                Status = StationStatus.Running
            });

            // Create 10 consumers (5 per switch)
            for (int i = 1; i <= 10; i++)
            {
                var switchId = i <= 5 ? "SWITCH_01" : "SWITCH_02";

                system.Consumers.Add(new Consumer
                {
                    Id = $"CONSUMER_{i:D2}",
                    Name = $"Consumer {i}",
                    SwitchId = switchId,
                    Status = StationStatus.Running
                });
            }

            return system;
        }
    }
}