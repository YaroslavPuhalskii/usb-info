namespace USB_Info.Core.Abstractions;

internal interface IUsbDeviceFactory
{
    IUsbDevice Create();
}
