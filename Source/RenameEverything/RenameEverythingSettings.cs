using UnityEngine;
using Verse;

namespace RenameEverything;

public class RenameEverythingSettings : ModSettings
{
    public static bool ShowNameOnGround = true;

    public static bool AppendCachedLabel;

    public static bool OnlyAppendInThingHolder = true;

    private static bool pawnWeaponRenameGizmos = true;

    public static bool DualWieldInspectString = true;

    public void DoWindowContents(Rect wrect)
    {
        var listingStandard = new Listing_Standard();
        var color = GUI.color;
        listingStandard.Begin(wrect);
        GUI.color = color;
        Text.Font = GameFont.Small;
        Text.Anchor = TextAnchor.UpperLeft;
        listingStandard.Gap();
        listingStandard.CheckboxLabeled("RenameEverything.ShowNameOnGround".Translate(), ref ShowNameOnGround,
            "RenameEverything.ShowNameOnGround_Tooltip".Translate());
        listingStandard.Gap();
        listingStandard.CheckboxLabeled("RenameEverything.AppendCachedLabel".Translate(), ref AppendCachedLabel,
            "RenameEverything.AppendCachedLabel_Tooltip".Translate());
        if (!AppendCachedLabel)
        {
            GUI.color = Color.grey;
        }

        listingStandard.Gap();
        listingStandard.CheckboxLabeled("RenameEverything.AppendCachedLabelInThingHolder".Translate(),
            ref OnlyAppendInThingHolder, "RenameEverything.AppendCachedLabelInThingHolder_Tooltip".Translate());
        GUI.color = color;
        listingStandard.Gap();
        listingStandard.CheckboxLabeled("RenameEverything.ShowRenameGizmosOnPawns".Translate(),
            ref pawnWeaponRenameGizmos, "RenameEverything.ShowRenameGizmosOnPawns_Tooltip".Translate());
        Text.Font = GameFont.Medium;
        listingStandard.Gap(24f);
        listingStandard.Label("RenameEverything.DualWieldIntegration".Translate());
        Text.Font = GameFont.Small;
        if (!ModCompatibilityCheck.DualWield)
        {
            GUI.color = Color.grey;
            listingStandard.Label("RenameEverything.DualWieldNotActive".Translate());
            GUI.color = color;
        }
        else
        {
            listingStandard.Gap();
            listingStandard.CheckboxLabeled("RenameEverything.ShowBothWeaponNamesDualWield".Translate(),
                ref DualWieldInspectString, "RenameEverything.ShowBothWeaponNamesDualWield_Tooltip".Translate());
        }

        if (RenameEverything.CurrentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("RenameEverything.CurrentModVersion".Translate(RenameEverything.CurrentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
        Mod.GetSettings<RenameEverythingSettings>().Write();
    }

    public override void ExposeData()
    {
        Scribe_Values.Look(ref ShowNameOnGround, "showNameOnGround", true);
        Scribe_Values.Look(ref AppendCachedLabel, "appendCachedLabel");
        Scribe_Values.Look(ref OnlyAppendInThingHolder, "onlyAppendInThingHolder", true);
        Scribe_Values.Look(ref pawnWeaponRenameGizmos, "pawnWeaponRenameGizmos", true);
        Scribe_Values.Look(ref DualWieldInspectString, "dualWieldInspectString", true);
    }
}