using FlatFieldCalibration.Models;

namespace FlatFieldCalibration.ViewModels.Wizards;

internal class WizardFFCCalibrationPageViewModel : WizardSidePageViewModelBase
{
    public WizardFFCCalibrationPageViewModel(WizardPageViewModel parent)
        : base(parent)
    {
        LeftClickedCmd = new BaslerCommand(BackPage);
        LeftName = "Back";
        RightClickedCmd = new BaslerCommand(Exit);
        RightName = "Exit";
    }
    private void BackPage()
    {
        _parent.SideView = new WizardTrainWhiteImagePageViewModel(_parent);
    }
    private void Exit()
    {
        
    }
}
