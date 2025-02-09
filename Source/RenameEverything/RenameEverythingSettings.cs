using UnityEngine;
using Verse;

namespace RenameEverything;

public class RenameEverythingSettings : ModSettings
{
    public static bool showNameOnGround = true;

    public static bool appendCachedLabel;

    public static bool onlyAppendInThingHolder = true;

    public static bool pawnWeaponRenameGizmos = true;

    public static bool dualWieldInspectString = true;

    public void DoWindowContents(Rect wrect)
    {
        var listing_Standard = new Listing_Standard();
        var color = GUI.color;
        listing_Standard.Begin(wrect);
        GUI.color = color;
        Text.Font = GameFont.Small;
        Text.Anchor = TextAnchor.UpperLeft;
        listing_Standard.Gap();
        listing_Standard.CheckboxLabeled("RenameEverything.ShowNameOnGround".Translate(), ref showNameOnGround,
            "RenameEverything.ShowNameOnGround_Tooltip".Translate());
        listing_Standard.Gap();
        listing_Standard.CheckboxLabeled("RenameEverything.AppendCachedLabel".Translate(), ref appendCachedLabel,
            "RenameEverything.AppendCachedLabel_Tooltip".Translate());
        if (!appendCachedLabel)
        {
            GUI.color = Color.grey;
        }

        listing_Standard.Gap();
        listing_Standard.CheckboxLabeled("RenameEverything.AppendCachedLabelInThingHolder".Translate(),
            ref onlyAppendInThingHolder, "RenameEverything.AppendCachedLabelInThingHolder_Tooltip".Translate());
        GUI.color = color;
        listing_Standard.Gap();
        listing_Standard.CheckboxLabeled("RenameEverything.ShowRenameGizmosOnPawns".Translate(),
            ref pawnWeaponRenameGizmos, "RenameEverything.ShowRenameGizmosOnPawns_Tooltip".Translate());
        Text.Font = GameFont.Medium;
        listing_Standard.Gap(24f);
        listing_Standard.Label("RenameEverything.DualWieldIntegration".Translate());
        Text.Font = GameFont.Small;
        if (!ModCompatibilityCheck.DualWield)
        {
            GUI.color = Color.grey;
            listing_Standard.Label("RenameEverything.DualWieldNotActive".Translate());
            GUI.color = color;
        }
        else
        {
            listing_Standard.Gap();
            listing_Standard.CheckboxLabeled("RenameEverything.ShowBothWeaponNamesDualWield".Translate(),
                ref dualWieldInspectString, "RenameEverything.ShowBothWeaponNamesDualWield_Tooltip".Translate());
        }

        if (RenameEverything.currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("RenameEverything.CurrentModVersion".Translate(RenameEverything.currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
        Mod.GetSettings<RenameEverythingSettings>().Write();
    }

    public override void ExposeData()
    {
        Scribe_Values.Look(ref showNameOnGround, "showNameOnGround", true);
        Scribe_Values.Look(ref appendCachedLabel, "appendCachedLabel");
        Scribe_Values.Look(ref onlyAppendInThingHolder, "onlyAppendInThingHolder", true);
        Scribe_Values.Look(ref pawnWeaponRenameGizmos, "pawnWeaponRenameGizmos", true);
        Scribe_Values.Look(ref dualWieldInspectString, "dualWieldInspectString", true);
    }
}