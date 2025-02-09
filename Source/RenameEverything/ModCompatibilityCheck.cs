using System.Linq;
using Verse;

namespace RenameEverything;

public static class ModCompatibilityCheck
{
    public static bool DualWield => ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == "Dual Wield");

    public static bool Infused => ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == "Infused");

    public static bool RPGStyleInventory => ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == "RPG Style Inventory");

    public static bool RPGStyleInventoryCE =>
        ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == "RPG Style Inventory CE [1.0]");
}