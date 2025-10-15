namespace MRCustom.Modules;

public abstract class RWModule
{
    protected UpdatableAndDeletable _owner;
    public UpdatableAndDeletable Owner
    {
        get => _owner; 
        set => SetOwner(value);
    }

    public Type moduleType;

    public RWModule(UpdatableAndDeletable owner, Type moduleType)
    {
        this._owner = owner;
        this.moduleType = moduleType;
    }

    public void SetOwner(UpdatableAndDeletable newOwner)
    {
        _owner = newOwner;
    }
}
