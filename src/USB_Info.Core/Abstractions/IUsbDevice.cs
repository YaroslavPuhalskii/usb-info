namespace USB_Info.Core.Abstractions;

public interface IUsbDevice
{
    ushort HubIndex { get; set; }

    uint Port { get; set; }

    string? ConnectionName { get; }

    string? ConnectionStat { get; }
}
