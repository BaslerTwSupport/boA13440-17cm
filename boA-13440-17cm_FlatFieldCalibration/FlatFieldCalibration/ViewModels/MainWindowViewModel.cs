using FlatFieldCalibration.Models;

namespace FlatFieldCalibration.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public BaslerCommand LoadedCmd { get; }
    public ViewModelBase MainView { get; }
    public ImageViewModel ImageView { get; set; }
    public MainWindowViewModel()
    {
        LoadedCmd = new BaslerCommand(Loaded);
        MainView = new MainViewModel(this);
    }

    private void Loaded()
    {
        
    }

}