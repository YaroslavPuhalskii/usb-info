using USB_Info.Core.Abstractions;

namespace USB_Info.Core.Models;

internal class UnConnectUsbDevice : IUsbDevice
{
    public ushort HubIndex { get; set; }

    public uint Port { get; set; }

    public string? ConnectionName { get; }

    public string? ConnectionStat { get; }

    public UnConnectUsbDevice(string connectionName,
        string connectionStat,
        ushort hubIndex,
        uint portIndex)
    {
        ConnectionName = connectionName;
        ConnectionStat = connectionStat;
        HubIndex = hubIndex;
        Port = portIndex;
    }
}