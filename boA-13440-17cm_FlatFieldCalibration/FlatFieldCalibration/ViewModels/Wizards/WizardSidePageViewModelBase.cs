using FlatFieldCalibration.Models;

namespace FlatFieldCalibration.ViewModels.Wizards;

public class WizardSidePageViewModelBase : ViewModelBase
{
    protected WizardPageViewModel _parent;
    public string Title { get; protected set; }
    public string RightName { get; protected set; }
    public string LeftName { get; protected set; }
    public BaslerCommand? LeftClickedCmd { get; protected set; }
    public BaslerCommand? RightClickedCmd { get; protected set; }
    public string Description { get; protected set; }
    public WizardSidePageViewModelBase(WizardPageViewModel parent)
    {
        Description = LeftName = RightName = Title = string.Empty;
        LeftClickedCmd = null;
        RightClickedCmd = null;
        _parent = parent;
    }
}