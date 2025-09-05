namespace MRCustom;

public static class ColorExtensions
{
    private const float DefaultVisibilityLerpRatioModifierFullStrength = 0.13f;

    /// <summary>
    /// Dynamically adjusts a color to shift closer to the shiftToColor, depending on how close the grayscale is matching the shiftAgainstColor.
    /// (Used mainly for Crafter's gray so they gurantee pop against the background)
    /// </summary>
    /// <param name="idealColor"></param>
    /// <param name="shiftToColor"></param>
    /// <param name="shiftAgainstColor"></param>
    /// <param name="visibilityLerpRatioModifierFullStrength"></param>
    /// <returns></returns>
    public static Color ShiftToColorIfGrayscaleTooClose(this Color idealColor, in Color shiftToColor, in Color shiftAgainstColor, float visibilityLerpRatioModifierFullStrength = DefaultVisibilityLerpRatioModifierFullStrength)
    {
        float adjustmentIfTooCloseRatio;
        if (idealColor.grayscale < shiftAgainstColor.grayscale)
        {
            float inverseLerp = Mathf.InverseLerp(0, shiftAgainstColor.grayscale, idealColor.grayscale);
            adjustmentIfTooCloseRatio = inverseLerp * visibilityLerpRatioModifierFullStrength * 1.5f; //- MS7: Multiply a bit more since we need to put in extra work to get out of the darker section.

            return Color.Lerp(idealColor, shiftToColor, adjustmentIfTooCloseRatio);
        }
        else
        {
            float inverseLerp = Mathf.InverseLerp(shiftAgainstColor.grayscale, Color.white.grayscale, idealColor.grayscale);
            adjustmentIfTooCloseRatio = (1 - inverseLerp) * visibilityLerpRatioModifierFullStrength; // 1 - 0 because the inverse lerp here is closest to ideal gray at 0

            return Color.Lerp(idealColor, shiftToColor, adjustmentIfTooCloseRatio);
        }
    }

    /// <summary>
    /// Dynamically adjusts a color to shift to either the darker or brighter color depending on if it's grayscale is brighter or darker.
    /// </summary>
    /// <param name="idealColor"></param>
    /// <param name="shiftToColorIfThisGrayscaleBrighter"></param>
    /// <param name="shiftToColorIfThisGrayscaleDarker"></param>
    /// <param name="shiftAgainstColor"></param>
    /// <param name="visibilityLerpRatioModifierFullStrength"></param>
    /// <returns></returns>
    public static Color ShiftToColorIfGrayscaleTooClose(this Color idealColor, in Color shiftToColorIfThisGrayscaleBrighter, in Color shiftToColorIfThisGrayscaleDarker, in Color shiftAgainstColor, float visibilityLerpRatioModifierFullStrength = DefaultVisibilityLerpRatioModifierFullStrength)
    {
        float adjustmentIfTooCloseRatio;
        if (idealColor.grayscale < shiftAgainstColor.grayscale)
        {
            float inverseLerp = Mathf.InverseLerp(0, shiftAgainstColor.grayscale, idealColor.grayscale);
            adjustmentIfTooCloseRatio = inverseLerp * visibilityLerpRatioModifierFullStrength * 1.5f; //- MS7: Multiply a bit more since we need to put in extra work to get out of the darker section.

            return Color.Lerp(idealColor, shiftToColorIfThisGrayscaleDarker, adjustmentIfTooCloseRatio);
        }
        else
        {
            float inverseLerp = Mathf.InverseLerp(shiftAgainstColor.grayscale, Color.white.grayscale, idealColor.grayscale);
            adjustmentIfTooCloseRatio = (1 - inverseLerp) * visibilityLerpRatioModifierFullStrength; // 1 - 0 because the inverse lerp here is closest to ideal gray at 0

            return Color.Lerp(idealColor, shiftToColorIfThisGrayscaleBrighter, adjustmentIfTooCloseRatio);
        }
    }
}
