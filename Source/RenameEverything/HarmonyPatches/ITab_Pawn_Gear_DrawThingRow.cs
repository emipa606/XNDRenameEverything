using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace RenameEverything;

[HarmonyPatch(typeof(ITab_Pawn_Gear), "DrawThingRow")]
public static class ITab_Pawn_Gear_DrawThingRow
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var instructionList = instructions.ToList();
        var wordWrapInfo = AccessTools.Property(typeof(Text), nameof(Text.WordWrap)).GetSetMethod();
        var wordWraps = 0;
        var infoCardButtonInfo = AccessTools.Method(typeof(Widgets), nameof(Widgets.InfoCardButton), [
            typeof(float),
            typeof(float),
            typeof(Thing)
        ]);
        var doRenameFloatMenuButtonInfo =
            AccessTools.Method(typeof(ITab_Pawn_Gear_DrawThingRow), nameof(doRenameFloatMenuButton));
        foreach (var instruction in instructionList)
        {
            var codeInstruction = instruction;
            if (codeInstruction.opcode == OpCodes.Call && codeInstruction.operand == infoCardButtonInfo)
            {
                yield return codeInstruction;
                yield return new CodeInstruction(OpCodes.Ldloca_S, 0);
                yield return new CodeInstruction(OpCodes.Ldarga_S, 1);
                yield return new CodeInstruction(OpCodes.Ldarg_3);
                codeInstruction = new CodeInstruction(OpCodes.Call, doRenameFloatMenuButtonInfo);
            }

            if (codeInstruction.opcode == OpCodes.Call && codeInstruction.operand == wordWrapInfo)
            {
                wordWraps++;
                yield return codeInstruction;
                if (wordWraps % 2 == 0)
                {
                    codeInstruction =
                        new CodeInstruction(OpCodes.Call, RenameUtility.ChangeGUIColourPostLabelDrawInfo);
                }
                else
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_3);
                    codeInstruction = new CodeInstruction(OpCodes.Call,
                        RenameUtility.ChangeGUIColourPreLabelDrawThingInfo);
                }
            }

            yield return codeInstruction;
        }
    }

    private static void doRenameFloatMenuButton(ref Rect rect, ref float y, Thing thing)
    {
        rect.width -= 24f;
        var compRenamable = thing.TryGetComp<CompRenamable>();
        if (compRenamable != null &&
            Widgets.ButtonImage(new Rect(rect.width - 24f, rect.y + y, 24f, 24f), TexButton.RenameTex))
        {
            Find.WindowStack.Add(new FloatMenu(RenameUtility.CaravanRenameThingButtonFloatMenuOptions(compRenamable)
                .ToList()));
        }
    }
}