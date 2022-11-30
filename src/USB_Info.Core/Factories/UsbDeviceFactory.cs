using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using USB_Info.Core.Abstractions;
using USB_Info.Core.Extensions;
using USB_Info.Core.Helpers;
using USB_Info.Core.Models;
using USB_Info.Core.Natives;
using USB_Info.Core.Natives.Types;
using static USB_Info.Core.Natives.NativeMethods;

namespace USB_Info.Core.Factories;

internal class UsbDeviceFactory : IUsbDeviceFactory
{
    private readonly SafeFileHandle _handle;

    private readonly USB_NODE_CONNECTION_INFORMATION_EX _usbConnecInformation;

    private readonly uint _portIndex;

    private readonly ushort _hubIndex;

    public UsbDeviceFactory(SafeFileHandle handle,
        USB_NODE_CONNECTION_INFORMATION_EX usbConnectinformation,
        uint portIndex,
        ushort hubIndex)
    {
        _handle = handle;
        _usbConnecInformation = usbConnectinformation;
        _portIndex = portIndex;
        _hubIndex = hubIndex;
    }

    [SupportedOSPlatform("windows")]
    public IUsbDevice Create()
    {
        var driverKey = DeviceIoControlInvoker.Invoke(
                _handle, IOCTL_USB_GET_NODE_CONNECTION_DRIVERKEY_NAME,
                new USB_NODE_CONNECTION_DRIVERKEY_NAME() { ConnectionIndex = _portIndex }).DriverKeyName;

        IConnectedUsbDevice device = new UsbDevice(_hubIndex,
            _portIndex,
            _usbConnecInformation.DeviceDescriptor.idVendor,
            _usbConnecInformation.DeviceDescriptor.idProduct,
            _usbConnecInformation.DeviceDescriptor.bcdDevice,
            _usbConnecInformation.DeviceDescriptor.bcdUSB,
            driverKey,
            $"Hub{_hubIndex.HubPortNumber()}Port{_portIndex.PortNumber()}",
            GetDeviceConnectionStatus(_usbConnecInformation.ConnectionStatus),
            _usbConnecInformation.DeviceDescriptor.iManufacturer,
            _usbConnecInformation.DeviceDescriptor.iProduct,
            _usbConnecInformation.DeviceDescriptor.iSerialNumber,
            GetManufacturer(),
            GetProduct(),
            GetSerialNumber());

        GetDeviceRegistryDescription(device);
        EnumerateLogicalDevices(device.RegParentID!, device, device);

        return device;
    }

    [SupportedOSPlatform("windows")]
    public void GetDeviceRegistryDescription(IConnectedUsbDevice device)
    {
        if (string.IsNullOrEmpty(device.DriverKey))
            return;

        using var usbRegistryKey = Registry.LocalMachine.OpenSubKey(PathHelper.UsbRegistryPath, false);
        var usbRegistrySubKeys = usbRegistryKey!.GetSubKeyNames();

        foreach (var usbRegistrySubKey in usbRegistrySubKeys)
        {
            using var usbRegistryDeviceKey = Registry.LocalMachine.OpenSubKey(@$"{PathHelper.UsbRegistryPath}\{usbRegistrySubKey}", false);
            var usbRegistryDeviceSubKeys = usbRegistryDeviceKey!.GetSubKeyNames();

            foreach (var usbRegistryDeviceSubKey in usbRegistryDeviceSubKeys)
            {
                using var usbRegistryInstanceKey =
                    Registry.LocalMachine.OpenSubKey(@$"{PathHelper.UsbRegistryPath}\{usbRegistrySubKey}\{usbRegistryDeviceSubKey}", false);
                if (!device.DriverKey.ToUpper().Contains(usbRegistryInstanceKey!.GetValue("Driver")!.ToString()!.ToUpper()))
                    continue;

                device.HardwarePath = @$"USB\{usbRegistrySubKey}\{usbRegistryDeviceSubKey}";
                device.RegDeviceDesc = !string.IsNullOrEmpty(usbRegistryInstanceKey.GetValue("DeviceDesc")!.ToString()) ?
                            usbRegistryInstanceKey.GetValue("DeviceDesc")!.ToString()!.Split(';')[1]
                            : string.Empty;
                device.RegParentID = !string.IsNullOrEmpty(usbRegistryInstanceKey.GetValue("ParentIdPrefix")!.ToString()) ?
                    usbRegistryInstanceKey.GetValue("ParentIdPrefix")!.ToString()
                    : string.Empty;
            }
        }
    }


    [SupportedOSPlatform("windows")]
    private void EnumerateLogicalDevices(string regParentId, IConnectedUsbDevice parentDevice, IConnectedUsbDevice childDevice)
    {
        if (string.IsNullOrEmpty(regParentId))
            return;

        try
        {
            using var enumRegistryKey = Registry.LocalMachine.OpenSubKey(PathHelper.EnumRegistryPath)!;
            var usbSubKeys = enumRegistryKey.GetSubKeyNames();

            foreach (var usbSubKey in usbSubKeys)
            {
                using var usbDevice = Registry.LocalMachine.OpenSubKey(@$"{PathHelper.EnumRegistryPath}\{usbSubKey}")!;
                var usbInformation = usbDevice.GetSubKeyNames();

                foreach (var usbInform in usbInformation)
                {
                    using var d = Registry.LocalMachine.OpenSubKey(@$"{PathHelper.EnumRegistryPath}\{usbSubKey}\{usbInform}");
                    var dd = d!.GetSubKeyNames();

                    foreach (var ddd in dd)
                    {
                        if (ddd.ToUpper().Contains(regParentId.ToUpper()))
                        {
                            using var c = Registry.LocalMachine.OpenSubKey(@$"{PathHelper.EnumRegistryPath}\{usbSubKey}\{usbInform}\{ddd}");
                            var logicalDevice = new UsbDevice
                            {
                                ConnectionName = childDevice!.ConnectionName,
                                ConnectionStat = childDevice.ConnectionStat,
                                Port = childDevice.Port,
                                HubIndex = childDevice.HubIndex,
                                VendorID = childDevice.VendorID,
                                ProductID = childDevice.ProductID,
                                VersionID = childDevice.VersionID,
                                bcdUSB = childDevice.bcdUSB,
                                iManufacturer = childDevice.iManufacturer,
                                iProduct = childDevice.iProduct,
                                iSerialNumber = childDevice.iSerialNumber,
                                sManufacturer = childDevice.ConnectionName,
                                sSerialNumber = childDevice.ConnectionName,
                                sProduct = childDevice.ConnectionName,
                            };
                            logicalDevice.HardwarePath = @$"{usbSubKey}\{usbInform}\{ddd}";
                            logicalDevice.RegDeviceDesc = c!.GetValueNames().Contains("DeviceDesc")! ?
                                c.GetValue("DeviceDesc")!.ToString()!.Split(';')[1]
                                : string.Empty;
                            logicalDevice.RegParentID = c.GetValueNames().Contains("ParentIdPrefix") ?
                                c.GetValue("ParentIdPrefix")!.ToString()
                                : string.Empty;

                            logicalDevice.DriverKey = c.GetValueNames().Contains("Driver")! ?
                                c.GetValue("Driver")!.ToString()
                                : string.Empty;

                            parentDevice.LogicalDevices!.Add(logicalDevice);
                            EnumerateLogicalDevices(logicalDevice.RegParentID!, parentDevice, logicalDevice);
                        }
                    }
                }
            }
        }
        catch (Exception)
        {
            return;
        }
    }

    private string GetSerialNumber()
    {
        if (_usbConnecInformation.DeviceDescriptor.iSerialNumber <= 0)
            return string.Empty;

        var request = new USB_DESCRIPTOR_REQUEST
        {
            ConnectionIndex = _portIndex,
            SetupPacket =
        {
            wValue = (short) ((USB_STRING_DESCRIPTOR_TYPE << 8) +
            _usbConnecInformation.DeviceDescriptor.iSerialNumber)
        }
        };
        request.SetupPacket.wLength = (short)Marshal.SizeOf(request);

        request.SetupPacket.wIndex = 0x409;

        try
        {
            var usbDescriptorRequest = DeviceIoControlInvoker.Invoke(
                _handle, IOCTL_USB_GET_DESCRIPTOR_FROM_NODE_CONNECTION, request);

            return usbDescriptorRequest.StringDescriptor.bString;
        }
        catch
        {
            return string.Empty;
        }
    }

    private string GetManufacturer()
    {
        if (_usbConnecInformation.DeviceDescriptor.iManufacturer <= 0)
            return string.Empty;

        var request = new USB_DESCRIPTOR_REQUEST
        {
            ConnectionIndex = _portIndex,
            SetupPacket =
        {
            wValue = (short) ((USB_STRING_DESCRIPTOR_TYPE << 8) +
            _usbConnecInformation.DeviceDescriptor.iManufacturer)
        }
        };
        request.SetupPacket.wLength = (short)Marshal.SizeOf(request);

        request.SetupPacket.wIndex = 0x409;

        try
        {
            var usbDescriptorRequest = DeviceIoControlInvoker.Invoke(
                _handle, IOCTL_USB_GET_DESCRIPTOR_FROM_NODE_CONNECTION, request);

            return usbDescriptorRequest.StringDescriptor.bString;
        }
        catch
        {
            return string.Empty;
        }
    }

    private string GetProduct()
    {
        if (_usbConnecInformation.DeviceDescriptor.iProduct <= 0)
            return string.Empty;

        var request = new USB_DESCRIPTOR_REQUEST
        {
            ConnectionIndex = _portIndex,
            SetupPacket =
        {
            wValue = (short) ((USB_STRING_DESCRIPTOR_TYPE << 8) +
            _usbConnecInformation.DeviceDescriptor.iProduct)
        }
        };
        request.SetupPacket.wLength = (short)Marshal.SizeOf(request);

        request.SetupPacket.wIndex = 0x409;

        try
        {
            var usbDescriptorRequest = DeviceIoControlInvoker.Invoke(
                _handle, IOCTL_USB_GET_DESCRIPTOR_FROM_NODE_CONNECTION, request);

            return usbDescriptorRequest.StringDescriptor.bString;
        }
        catch
        {
            return string.Empty;
        }
    }
}