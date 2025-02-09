using HarmonyLib;
using RimWorld;
using Verse;

namespace RenameEverything;

[HarmonyPatch(typeof(Thing), nameof(Thing.DrawGUIOverlay))]
public static class Thing_DrawGUIOverlay
{
    public static void Postfix(Thing __instance)
    {
        if (Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest &&
            RenameUtility.CanDrawThingName(__instance) && __instance.def.stackLimit <= 1 &&
            !__instance.def.HasComp(typeof(CompQuality)))
        {
            RenameUtility.DrawThingName(__instance);
        }
    }
}