namespace MRCustom.Modules.PhysicalObjects;

public abstract class PhysicalObjectModule : RWModule
{
    public new PhysicalObject Owner => (PhysicalObject)base.Owner;

    public PhysicalObjectModule(PhysicalObject owner, Type moduleType) : base(owner, moduleType)
    {

    }
}