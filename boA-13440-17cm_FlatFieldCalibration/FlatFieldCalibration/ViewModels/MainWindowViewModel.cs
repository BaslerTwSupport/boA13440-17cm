using FlatFieldCalibration.Models;
using FlatFieldCalibration.ViewModels.Wizards;

namespace FlatFieldCalibration.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase _mainWindowView;
    public BaslerCommand LoadedCmd { get; }
    public ViewModelBase MainWindowView
    {
        get => _mainWindowView;
        set => SetProperty(ref _mainWindowView, value);
    }
    public WizardPageViewModel Wizards { get; }
    public MainViewModel MainView { get; }
    public ImageViewModel ImageView { get; set; }
    public MainWindowViewModel()
    {
        LoadedCmd = new BaslerCommand(Loaded);
        MainView = new MainViewModel(this);
        Wizards = new WizardPageViewModel(this);
        MainWindowView = Wizards;
    }

    private void Loaded()
    {
        
    }

}