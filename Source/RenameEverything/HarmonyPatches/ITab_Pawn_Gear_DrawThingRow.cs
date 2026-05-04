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
    public static void Prefix(ref float y, out float __state)
    {
        __state = y;
    }

    public static void Postfix(float width, Thing thing, float __state)
    {
        doRenameFloatMenuButton(width, __state, thing);
    }

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var instructionList = instructions.ToList();
        var wordWrapInfo = AccessTools.Property(typeof(Text), nameof(Text.WordWrap)).GetSetMethod();
        var wordWraps = 0;
        foreach (var instruction in instructionList)
        {
            var codeInstruction = instruction;
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

    private static void doRenameFloatMenuButton(float width, float y, Thing thing)
    {
        var compRenamable = thing.GetInnerIfMinified().TryGetComp<CompRenamable>();
        if (compRenamable == null)
        {
            return;
        }

        if (Widgets.ButtonImage(new Rect(width - 72f, y, 24f, 24f), TexButton.RenameTex))
        {
            Find.WindowStack.Add(new FloatMenu(RenameUtility.CaravanRenameThingButtonFloatMenuOptions(compRenamable)
                .ToList()));
        }
    }
}