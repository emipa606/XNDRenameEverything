using System.Linq;
using Multiplayer.API;
using RimWorld;
using Verse;

namespace RenameEverything;

[StaticConstructorOnStartup]
public static class StaticConstructorClass
{
    static StaticConstructorClass()
    {
        foreach (var item in DefDatabase<ThingDef>.AllDefs.Where(t =>
                     typeof(ThingWithComps).IsAssignableFrom(t.thingClass)))
        {
            item.comps ??= [];

            if (item.IsWeapon)
            {
                item.comps.Add(new CompProperties_Renamable
                {
                    renameTranslationKey = "RenameEverything.RenameWeapon",
                    inspectStringTranslationKey = "ShootReportWeapon"
                });
            }
            else if (item.IsApparel)
            {
                item.comps.Add(new CompProperties_Renamable
                {
                    renameTranslationKey = "RenameEverything.RenameApparel",
                    inspectStringTranslationKey = "Apparel"
                });
            }
            else if (item.IsBuildingArtificial)
            {
                item.comps.Add(new CompProperties_Renamable
                {
                    renameTranslationKey = "RenameEverything.RenameBuilding",
                    inspectStringTranslationKey = "RenameEverything.Building"
                });
            }
            else if (typeof(Plant).IsAssignableFrom(item.thingClass))
            {
                item.comps.Add(new CompProperties_Renamable
                {
                    renameTranslationKey = "RenameEverything.RenamePlant",
                    inspectStringTranslationKey = "RenameEverything.Plant"
                });
            }
            else if (!typeof(Pawn).IsAssignableFrom(item.thingClass) &&
                     !typeof(Corpse).IsAssignableFrom(item.thingClass) &&
                     !typeof(MinifiedThing).IsAssignableFrom(item.thingClass))
            {
                item.comps.Add(new CompProperties_Renamable());
            }
        }

        if (MP.enabled)
        {
            MP.RegisterAll();
        }
    }
}