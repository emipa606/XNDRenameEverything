using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ColourPicker;
using HarmonyLib;
using Multiplayer.API;
using RimWorld;
using UnityEngine;
using Verse;

namespace RenameEverything;

public static class RenameUtility
{
    private const int MaxTextWidth = 65;

    private static Color cachedGUIColour;

    public static MethodInfo ChangeGUIColourPreLabelDrawIEnumerableThingInfo =>
        AccessTools.Method(typeof(RenameUtility), nameof(ChangeGUIColourPreLabelDraw), [typeof(IEnumerable<Thing>)]);

    public static MethodInfo ChangeGUIColourPreLabelDrawThingInfo => AccessTools.Method(typeof(RenameUtility),
        nameof(ChangeGUIColourPreLabelDraw), [typeof(Thing)]);

    public static MethodInfo ChangeGUIColourPostLabelDrawInfo =>
        AccessTools.Method(typeof(RenameUtility), nameof(ChangeGUIColourPostLabelDraw));

    public static IEnumerable<Gizmo> GetRenamableCompGizmos(CompRenamable renamableComp)
    {
        string filler = renamableComp.Props.inspectStringTranslationKey.Translate().CapitalizeFirst();
        yield return new Command_Rename
        {
            renamable = renamableComp,
            defaultLabel = renamableComp.Props.renameTranslationKey.Translate(),
            defaultDesc = "RenameEverything.RenameGizmo_Description".Translate(filler),
            icon = TexButton.RenameTex,
            hotKey = KeyBindingDefOf.Misc1
        };
        yield return new Command_RecolourLabel
        {
            renamable = renamableComp,
            defaultLabel = "RenameEverything.RecolourLabel".Translate(),
            defaultDesc = "RenameEverything.RecolourLabel_Description".Translate(filler),
            icon = TexButton.RecolourTex
        };
        if (!renamableComp.Named && !renamableComp.Coloured)
        {
            yield break;
        }

        if (renamableComp.parent.def.stackLimit > 1)
        {
            yield return new Command_Toggle
            {
                defaultLabel = "RenameEverything.AllowMerging".Translate(),
                defaultDesc = "RenameEverything.AllowMerging_Description".Translate(),
                icon = TexButton.AllowMergingTex,
                isActive = () => renamableComp.allowMerge,
                toggleAction = delegate { allowMergeGizmoToggleAction(renamableComp); }
            };
        }

        if (renamableComp.Named)
        {
            yield return new Command_Action
            {
                defaultLabel = "RenameEverything.RemoveName".Translate(),
                defaultDesc = "RenameEverything.RemoveName_Description".Translate(filler),
                icon = TexButton.DeleteX,
                action = delegate { removeNameGizmoAction(renamableComp); }
            };
        }
    }

    [SyncMethod]
    private static void allowMergeGizmoToggleAction(CompRenamable renamableComp)
    {
        renamableComp.allowMerge = !renamableComp.allowMerge;
    }

    [SyncMethod]
    private static void removeNameGizmoAction(CompRenamable renamableComp)
    {
        renamableComp.Named = false;
    }

    public static void ChangeGUIColourPreLabelDraw(IEnumerable<Thing> things)
    {
        var enumerable = things as Thing[] ?? things.ToArray();
        if (enumerable.Count() == 1)
        {
            ChangeGUIColourPreLabelDraw(enumerable.First());
        }
        else
        {
            cachedGUIColour = GUI.color;
        }
    }

    public static void ChangeGUIColourPreLabelDraw(Thing thing)
    {
        cachedGUIColour = GUI.color;
        var compRenamable = thing.GetInnerIfMinified().TryGetComp<CompRenamable>();
        if (compRenamable != null)
        {
            GUI.color = compRenamable.labelColour;
        }
    }

    public static void ChangeGUIColourPostLabelDraw()
    {
        GUI.color = cachedGUIColour;
    }

    public static IEnumerable<FloatMenuOption> CaravanRenameThingButtonFloatMenuOptions(CompRenamable renamableComp)
    {
        yield return new FloatMenuOption(renamableComp.Props.renameTranslationKey.Translate(),
            delegate { Find.WindowStack.Add(new Dialog_RenameThings(renamableComp)); });
        yield return new FloatMenuOption("RenameEverything.RecolourLabel".Translate(),
            delegate
            {
                Find.WindowStack.Add(new Dialog_ColourPicker(renamableComp.labelColour,
                    delegate(Color newColour) { renamableComp.labelColour = newColour; }));
            });
        if (renamableComp.Named)
        {
            yield return new FloatMenuOption("RenameEverything.RemoveName".Translate(),
                delegate { renamableComp.Named = false; });
        }
    }

    public static void DrawThingName(Thing thing)
    {
        if (!canDrawThingName(thing, out var renamableComp))
        {
            return;
        }

        Text.Font = GameFont.Tiny;
        var vector = GenMapUI.LabelDrawPosFor(thing, -0.4f);
        var text = Text.CalcSize(renamableComp.Name).x <= MaxTextWidth
            ? renamableComp.Name
            : renamableComp.Name.Shorten().Truncate(MaxTextWidth);
        var x = Text.CalcSize(text).x;
        GUI.DrawTexture(new Rect(vector.x - (x / 2f) - 4f, vector.y, x + 8f, 12f), TexUI.GrayTextBG);
        Text.Anchor = TextAnchor.UpperCenter;
        ChangeGUIColourPreLabelDraw(thing);
        Widgets.Label(new Rect(vector.x - (x / 2f), vector.y - 3f, x, 999f), text);
        ChangeGUIColourPostLabelDraw();
        Text.Anchor = TextAnchor.UpperLeft;
        Text.Font = GameFont.Small;
    }

    private static bool canDrawThingName(Thing t, out CompRenamable renamableComp)
    {
        renamableComp = t.TryGetComp<CompRenamable>();
        if (renamableComp is { Named: true } && RenameEverythingSettings.ShowNameOnGround)
        {
            return !typeof(Building).IsAssignableFrom(t.GetType());
        }

        return false;
    }

    public static bool CanDrawThingName(Thing t)
    {
        return canDrawThingName(t, out _);
    }
}