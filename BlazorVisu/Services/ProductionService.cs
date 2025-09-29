using BlazorVisu.Models;
using Microsoft.AspNetCore.SignalR;

namespace BlazorVisu.Services
{
    public class ProductionHub : Hub
    {
        public async Task JoinProductionGroup()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "ProductionUpdates");
        }

        public async Task LeaveProductionGroup()
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "ProductionUpdates");
        }
    }

    public interface IProductionService
    {
        ProductionSystem GetCurrentState();
        Task StartProductionAsync();
        Task StopProductionAsync();
        Task SetMachineStatusAsync(StationStatus status);
        Task SetConsumerStatusAsync(string consumerId, StationStatus status);
        event EventHandler<ProductionSystem>? ProductionUpdated;
    }

    public class ProductionService : IProductionService, IHostedService
    {
        private readonly IHubContext<ProductionHub> _hubContext;
        private readonly ILogger<ProductionService> _logger;
        private readonly ProductionSystem _productionSystem;
        private Timer? _updateTimer;
        private Timer? _componentTimer;
        private readonly Random _random = new();

        public event EventHandler<ProductionSystem>? ProductionUpdated;

        public ProductionService(IHubContext<ProductionHub> hubContext, ILogger<ProductionService> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
            _productionSystem = InitializeProductionSystem();
        }

        public ProductionSystem GetCurrentState() => _productionSystem;

        public async Task StartProductionAsync()
        {
            _productionSystem.Machine.Status = StationStatus.Running;
            _productionSystem.Switch.Status = StationStatus.Running;

            foreach (var consumer in _productionSystem.Consumers)
            {
                consumer.Status = StationStatus.Running;
            }

            await NotifyClientsAsync();
            _logger.LogInformation("Production started");
        }

        public async Task StopProductionAsync()
        {
            _productionSystem.Machine.Status = StationStatus.Stopped;
            await NotifyClientsAsync();
            _logger.LogInformation("Production stopped");
        }

        public async Task SetMachineStatusAsync(StationStatus status)
        {
            _productionSystem.Machine.Status = status;
            await NotifyClientsAsync();
            _logger.LogInformation("Machine status changed to {Status}", status);
        }

        public async Task SetConsumerStatusAsync(string consumerId, StationStatus status)
        {
            var consumer = _productionSystem.Consumers.FirstOrDefault();
            if (consumer != null)
            {
                consumer.Status = status;
                await NotifyClientsAsync();
                _logger.LogInformation("Consumer status changed to {Status}", status);
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Update production statistics every 2 seconds
            _updateTimer = new Timer(async _ => await UpdateProductionAsync(), null,
                                   TimeSpan.Zero, TimeSpan.FromSeconds(2));

            // Generate new components every 3 seconds when running
            _componentTimer = new Timer(async _ => await GenerateComponentsAsync(), null,
                                      TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(3));

            _logger.LogInformation("Production service started");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _updateTimer?.Dispose();
            _componentTimer?.Dispose();
            _logger.LogInformation("Production service stopped");
            return Task.CompletedTask;
        }

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

        private async Task UpdateProductionAsync()
        {
            try
            {
                // Occasionally introduce random status changes for realism
                if (_productionSystem.Machine.Status == StationStatus.Running && _random.Next(0, 100) < 2) // 2% chance
                {
                    var statuses = new[] { StationStatus.Maintenance, StationStatus.Error };
                    var randomStatus = statuses[_random.Next(statuses.Length)];
                    await SetMachineStatusAsync(randomStatus);

                    // Auto-recover after some time
                    _ = Task.Delay(TimeSpan.FromSeconds(10)).ContinueWith(async _ =>
                    {
                        await SetMachineStatusAsync(StationStatus.Running);
                    });
                }

                await NotifyClientsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating production");
            }
        }

        private async Task GenerateComponentsAsync()
        {
            try
            {
                if (_productionSystem.Machine.Status != StationStatus.Running)
                    return;

                // Generate new component
                var componentTypes = Enum.GetValues<ComponentType>();
                var newComponent = new Component
                {
                    Type = componentTypes[_random.Next(componentTypes.Length)],
                    CurrentStationId = "MACHINE_01"
                };

                _productionSystem.ComponentsInTransit.Add(newComponent);

                // Simulate component movement through system
                _ = Task.Delay(TimeSpan.FromSeconds(1.5)).ContinueWith(async _ =>
                {
                    // Move to switch
                    newComponent.CurrentStationId = "SWITCH_01";
                    await NotifyClientsAsync();

                    await Task.Delay(TimeSpan.FromSeconds(1.5));

                    // Route to consumer
                    var consumerIndex = _random.Next(_productionSystem.Consumers.Count);
                    newComponent.CurrentStationId = $"CONSUMER_{consumerIndex + 1}";

                    await Task.Delay(TimeSpan.FromSeconds(1));

                    // Remove from transit
                    _productionSystem.ComponentsInTransit.Remove(newComponent);
                    await NotifyClientsAsync();
                });

                await NotifyClientsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating components");
            }
        }

        private async Task NotifyClientsAsync()
        {
            await _hubContext.Clients.Group("ProductionUpdates")
                .SendAsync("ProductionUpdated", _productionSystem);

            ProductionUpdated?.Invoke(this, _productionSystem);
        }
    }
}