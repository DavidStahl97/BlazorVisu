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

    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Component
    {
        public ComponentType Type { get; set; }
        public string? CurrentStationId { get; set; }
    }

    public class Machine
    {
        public string Id { get; set; } = "MACHINE_01";
        public string Name { get; set; } = "Machine";
        public StationStatus Status { get; set; } = StationStatus.Running;
    }

    public class Switch
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public StationStatus Status { get; set; } = StationStatus.Running;
    }

    public class Consumer
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string SwitchId { get; set; } = string.Empty;
        public StationStatus Status { get; set; } = StationStatus.Running;
    }

    public class ProductionConfig
    {
        public Machine Machine { get; set; } = new();
        public List<Switch> Switches { get; set; } = new();
        public List<Consumer> Consumers { get; set; } = new();
    }

    public class TransportRoute
    {
        public string TargetSwitchId { get; set; } = string.Empty;
        public string TargetConsumerId { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class ProductionSystem
    {
        public Machine Machine { get; set; } = new();
        public List<Switch> Switches { get; set; } = new();
        public List<Consumer> Consumers { get; set; } = new();
        public List<Component> ComponentsInTransit { get; set; } = new();
        public TransportRoute CurrentTransportRoute { get; set; } = new();
    }
}