using FlatFieldCalibration.Models;

namespace FlatFieldCalibration.ViewModels;

public class MainViewModel : ViewModelBase
{
    public ImageViewModel ImageView { get; }
    public BaslerCommand LoadedCmd { get; }
    public BaslerCommand FfcCmd { get; }
    public MainViewModel(MainWindowViewModel main)
    {
        LoadedCmd = new BaslerCommand(Loaded);
        FfcCmd = new BaslerCommand(() => main.MainWindowView = main.Wizards);
        ImageView = new ImageViewModel();
        main.ImageView = ImageView;
    }
    private void Loaded()
    {

    }
}