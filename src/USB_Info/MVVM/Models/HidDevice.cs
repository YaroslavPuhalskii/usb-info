using System;

namespace USB_Info.MVVM.Models;

public class HidDevice : IEquatable<HidDevice>
{
    public int Id { get; set; }

    public string? VendorId { get; set; }

    public string? ProductId { get; set; }

    public string? Path { get; set; }

    public string? Name { get; set; }

    public HidLibrary.HidDevice? Device { get; set; }

    public string? DevicePath { get; set; }

    public string? DeviceInfornation => $"{Id}. {Name} - VID[{VendorId}], PID[{ProductId}], Path{Path}";

    public bool Equals(HidDevice? other)
    {
        return Name!.Equals(other!.Name)
            && VendorId!.Equals(other.VendorId)
            && ProductId!.Equals(other.ProductId);
    }
}
