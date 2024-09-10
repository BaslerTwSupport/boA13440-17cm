using FlatFieldCalibration.Models;

namespace FlatFieldCalibration.ViewModels.Wizards;
internal class WizardTrainDarkImagePageViewModel : WizardSidePageViewModelBase
{
    public BaslerCommand SaveImgCmd { get; }
    public WizardTrainDarkImagePageViewModel(WizardPageViewModel parent) 
        : base(parent)
    {
        Title = "Train Dark Image";
        Description = "1. Cover lens\r\n2. Press button Save image.";
        RightClickedCmd = new BaslerCommand(NextPage);
        RightName = "Next";
        SaveImgCmd = new BaslerCommand(SaveImage);
    }
    private void NextPage()
    {
        _parent.SideView = new WizardTrainWhiteImagePageViewModel(_parent);
    }
    private void SaveImage()
    {
        
    }
}