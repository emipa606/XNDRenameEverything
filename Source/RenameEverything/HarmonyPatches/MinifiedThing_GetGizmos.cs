using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace RenameEverything;

[HarmonyPatch(typeof(MinifiedThing), nameof(MinifiedThing.GetGizmos))]
public static class MinifiedThing_GetGizmos
{
    public static void Postfix(MinifiedThing __instance, ref IEnumerable<Gizmo> __result)
    {
        var innerThing = __instance.InnerThing;

        var compRenamable = innerThing?.TryGetComp<CompRenamable>();
        if (compRenamable != null)
        {
            __result = __result.Concat(RenameUtility.GetRenamableCompGizmos(compRenamable));
        }
    }
}