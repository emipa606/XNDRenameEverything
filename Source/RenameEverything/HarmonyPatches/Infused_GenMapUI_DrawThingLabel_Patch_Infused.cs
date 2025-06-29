using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;

namespace RenameEverything;

public static class Infused_GenMapUI_DrawThingLabel_Patch_Infused
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var instructionList = instructions.ToList();
        var adjustPositionIfNamedInfo =
            AccessTools.Method(typeof(GenMapUI_DrawThingLabel),
                nameof(GenMapUI_DrawThingLabel.AdjustPositionIfNamed));
        foreach (var instruction in instructionList)
        {
            var codeInstruction = instruction;
            if (codeInstruction.opcode == OpCodes.Ldc_R4 && (float)codeInstruction.operand == -0.66f)
            {
                yield return codeInstruction;
                yield return new CodeInstruction(OpCodes.Ldarg_0);
                codeInstruction = new CodeInstruction(OpCodes.Call, adjustPositionIfNamedInfo);
            }

            yield return codeInstruction;
        }
    }
}