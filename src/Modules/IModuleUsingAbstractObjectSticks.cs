namespace MRCustom.Modules;

// TODO: actually hook in somewhere to make it run this function on realize after the other recreations.

/// <summary>
/// Having this interface on a module will allow it to load abstractObjectStick data from owner on realize.
/// </summary>
public interface IModuleUsingAbstractObjectSticks
{
    public abstract void RecreateSticksFromAbstract();
}
