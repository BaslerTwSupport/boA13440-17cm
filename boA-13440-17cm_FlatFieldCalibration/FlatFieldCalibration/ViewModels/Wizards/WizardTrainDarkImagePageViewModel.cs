using FlatFieldCalibration.Models;

namespace FlatFieldCalibration.ViewModels.Wizards;
internal class WizardTrainDarkImagePageViewModel : WizardSidePageViewModelBase
{
    public WizardTrainDarkImagePageViewModel(WizardPageViewModel parent) 
        : base(parent)
    {
        Title = "Train Dark Image";
        Description = "1. Cover lens\r\n2. Press button Save image.";
        RightClickedCmd = new BaslerCommand(NextPage);
        RightName = "Next";
    }
    private void NextPage()
    {
        _parent.SideView = new WizardTrainWhiteImagePageViewModel(_parent);
    }
}