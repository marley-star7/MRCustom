using System;
using System.Drawing;

namespace MRCustom.UI;

public abstract class ScrollableItemList<ItemType> : ScrollableContainer where ItemType : ScrollableItemList<ItemType>.Item
{
    /// <summary>
    /// An individual item in a scrollable item container
    /// </summary>
    public class Item : RectangularMenuObject
    {
        public new ScrollableItemList<ItemType> owner => (ScrollableItemList<ItemType>)base.owner;

        protected float fade;
        protected float lastFade;

        public int index;

        public Item(ScrollableItemList<ItemType> owner, int index) : base(owner.menu, owner, Vector2.zero, new Vector2(owner.size.x, owner.GetSingleItemHeight))
        {
            this.index = index;
        }

        public override void Update()
        {
            base.Update();

            lastFade = fade;

            float transparency = 1f;
            var ownerFloatScrollPos = owner.scrollableBehav.floatScrollPos;

            var thisItemFloatScrollPos = pos.y - owner.size.y / 2f;

            if (index < ownerFloatScrollPos)
            {
                transparency = Mathf.InverseLerp(ownerFloatScrollPos - 1f, ownerFloatScrollPos, thisItemFloatScrollPos);
            }
            else if (index > ownerFloatScrollPos + owner.MaxVisibleItems - 1f)
            {
                transparency = Mathf.InverseLerp(ownerFloatScrollPos + owner.MaxVisibleItems - 1f, ownerFloatScrollPos + owner.MaxVisibleItems, thisItemFloatScrollPos);
            }
            transparency = Mathf.InverseLerp(-1, 0, -transparency); // Swap transparency from 0 to 1 to 1 to 0 calculation

            fade = Custom.LerpAndTick(fade, transparency, 0.08f, 0.1f);
        }

        public override void GrafUpdate(float timeStacker)
        {
            base.GrafUpdate(timeStacker);
        }
    }

    public List<Item> items = new();

    public int TotalItems
    {
        get => items.Count;
    }

    public int MaxVisibleItems
    {
        get
        {
            return (int)(size.y / GetSingleItemHeight);
        }
    }

    public override int GetLastPossibleScrollPos
    {
        get => System.Math.Max(0, items.Count - 1 - (MaxVisibleItems - 1));
    }

    public override int GetScrollButtonDistanceFromBorder => 10; // Same as LevelSelector's pos

    public abstract float GetSingleItemHeight { get; }

    public ScrollableItemList(Menu.Menu menu, MenuObject owner, Vector2 pos, Vector2 size, Slider.SliderID sliderID, bool sliderRightSide = true) : base(menu, owner, pos, size, sliderID, sliderRightSide)
    {

    }

    public override void Update()
    {
        base.Update();

        for (int i = 0; i < items.Count; i++)
        {
            items[i].pos.y = GetIdealYPosForItem(items[i], i);
        }
    }

    protected void AddItem(ItemType item)
    {
        item.pos = GetIdealPosForItem(item, items.Count);

        this.items.Add(item);
        this.subObjects.Add(item);
        PostAddItem(item);
    }

    /// <summary>
    /// Occurs on the addition of a new item.
    /// </summary>
    /// <param name="item"></param>
    protected virtual void PostAddItem(ItemType item)
    {

    }

    public void ClearItems()
    {
        foreach (var item in items)
        {
            item.RemoveSprites();
        }
        subObjects.RemoveAll(item => item is Item);
        items.Clear();
    }

    public void ReorderItems(List<ItemType> newOrder)
    {
        ClearItems();

        // Re-add items in the new order
        for (int i = 0; i < newOrder.Count; i++)
        {
            AddItem(newOrder[i]);
            items[i].pos.y = GetIdealYPosForItem(items[i], i);
        }

        scrollableBehav.ResetScrollPos();
    }

    /// <summary>
    /// Find how many steps
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    /*
    public float GetStepsDownOfItem(int itemIndex)
    {
        float pos = 0f;
        for (int i = 0; i <= System.Math.Min(itemIndex, this.items.Count - 1); i++)
        {
            pos += 1f;
        }
        return pos;
    }
    */

    protected Vector2 GetIdealPosForItem(in Item item, int itemIndex)
    {
        return new Vector2(
            GetIdealXPosForItem(item),
            GetIdealYPosForItem(item, itemIndex)
            );
    }

    protected float GetIdealXPosForItem(in Item item)
    {
        return size.x / 2f - item.size.x;
    }

    protected float GetIdealYPosForItem(in Item item, int itemIndex)
    {
        var singleItemHeight = GetSingleItemHeight;

        float posY = size.y / 2f - item.size.y / 2f; // Top of list
        posY -= itemIndex * singleItemHeight;

        posY += scrollableBehav.floatScrollPos * singleItemHeight;

        return posY;
    }
}
