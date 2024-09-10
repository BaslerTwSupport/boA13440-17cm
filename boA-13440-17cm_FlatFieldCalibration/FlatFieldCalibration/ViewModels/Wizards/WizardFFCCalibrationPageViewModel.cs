using FlatFieldCalibration.Models;

namespace FlatFieldCalibration.ViewModels.Wizards;

internal class WizardFFCCalibrationPageViewModel : WizardSidePageViewModelBase
{
    public BaslerCommand ExecuteCmd { get; }
    public WizardFFCCalibrationPageViewModel(WizardPageViewModel parent)
        : base(parent)
    {
        Title = "Calibration";
        Description = "1. Press button Calibration.";
        LeftClickedCmd = new BaslerCommand(BackPage);
        LeftName = "Back";
        RightClickedCmd = new BaslerCommand(Exit);
        RightName = "Exit";
        ExecuteCmd = new BaslerCommand(Execute);
    }
    private void BackPage()
    {
        _parent.SideView = new WizardTrainWhiteImagePageViewModel(_parent);
    }
    private void Exit()
    {
        _parent.Main.MainWindowView = _parent.Main.MainView;
        _parent.SideView = new WizardTrainDarkImagePageViewModel(_parent);
    }
    private void Execute()
    {
        
    }
}
