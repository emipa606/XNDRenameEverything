using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace RenameEverything;

[HarmonyPatch(typeof(GenMapUI), nameof(GenMapUI.DrawThingLabel), typeof(Thing), typeof(string), typeof(Color))]
public static class GenMapUI_DrawThingLabel
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var instructionList = instructions.ToList();
        var adjustPositionIfNamedInfo =
            AccessTools.Method(typeof(GenMapUI_DrawThingLabel), nameof(AdjustPositionIfNamed));
        for (var i = 0; i < instructionList.Count; i++)
        {
            var codeInstruction = instructionList[i];
            if (codeInstruction.opcode == OpCodes.Ldc_R4 && (float)codeInstruction.operand == -0.4f)
            {
                yield return codeInstruction;
                yield return new CodeInstruction(OpCodes.Ldarg_0);
                codeInstruction = new CodeInstruction(OpCodes.Call, adjustPositionIfNamedInfo);
            }

            yield return codeInstruction;
        }
    }

    public static float AdjustPositionIfNamed(float original, Thing thing)
    {
        if (RenameUtility.CanDrawThingName(thing))
        {
            return original - 0.26f;
        }

        return original;
    }

    public static void Postfix(Thing thing)
    {
        RenameUtility.DrawThingName(thing);
    }
}