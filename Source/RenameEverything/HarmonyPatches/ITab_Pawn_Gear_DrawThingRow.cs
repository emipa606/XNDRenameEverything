using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace RenameEverything;

[HarmonyPatch]
public static class ITab_Pawn_Gear_DrawThingRow
{
    public static IEnumerable<MethodBase> TargetMethods()
    {
        yield return AccessTools.Method(typeof(ITab_Pawn_Gear), "DrawThingRow");
        if (!ModCompatibilityCheck.RPGStyleInventory)
        {
            yield break;
        }

        yield return AccessTools.Method("Sandy_Detailed_RPG_GearTab:DrawThingRow");
    }

    public static void Prefix(Thing thing, float y, ref float width)
    {
        var compRenamable = thing.GetInnerIfMinified().TryGetComp<CompRenamable>();
        if (compRenamable == null)
        {
            return;
        }


        if (Widgets.ButtonImage(new Rect(width - 24f, y, 24f, 24f), TexButton.RenameTex))
        {
            Find.WindowStack.Add(new FloatMenu(RenameUtility.CaravanRenameThingButtonFloatMenuOptions(compRenamable)
                .ToList()));
        }

        width -= 24f;
    }
}