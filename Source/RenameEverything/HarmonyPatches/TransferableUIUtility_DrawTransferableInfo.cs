using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace RenameEverything;

[HarmonyPatch(typeof(TransferableUIUtility), nameof(TransferableUIUtility.DrawTransferableInfo))]
public static class TransferableUIUtility_DrawTransferableInfo
{
    public static void Prefix(Transferable trad, ref Color labelColor)
    {
        if (!trad.HasAnyThing)
        {
            return;
        }

        var compRenamable = trad.AnyThing.TryGetComp<CompRenamable>();
        if (compRenamable != null)
        {
            labelColor = compRenamable.labelColour;
        }
    }
}