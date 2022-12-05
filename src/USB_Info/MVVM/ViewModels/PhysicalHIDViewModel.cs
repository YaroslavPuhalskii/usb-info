using CommunityToolkit.Mvvm.ComponentModel;
using USB_Info.Services.Abstractions;

namespace USB_Info.MVVM.ViewModels
{
    internal partial class PhysicalHIDViewModel : ObservableObject
    {
        private readonly IPhysicalHIDService _physicalHIDDeviceService;

        public PhysicalHIDViewModel(IPhysicalHIDService physicalHIDDeviceService)
        {
            _physicalHIDDeviceService = physicalHIDDeviceService;
        }
    }
}
