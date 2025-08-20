namespace MRCustom.UI;

public class ObjectIconSymbol
{
    public FSprite symbolSprite;

    public Color color;

    public float showFlash;
    public float lastShowFlash;

    public void Draw(Vector2 drawPos, float timeStacker)
    {
        float flash = Mathf.Lerp(this.lastShowFlash, this.showFlash, timeStacker);
        if (this.symbolSprite != null)
        {
            this.symbolSprite.color = Color.Lerp(this.color, new Color(1f, 1f, 1f), Mathf.Pow(flash, 3f));
            this.symbolSprite.x = drawPos.x;
            this.symbolSprite.y = drawPos.y;
        }
        /*
        if (this.shadowSprite1 != null)
        {
            this.shadowSprite1.x = drawPos.x - 2f;
            this.shadowSprite1.y = drawPos.y - 1f;
            this.shadowSprite2.x = drawPos.x - 1f;
            this.shadowSprite2.y = drawPos.y + 1f;
        }
        */
    }
}
