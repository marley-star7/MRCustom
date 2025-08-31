namespace MRCustom.UI;

/// <summary>
/// A scroll button element similar to the ones used in the LevelSelector in Arena.
/// </summary>
public class ScrollButton : SymbolButton
{
    //-- Ms7: The values here are important, it is part of the math for rotating.
    public enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3,
    }

    public Direction direction;

    private int heldCounter;

    public ScrollButton(Menu.Menu menu, MenuObject owner, string singalText, Vector2 pos, Direction direction) : base(menu, owner, "Menu_Symbol_Arrow", singalText, pos)
    {
        this.direction = direction;
    }

    public override void Update()
    {
        base.Update();

        if (this.buttonBehav.clicked && !this.buttonBehav.greyedOut)
        {
            this.heldCounter++;
            if (this.heldCounter > 20 && this.heldCounter % 4 == 0)
            {
                this.menu.PlaySound(SoundID.MENU_Scroll_Tick);
                this.Singal(this, this.signalText);
                this.buttonBehav.sin = 0.5f;
                return;
            }
        }
        else
        {
            this.heldCounter = 0;
        }
    }

    public override void GrafUpdate(float timeStacker)
    {
        base.GrafUpdate(timeStacker);
        this.symbolSprite.rotation = 90f * (float)direction;
    }

    public override void Clicked()
    {
        if (this.heldCounter < 20)
        {
            this.menu.PlaySound(SoundID.MENU_First_Scroll_Tick);
            this.Singal(this, this.signalText);
        }
    }
}