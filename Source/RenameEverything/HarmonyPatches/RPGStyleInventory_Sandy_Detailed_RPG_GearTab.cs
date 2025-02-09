using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace RenameEverything;

public static class RPGStyleInventory_Sandy_Detailed_RPG_GearTab
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var instructionList = instructions.ToList();
        var infoCardButtonInfo = AccessTools.Method(typeof(Widgets), nameof(Widgets.InfoCardButton), [
            typeof(float),
            typeof(float),
            typeof(Thing)
        ]);
        var doRenameFloatMenuButtonInfo =
            AccessTools.Method(typeof(RPGStyleInventory_Sandy_Detailed_RPG_GearTab), nameof(DoRenameFloatMenuButton));
        foreach (var instruction in instructionList)
        {
            var codeInstruction = instruction;
            if (codeInstruction.opcode == OpCodes.Call && codeInstruction.operand == infoCardButtonInfo)
            {
                yield return codeInstruction;
                yield return new CodeInstruction(OpCodes.Ldarg_1);
                yield return new CodeInstruction(OpCodes.Ldarg_2);
                codeInstruction = new CodeInstruction(OpCodes.Call, doRenameFloatMenuButtonInfo);
            }

            yield return codeInstruction;
        }
    }

    private static void DoRenameFloatMenuButton(Rect rect, Thing thing)
    {
        var compRenamable = thing.TryGetComp<CompRenamable>();
        if (compRenamable != null &&
            Widgets.ButtonImage(new Rect(rect.x, rect.y + 24f, 24f, 24f), TexButton.RenameTex))
        {
            Find.WindowStack.Add(new FloatMenu(RenameUtility.CaravanRenameThingButtonFloatMenuOptions(compRenamable)
                .ToList()));
        }
    }
}