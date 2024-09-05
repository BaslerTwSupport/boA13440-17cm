using System.Windows;
using FlatFieldCalibration.Models;

namespace FlatFieldCalibration.ViewModels.Wizards;

public class WizardPageViewModel : BindableBase
{
    private string _title, _leftName, _rightName;
    private BaslerCommand? _leftClickedCmd, _rightClickedCmd;
    private Visibility _leftVisibility, _rightVisibility;
    private readonly MainWindowViewModel _main;
    private WizardSidePageViewModelBase _sideView;
    public ImageViewModel ImageView { get; }
    public string Title { get => _title; set => SetProperty(ref _title, value); }
    public string LeftName { get => _leftName; set => SetProperty(ref _leftName, value); }
    public string RightName { get => _rightName; set => SetProperty(ref _rightName, value); }
    public BaslerCommand? LeftClickedCmd
    {
        get => _leftClickedCmd;
        set
        {
            SetProperty(ref _leftClickedCmd, value);
            LeftVisibility = value == null ? Visibility.Collapsed : Visibility.Visible;
        }
    }

    public BaslerCommand? RightClickedCmd
    {
        get => _rightClickedCmd;
        set
        {
            SetProperty(ref _rightClickedCmd, value);
            RightVisibility = value == null ? Visibility.Collapsed : Visibility.Visible;
        }
    }
    public Visibility LeftVisibility { get => _leftVisibility; set => SetProperty(ref _leftVisibility, value); }
    public Visibility RightVisibility { get => _rightVisibility; set => SetProperty(ref _rightVisibility, value); }
    private WizardSidePageViewModelBase SideView { get => _sideView; set => SetProperty(ref _sideView, value); }
    public WizardPageViewModel(MainWindowViewModel main)
    {
        _main = main;
        ImageView = ((MainViewModel)_main.MainView).ImageView;
    }
}