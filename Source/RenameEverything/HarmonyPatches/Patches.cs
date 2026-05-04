using HarmonyLib;
using Verse;

namespace RenameEverything;

[StaticConstructorOnStartup]
public static class Patches
{
    static Patches()
    {
        RenameEverything.Harmony.PatchAll();

        if (!ModCompatibilityCheck.Infused)
        {
            return;
        }

        var infusedType = GenTypes.GetTypeInAnyAssembly("Infused.GenMapUI_DrawThingLabel_Patch", "Infused");
        if (infusedType != null)
        {
            RenameEverything.Harmony.Patch(AccessTools.Method(infusedType, "Postfix"), null, null,
                new HarmonyMethod(typeof(Infused_GenMapUI_DrawThingLabel_Patch_Infused),
                    nameof(Infused_GenMapUI_DrawThingLabel_Patch_Infused.Transpiler)));
        }
        else
        {
            Log.Error("Could not find type Infused.GenMapUI_DrawThingLabel_Patch in Infused");
        }
    }
}