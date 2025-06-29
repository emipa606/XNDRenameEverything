using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using Verse;

namespace RenameEverything;

[HarmonyPatch(typeof(Pawn), nameof(Pawn.GetInspectString))]
public static class Pawn_GetInspectString
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var instructionList = instructions.ToList();
        var thingLabelInfo = AccessTools.Property(typeof(Entity), nameof(Entity.Label)).GetGetMethod();
        var adjustedEquippedInspectStringInfo =
            AccessTools.Method(typeof(Pawn_GetInspectString), nameof(AdjustedEquippedInspectString));
        foreach (var instruction in instructionList)
        {
            var codeInstruction = instruction;
            if (codeInstruction.opcode == OpCodes.Callvirt && codeInstruction.operand == thingLabelInfo)
            {
                yield return codeInstruction;
                yield return new CodeInstruction(OpCodes.Ldarg_0);
                codeInstruction = new CodeInstruction(OpCodes.Call, adjustedEquippedInspectStringInfo);
            }

            yield return codeInstruction;
        }
    }

    public static string AdjustedEquippedInspectString(string original, Pawn instance)
    {
        if (!ModCompatibilityCheck.DualWield || !RenameEverythingSettings.DualWieldInspectString)
        {
            return original;
        }

        var equipment = instance.equipment;
        if (equipment is { Primary: not null } &&
            ReflectedMethods.TryGetOffHandEquipment(equipment, out var output))
        {
            return $"{original} {"AndLower".Translate()} {output.LabelCap}";
        }

        return original;
    }
}