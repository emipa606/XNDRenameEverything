using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace RenameEverything;

[HarmonyPatch(typeof(InspectPaneUtility), nameof(InspectPaneUtility.InspectPaneOnGUI))]
public static class InspectPaneUtility_InspectPaneOnGUI
{
    public static readonly List<Thing> cachedSelectedThings = [];

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var instructionList = instructions.ToList();
        var labelInfo = AccessTools.Method(typeof(Widgets), nameof(Widgets.Label), [
            typeof(Rect),
            typeof(string)
        ]);
        var cachedSelectedThingsInfo =
            AccessTools.Field(typeof(InspectPaneUtility_InspectPaneOnGUI), nameof(cachedSelectedThings));
        for (var i = 0; i < instructionList.Count; i++)
        {
            var instruction = instructionList[i];
            if (instruction.opcode == OpCodes.Ldloc_S && ((LocalBuilder)instruction.operand).LocalIndex == 4)
            {
                var codeInstruction = instructionList[i + 1];
                var codeInstruction2 = instructionList[i + 2];
                if (codeInstruction.opcode == OpCodes.Ldloc_S &&
                    ((LocalBuilder)codeInstruction.operand).LocalIndex == 5 &&
                    codeInstruction2.opcode == OpCodes.Call && codeInstruction2.operand == labelInfo)
                {
                    yield return new CodeInstruction(OpCodes.Ldsfld, cachedSelectedThingsInfo);
                    yield return new CodeInstruction(OpCodes.Call,
                        RenameUtility.ChangeGUIColourPreLabelDrawIEnumerableThingInfo);
                    instructionList.Insert(i + 3,
                        new CodeInstruction(OpCodes.Call, RenameUtility.ChangeGUIColourPostLabelDrawInfo));
                }
            }

            yield return instruction;
        }
    }
}