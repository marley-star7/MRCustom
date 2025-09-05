/*
namespace MRCustom.UI;

public class ObjectIconSymbol
{
    public FContainer container;

    public FSprite symbolSprite;

    public string spriteName;

    public Color color;

    public float showFlash;
    public float lastShowFlash;

    public Vector2 pos;

    public ObjectIconSymbol(ObjectIconSymbolProperties objectIconSymbolProperties, FContainer container)
    {
        this.spriteName = objectIconSymbolProperties.spriteName;
        this.container = container;
    }

    public void Show()
    {
        /*
        if (showShadowSprites)
        {
            this.shadowSprite1 = new FSprite(this.spriteName, true);
            this.container.AddChild(this.shadowSprite1);
            this.shadowSprite1.color = new Color(0f, 0f, 0f);
            this.shadowSprite2 = new FSprite(this.spriteName, true);
            this.container.AddChild(this.shadowSprite2);
            this.shadowSprite2.color = new Color(0f, 0f, 0f);
        }
        this.symbolSprite = new FSprite(this.spriteName, true);
        this.container.AddChild(this.symbolSprite);
        this.showFlash = 1f;
        this.lastShowFlash = 1f;
    }

    public void RemoveSprites()
    {
        if (this.symbolSprite != null)
        {
            this.symbolSprite.RemoveFromContainer();
        }
        /*
        if (this.shadowSprite1 != null)
        {
            this.shadowSprite1.RemoveFromContainer();
        }
        if (this.shadowSprite2 != null)
        {
            this.shadowSprite2.RemoveFromContainer();
        }
    }

    public void Update()
    {
        this.lastShowFlash = this.showFlash;
        this.showFlash = Custom.LerpAndTick(this.showFlash, 0f, 0.08f, 0.1f);
    }

    public void Draw(float timeStacker)
    {
        float flash = Mathf.Lerp(this.lastShowFlash, this.showFlash, timeStacker);
        if (this.symbolSprite != null)
        {
            this.symbolSprite.color = Color.Lerp(this.color, new Color(1f, 1f, 1f), Mathf.Pow(flash, 3f));
            this.symbolSprite.x = pos.x;
            this.symbolSprite.y = pos.y;
        }
        /*
        if (this.shadowSprite1 != null)
        {
            this.shadowSprite1.x = drawPos.x - 2f;
            this.shadowSprite1.y = drawPos.y - 1f;
            this.shadowSprite2.x = drawPos.x - 1f;
            this.shadowSprite2.y = drawPos.y + 1f;
        }
    }
}
*/