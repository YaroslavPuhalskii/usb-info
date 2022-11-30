using USB_Info.Core.Abstractions;

namespace USB_Info.Core.Models;

public class UsbDevice : IConnectedUsbDevice
{
    public ushort HubIndex { get; set; }

    public uint Port { get; set; }

    public ushort VendorID { get; set; }

    public ushort ProductID { get; set; }

    public ushort VersionID { get; set; }

    public ushort bcdUSB { get; set; }

    public string? DriverKey { get; set; }

    public string? ConnectionName { get; set; }

    public string? ConnectionStat { get; set; }

    public string? HardwarePath { get; set; }

    public string? RegParentID { get; set; }

    public string? RegDeviceDesc { get; set; }

    public ushort iManufacturer { get; set; }

    public ushort iProduct { get; set; }

    public ushort iSerialNumber { get; set; }

    public string? sManufacturer { get; set; }

    public string? sProduct { get; set; }

    public string? sSerialNumber { get; set; }

    public ICollection<UsbDevice>? LogicalDevices { get; set; } = new List<UsbDevice>();

    public UsbDevice(ushort hubIndex,
        uint port,
        ushort vendorId,
        ushort productId,
        ushort versionId,
        ushort bcdUsb,
        string driverKey,
        string connectionName,
        string connectionStat,
        ushort imanufacturer,
        ushort iproduct,
        ushort iserialNumber,
        string smanufacturer,
        string sproduct,
        string sserialNumber)
    {
        HubIndex = hubIndex;
        Port = port;
        VendorID = vendorId;
        ProductID = productId;
        VersionID = versionId;
        bcdUSB = bcdUsb;
        DriverKey = driverKey;
        ConnectionName = connectionName;
        ConnectionStat = connectionStat;
        iManufacturer = imanufacturer;
        iProduct = iproduct;
        iSerialNumber = iserialNumber;
        sManufacturer = smanufacturer;
        sProduct = sproduct;
        sSerialNumber = sserialNumber;
    }

    public UsbDevice()
    { }
}