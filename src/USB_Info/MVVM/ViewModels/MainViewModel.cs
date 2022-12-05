using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using USB_Info.Services.Abstractions;

namespace USB_Info.MVVM.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableObject? currentViewModel;

        [ObservableProperty]
        public bool isChecked;

        private readonly ILogicalHIDService _logicalHIDService;

        private readonly IPhysicalHIDService _physicalHIDService;

        public MainViewModel(ILogicalHIDService logicalHIDDeviceService,
            IPhysicalHIDService physicalHIDDeviceService)
        {
            _logicalHIDService = logicalHIDDeviceService;
            _physicalHIDService = physicalHIDDeviceService;
        }

        [RelayCommand]
        public void ChangeView()
            => CurrentViewModel = IsChecked ? new PhysicalHIDViewModel(_physicalHIDService) : new LogicalHIDViewModel(_logicalHIDService);
    }
}
