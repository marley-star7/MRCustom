//-- MS7: Made this to share the code of lizard color graphics calculations of-
// durability/health changing color, being hit causing flickering, etc.

namespace MRCustom;

/// <summary>
/// A class for managing the color graphics of a lizard shell.
/// Make sure to run `Update()` every object's update, and `DrawSpritesUpdate()` every object's DrawSprites.
/// Mostly copied to be identical to source in how lizard flickering appears.
/// </summary>
public class LizardEffectColorGraphics
{
    private const int sourceCodeLizardsFlickerThreshold = 10;
    private const int sourceCodeLizardsWhiteFlickerThreshold = 15;

    public RoomPalette palette;
    public Color rawColor;

    /// <summary>
    /// Brightness value from 0 to 1,
    /// 0 is black, 1 is full raw color.
    /// </summary>
    public float brightness;

    public int whiteFlicker = 0;
    public int flicker;
    private float flickerColor = 0;

    public LizardEffectColorGraphics(Color rawColor)
    {
        this.rawColor = rawColor;
    }

    public void Update()
    {
        if (flicker > 0)
            flicker--;

        if (whiteFlicker > 0)
            whiteFlicker--;
    }

    public void DrawSpritesUpdate()
    {
        //-- flicker code stolen from source.
        if (flicker > 0)
        {
            flickerColor = Random.value;
        }
    }

    public void ApplyPalette(RoomPalette palette)
    {
        this.palette = palette;
    }

    //-- MS7: Copied from source "HeadColor1".
    private Color EffectColor1
    {
        get
        {
            /*
            if (abstractLizardShell.templateType == CreatureTemplate.Type.WhiteLizard)
            {
                return Color.Lerp(new Color(1f, 1f, 1f), whiteCamoColor, whiteCamoColorAmount);
            }
            if (abstractLizardShell.templateType == CreatureTemplate.Type.Salamander)
            {
                return SalamanderColor;
            }
            if (snowAccCosmetic != null)
            {
                return Color.Lerp(palette.blackColor, effectColor, Mathf.Min(1f, snowAccCosmetic.DebrisSaturation * 1.5f));
            }
            */
            return palette.blackColor;
        }
    }

    //-- MS7: Copied from source "HeadColor2".
    private Color EffectColor2
    {
        get
        {
            /*
            if abstractLizardShell.templateType == CreatureTemplate.Type.WhiteLizard)
            {
                return Color.Lerp(palette.blackColor, whiteCamoColor, whiteCamoColorAmount);
            }
            */
            return rawColor;
        }
    }

    //-- MS7: Copied from source "HeadColor".
    public Color ShellColor()
    {
        if (whiteFlicker > 0)
        {
            return new Color(1f, 1f, 1f);
        }
        if (flicker > sourceCodeLizardsFlickerThreshold)
        {
            brightness = flickerColor;
        }
        return Color.Lerp(EffectColor1, EffectColor2, brightness);
    }

    public void Flicker()
    {
        if (sourceCodeLizardsFlickerThreshold > flicker)
            flicker = sourceCodeLizardsFlickerThreshold;
    }

    public void Flicker(int fl = 10)
    {
        if (fl > flicker)
            flicker = fl;
    }

    public void WhiteFlicker(int fl = 15)
    {
        if (fl > whiteFlicker)
            whiteFlicker = fl;
    }
}
