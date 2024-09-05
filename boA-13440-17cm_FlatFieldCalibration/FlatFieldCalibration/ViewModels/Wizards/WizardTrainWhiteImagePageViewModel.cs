using FlatFieldCalibration.Models;

namespace FlatFieldCalibration.ViewModels.Wizards;
internal class WizardTrainWhiteImagePageViewModel : WizardSidePageViewModelBase
{
    public WizardTrainWhiteImagePageViewModel(WizardPageViewModel parent) 
        : base(parent)
    {
        Title = "Train White Image";
        Description = "1. Adjust gray values close to 170\r\n2. Press button Save image.";
        LeftClickedCmd = new BaslerCommand(BackPage);
        LeftName = "Back";
        RightClickedCmd = new BaslerCommand(NextPage);
        RightName = "Next";
    }
    private void BackPage()
    {
        _parent.SideView = new WizardTrainDarkImagePageViewModel(_parent);
    }
    private void NextPage()
    {
        _parent.Main.MainWindowView = _parent.Main.MainView;
    }
}