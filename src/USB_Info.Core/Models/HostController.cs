namespace USB_Info.Core.Models;

public class HostController
{
    public string? Name { get; set; }

    public string? RootHubPath { get; set; }

    public string? HostControllerPath { get; set; }

    public ushort HubIndex { get; set; }

    public int HubPorts { get; set; }

    public string? ConnectionStatus { get; set; }

    public UsbHub? Hub { get; set; }

    public HostController(string name,
        string rootHubPath,
        string hostControllerPath,
        ushort hubIndex,
        int hubPorts,
        string connectionStatus)
    {
        Name = name;
        RootHubPath = rootHubPath;
        HostControllerPath = hostControllerPath;
        HubIndex = hubIndex;
        HubPorts = hubPorts;
        ConnectionStatus = connectionStatus;
    }

    public HostController()
    { }
}
