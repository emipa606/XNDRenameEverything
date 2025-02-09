using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace RenameEverything;

[HarmonyPatch(typeof(InspectPaneUtility), nameof(InspectPaneUtility.AdjustedLabelFor))]
public static class InspectPaneUtility_AdjustedLabelFor
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var instructionList = instructions.ToList();
        var clearInfo = AccessTools.Method(typeof(List<Thing>), nameof(List<Thing>.Clear));
        var clearCount = instructionList.Count(i => i.opcode == OpCodes.Callvirt && i.operand == clearInfo);
        var clearCounts = 0;
        var updateCachedSelectedThings =
            AccessTools.Method(typeof(InspectPaneUtility_AdjustedLabelFor), nameof(UpdateCachedSelectedThings));
        for (var j = 0; j < instructionList.Count; j++)
        {
            var instruction = instructionList[j];
            if (instruction.opcode == OpCodes.Callvirt && instruction.operand == clearInfo)
            {
                clearCounts++;
                if (clearCounts == clearCount)
                {
                    yield return new CodeInstruction(OpCodes.Call, updateCachedSelectedThings);
                    yield return instructionList[j - 1].Clone();
                }
            }

            yield return instruction;
        }
    }

    private static void UpdateCachedSelectedThings(List<Thing> selectedThings)
    {
        InspectPaneUtility_InspectPaneOnGUI.cachedSelectedThings.Clear();
        InspectPaneUtility_InspectPaneOnGUI.cachedSelectedThings.AddRange(selectedThings.ToArray());
    }
}