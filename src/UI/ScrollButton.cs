namespace MRCustom.UI;

public class ScrollButton : SymbolButton
{
    public enum Direction
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
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
        this.symbolSprite.rotation = 90f * (float)this.direction;
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