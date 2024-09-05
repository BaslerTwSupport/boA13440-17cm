using System.Windows;
using FlatFieldCalibration.Models;

namespace FlatFieldCalibration.ViewModels.Wizards;

public class WizardPageViewModel : ViewModelBase
{
    private string _title, _leftName, _rightName, _description;
    private BaslerCommand? _leftClickedCmd, _rightClickedCmd;
    private Visibility _leftVisibility, _rightVisibility;
    private bool _leftEnabled, _rightEnabled;
    private WizardSidePageViewModelBase _sideView;
    public MainWindowViewModel Main { get; }
    public ImageViewModel ImageView { get; }
    public string Title { get => _title; set => SetProperty(ref _title, value); }
    public string Description { get => _description; set => SetProperty(ref _description, value); }
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
    public bool LeftEnabled { get => _leftEnabled; set => SetProperty(ref _leftEnabled, value); }
    public bool RightEnabled { get => _rightEnabled; set => SetProperty(ref _rightEnabled, value); }
    public WizardSidePageViewModelBase SideView 
    { 
        get => _sideView; 
        set 
        { 
            if(SetProperty(ref _sideView, value))
            {
                Title = value.Title;
                LeftClickedCmd = value.LeftClickedCmd;
                RightClickedCmd = value.RightClickedCmd;
                LeftName = value.LeftName;
                RightName = value.RightName;
                Description = value.Description;
            }
        }
    }
    public WizardPageViewModel(MainWindowViewModel main)
    {
        Main = main;
        _title = _leftName = _rightName = _description = string.Empty;
        _rightClickedCmd = _leftClickedCmd = null;
        _leftVisibility = _rightVisibility = Visibility.Hidden;
        _leftEnabled = _rightEnabled = true;
        ImageView = Main.MainView.ImageView;
        _sideView = new WizardSidePageViewModelBase(this);
        SideView = new WizardTrainDarkImagePageViewModel(this);
    }
}