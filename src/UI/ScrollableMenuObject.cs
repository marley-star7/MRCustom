namespace MRCustom.UI;

public interface ScrollableMenuObject
{
    public ScrollableBehaviour GetScrollableBehaviour { get; }

    public int GetLastPossibleScrollPos { get; }

    void OnScrollChanged();
}
