namespace BlazorVisu.Models
{
    public enum ComponentType
    {
        TypeA,
        TypeB,
        TypeC
    }

    public enum StationStatus
    {
        Running,
        Stopped,
        Error,
        Maintenance
    }

    public class Component
    {
        public ComponentType Type { get; set; }
        public string? CurrentStationId { get; set; }
    }

    public class Machine
    {
        public StationStatus Status { get; set; } = StationStatus.Running;
    }

    public class Switch
    {
        public StationStatus Status { get; set; } = StationStatus.Running;
    }

    public class Consumer
    {
        public StationStatus Status { get; set; } = StationStatus.Running;
    }

    public class ProductionSystem
    {
        public Machine Machine { get; set; } = new();
        public Switch Switch { get; set; } = new();
        public List<Consumer> Consumers { get; set; } = new();
        public List<Component> ComponentsInTransit { get; set; } = new();
    }
}