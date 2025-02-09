using System;
using HarmonyLib;
using Verse;

namespace RenameEverything;

[StaticConstructorOnStartup]
public static class ReflectedMethods
{
    public delegate V FuncOut<in T, U, out V>(T input, out U output);

    public static readonly FuncOut<Pawn_EquipmentTracker, ThingWithComps, bool> TryGetOffHandEquipment;

    static ReflectedMethods()
    {
        if (ModCompatibilityCheck.DualWield)
        {
            TryGetOffHandEquipment = (FuncOut<Pawn_EquipmentTracker, ThingWithComps, bool>)Delegate.CreateDelegate(
                typeof(FuncOut<Pawn_EquipmentTracker, ThingWithComps, bool>),
                AccessTools.Method(GenTypes.GetTypeInAnyAssembly("DualWield.Ext_Pawn_EquipmentTracker", "DualWield"),
                    "TryGetOffHandEquipment"));
        }
    }
}