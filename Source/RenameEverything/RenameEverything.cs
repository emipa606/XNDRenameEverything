using HarmonyLib;
using Mlie;
using UnityEngine;
using Verse;

namespace RenameEverything;

public class RenameEverything : Mod
{
    public static Harmony Harmony;
    public static string currentVersion;
    public RenameEverythingSettings settings;

    public RenameEverything(ModContentPack content)
        : base(content)
    {
        currentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
        GetSettings<RenameEverythingSettings>();
        Harmony = new Harmony("XeoNovaDan.RenameEverything");
    }

    public override string SettingsCategory()
    {
        return "RenameEverything.SettingsCategory".Translate();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        GetSettings<RenameEverythingSettings>().DoWindowContents(inRect);
    }
}