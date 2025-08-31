namespace MRCustom.UI;

/// <summary>
/// A class that runs the scrolling logic.
/// Make sure to run Update() in your ScrollableMenuObject;
/// </summary>
public class ScrollableBehaviour
{
    public readonly ScrollableMenuObject owner;

    private int _scrollPos = 0;
    public int ScrollPos
    {
        get => _scrollPos;
        set
        {
            _scrollPos = value;
            OnScrollChanged();
        }
    }

    public float floatScrollPos;
    public float floatScrollVel;

    public ScrollableBehaviour(ScrollableMenuObject owner)
    {
        this.owner = owner;
    }

    public void AddScroll(int scrollDir)
    {
        ScrollPos += scrollDir;
        owner.OnScrollChanged();
    }

    private void ConstrainScroll()
    {
        if (ScrollPos > owner.GetLastPossibleScrollPos)
        {
            ScrollPos = owner.GetLastPossibleScrollPos;
        }
        if (ScrollPos < 0)
        {
            ScrollPos = 0;
        }
    }

    /// <summary>
    /// Resets the ScrollPos, floatScrollPos, and floatScrollVel to 0.
    /// </summary>
    public void ResetScrollPos()
    {
        ScrollPos = 0;
        floatScrollPos = 0;
        floatScrollVel = 0;
    }

    /// <summary>
    /// Occurs whenever ScrollPos is updated, NOT floatScrollPos.
    /// </summary>
    private void OnScrollChanged()
    {
        ConstrainScroll();
        owner.OnScrollChanged();
    }

    public void Update()
    {
        float settleScrollPos = ScrollPos;

        //-- Ms7: I took this code below from source for LevelSelector, but what the actual fuck is it doing?? what is this math??? how does it work????
        // Damn arbitrary number heaven...
        /*
        if (ScrollPos > 0 
            && ScrollPos == Math.Max(0, craftRecipesItems.Count - (MaxVisibleItems - 1)))
        {
            for (int j = ScrollPos; j < craftRecipesItems.Count; j++)
            {
                settleScrollPos -= craftRecipesItems[j].fadeAway;
            }
        }
        */

        floatScrollPos = Custom.LerpAndTick(floatScrollPos, settleScrollPos, 0.01f, 0.01f); // Lerp and tick for smooth movement, but making sure it still moves I assume.

        floatScrollVel *= Custom.LerpMap(System.Math.Abs(settleScrollPos - floatScrollPos), 0.25f, 1.5f, 0.45f, 0.99f); // I have no idea what the hell LerpMap is still.
        floatScrollVel += Mathf.Clamp(settleScrollPos - floatScrollPos, -2.5f, 2.5f) / 2.5f * 0.15f; // ???
        floatScrollVel = Mathf.Clamp(floatScrollVel, -1.2f, 1.2f); // ???!!!

        floatScrollPos += floatScrollVel;
    }
}
