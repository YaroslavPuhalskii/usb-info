using USB_Info.Core.Models;

namespace USB_Info.Core.Abstractions;

public interface IConnectedUsbDevice : IUsbDevice
{
    ushort VendorID { get; set; }

    ushort ProductID { get; set; }

    ushort VersionID { get; set; }

    ushort bcdUSB { get; set; }

    string? DriverKey { get; set; }

    string? HardwarePath { get; set; }

    string? RegParentID { get; set; }

    string? RegDeviceDesc { get; set; }

    ushort iManufacturer { get; set; }

    ushort iProduct { get; set; }

    ushort iSerialNumber { get; set; }

    string? sManufacturer { get; set; }

    string? sProduct { get; set; }

    string? sSerialNumber { get; set; }

    ICollection<UsbDevice>? LogicalDevices { get; set; }
}
