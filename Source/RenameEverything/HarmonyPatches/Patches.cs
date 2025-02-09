using HarmonyLib;
using Verse;

namespace RenameEverything;

[StaticConstructorOnStartup]
public static class Patches
{
    static Patches()
    {
        RenameEverything.Harmony.PatchAll();

        if (ModCompatibilityCheck.RPGStyleInventory || ModCompatibilityCheck.RPGStyleInventoryCE)
        {
            var sandy_Detailed_RPG_Inventory_Type =
                GenTypes.GetTypeInAnyAssembly("Sandy_Detailed_RPG_Inventory.Sandy_Detailed_RPG_GearTab",
                    "Sandy_Detailed_RPG_Inventory");
            if (sandy_Detailed_RPG_Inventory_Type != null)
            {
                RenameEverything.Harmony.Patch(AccessTools.Method(sandy_Detailed_RPG_Inventory_Type, "DrawThingRow"),
                    null, null,
                    new HarmonyMethod(typeof(ITab_Pawn_Gear_DrawThingRow),
                        nameof(ITab_Pawn_Gear_DrawThingRow.Transpiler)));
                RenameEverything.Harmony.Patch(AccessTools.Method(sandy_Detailed_RPG_Inventory_Type, "DrawThingRow1"),
                    null, null,
                    new HarmonyMethod(
                        typeof(RPGStyleInventory_Sandy_Detailed_RPG_GearTab),
                        nameof(RPGStyleInventory_Sandy_Detailed_RPG_GearTab.Transpiler)));
            }
            else
            {
                Log.Error(
                    "Could not find type Sandy_Detailed_RPG_Inventory.Sandy_Detailed_RPG_GearTab in RPG Style Inventory");
            }
        }

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