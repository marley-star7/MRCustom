namespace MRCustom.UI;

public abstract class ScrollableContainer : RectangularMenuObject, ScrollableMenuObject, Slider.ISliderOwner
{
    /// <summary>
    /// The non-scrolling line that contains all the buttons (if any) for this container.
    /// </summary>
    public class ButtonsLine : RectangularMenuObject
    {
        public new ScrollableContainer owner => (ScrollableContainer)base.owner;

        private FSprite[] _lines;

        /// <summary>
        /// The color of the lines.
        /// </summary>
        public Color color;

        /// <param name="owner"></param>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        public ButtonsLine(ScrollableContainer owner, Vector2 pos, Vector2 size) : base(owner.menu, owner, pos, size)
        {
            var screenPos = ScreenPos;
            // -- Ms7: The side lines are just really stretched pixels lol.
            _lines = new FSprite[2];
            _lines[0] = new FSprite("pixel", true)
            {
                y = screenPos.y,
                scaleY = size.y
            };
            _lines[1] = new FSprite("pixel", true)
            {
                y = screenPos.y,
                scaleY = size.y
            };

            for (int i = 0; i < _lines.Length; i++)
            {
                _lines[i].anchorX = 0f;
                _lines[i].anchorY = 0f;
                _lines[i].color = MenuColorEffect.rgbMediumGrey;
                _lines[i].x = screenPos.x;
                _lines[i].scaleX = size.x;

                this.Container.AddChild(_lines[i]);
            }
        }

        /// <param name="timeStacker"></param>
        public override void GrafUpdate(float timeStacker)
        {
            base.GrafUpdate(timeStacker);

            for (int i = 0; i < _lines.Length; i++)
            {
                _lines[i].scaleY = size.y;
                _lines[i].color = color;
            }
        }

        public override void RemoveSprites()
        {
            base.RemoveSprites();

            for (int i = 0; i < _lines.Length; i++)
            {
                this.Container.RemoveChild(_lines[i]);
            }
        }
    }

    public ScrollableBehaviour scrollableBehav;
    public ScrollableBehaviour GetScrollableBehaviour => scrollableBehav;

    public VerticalSlider scrollSlider;

    public Slider.SliderID sliderID;

    private const float sliderSizeCorrection = -20f; // Ms7: Sliders will not align properly with the size of this container without this adjustment, taken from LevelSelector.

    private float sliderValueCap;
    private float sliderValue;

    private bool sliderPulled;

    public readonly bool sliderRightSide;

    public ScrollButton scrollUpButton;
    public ScrollButton scrollDownButton;

    public abstract int GetScrollButtonDistanceFromBorder { get; }

    public const float SideLinesPixelSizeX = 2;

    private readonly ButtonsLine buttonsLine;
    private readonly FSprite _separator;

    public virtual int GetLastPossibleScrollPos { get; }

    public ScrollableContainer(Menu.Menu menu, MenuObject owner, Vector2 pos, Vector2 size, Slider.SliderID sliderID, bool sliderRightSide = true) : base(menu, owner, pos, size)
    {
        this.scrollableBehav = new ScrollableBehaviour(this);

        this.sliderID = sliderID;
        this.sliderRightSide = sliderRightSide;

        float leftLinesPosX = -(size.x / 2);
        float rightLinesPosX = (size.x / 2);

        float linesPosX;
        float sliderPosX;

        if (sliderRightSide)
        {
            sliderPosX = rightLinesPosX;
            linesPosX = leftLinesPosX;
        }
        else
        {
            sliderPosX = leftLinesPosX;
            linesPosX = rightLinesPosX;
        }

        this._separator = new FSprite("listDivider", true)
        {
            color = MenuColorEffect.rgbDarkGrey,
            anchorX = 0.5f,
            anchorY = 0.5f,
            scaleX = 1.75f,
            x = pos.x,
            y = 0f,
        };
        this.Container.AddChild(this._separator);

        buttonsLine = new ButtonsLine(this, new Vector2(linesPosX, -size.y / 2), new Vector2(2, size.y));
        this.subObjects.Add(buttonsLine);

        scrollSlider = new VerticalSlider(
            this.menu,
            this,
            "SCROLL",
            new Vector2(sliderPosX, -size.y / 2),
            new Vector2(30f, size.y + sliderSizeCorrection),
            sliderID,
            true
        );
        this.subObjects.Add(scrollSlider);

        scrollUpButton = new ScrollButton(
            this.menu,
            this,
            "UP",
            new Vector2(0, (size.y / 2) + GetScrollButtonDistanceFromBorder),
            ScrollButton.Direction.Up
        );
        this.subObjects.Add(scrollUpButton);

        scrollDownButton = new ScrollButton(
            this.menu,
            this,
            "DOWN",
            new Vector2(0, -(size.y / 2) - GetScrollButtonDistanceFromBorder * 2),
            ScrollButton.Direction.Down
        );
        this.subObjects.Add(scrollDownButton);
    }

    public override void Update()
    {
        base.Update();
        scrollableBehav.Update();

        var lastPossibleScrollPos = GetLastPossibleScrollPos;

        // Can scroll entire container by putting mouse over it and useing mouse wheel.
        if (MouseOver && menu.manager.menuesMouseMode && menu.mouseScrollWheelMovement != 0)
        {
            scrollableBehav.AddScroll(menu.mouseScrollWheelMovement);
        }

        // Move the slider alongside the current position if we are moving it.
        sliderValueCap = Custom.LerpAndTick(sliderValueCap, lastPossibleScrollPos, 0.02f, 0.01f);

        if (lastPossibleScrollPos == 0)
        {
            sliderValue = Custom.LerpAndTick(sliderValue, 0.5f, 0.02f, 0.05f);
            scrollSlider.buttonBehav.greyedOut = true;
        }
        else
        {
            scrollSlider.buttonBehav.greyedOut = false;

            if (sliderPulled)
            {
                scrollableBehav.floatScrollPos = Mathf.Lerp(0f, sliderValueCap, sliderValue);
                scrollableBehav.ScrollPos = Custom.IntClamp(Mathf.RoundToInt(scrollableBehav.floatScrollPos), 0, lastPossibleScrollPos);
                sliderPulled = false;
                return;
            }

            sliderValue = Custom.LerpAndTick(sliderValue, Mathf.InverseLerp(0f, sliderValueCap, scrollableBehav.floatScrollPos), 0.02f, 0.05f);
        }
    }

    public override void GrafUpdate(float timeStacker)
    {
        base.GrafUpdate(timeStacker);

        // -- Ms7: Side lines mimic scroll slider color
        // (but you already knew that, because you read the code didn't you? pretty damn obvious what it does so just look at it... thank you for looking :] )
        buttonsLine.color = scrollSlider.MyColor(timeStacker);
    }

    public override void Singal(MenuObject sender, string message)
    {
        base.Singal(sender, message);
        if (message == "UP")
        {
            scrollableBehav.AddScroll(-1);
            return;
        }
        if (message == "DOWN")
        {
            scrollableBehav.AddScroll(1);
            return;
        }
    }

    protected void DisableScrolling()
    {
        scrollUpButton.inactive = true;
        scrollDownButton.inactive = true;
        scrollSlider.inactive = true;
    }

    protected void EnableScrolling()
    {
        scrollUpButton.inactive = false;
        scrollDownButton.inactive = false;
        scrollSlider.inactive = false;
    }

    public void OnScrollChanged()
    {
        var lastPossibleScrollPos = GetLastPossibleScrollPos;

        // If there are not enough items to scroll, then disable the scroll buttons and slider.
        if (lastPossibleScrollPos == 0)
        {
            scrollUpButton.buttonBehav.greyedOut = true;
            scrollDownButton.buttonBehav.greyedOut = true;
            scrollSlider.buttonBehav.greyedOut = true;

            return;
        }
        else
        {
            scrollSlider.buttonBehav.greyedOut = false;
        }

        // If we hit the limit on scroll in either direction, disable their respective scroll buttons.
        if (scrollableBehav.ScrollPos == 0)
        {
            scrollUpButton.buttonBehav.greyedOut = true;
        }
        else
        {
            scrollUpButton.buttonBehav.greyedOut = false;

            if (scrollableBehav.ScrollPos == lastPossibleScrollPos)
            {
                scrollDownButton.buttonBehav.greyedOut = true;
            }
            else
            {
                scrollDownButton.buttonBehav.greyedOut = false;
            }
        }
    }

    public float ValueOfSlider(Slider slider)
    {
        return 1f - this.sliderValue;
    }

    public void SliderSetValue(Slider slider, float setValue)
    {
        this.sliderValue = 1f - setValue;
        this.sliderPulled = true;
    }
}