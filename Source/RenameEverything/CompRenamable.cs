using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RenameEverything;

public class CompRenamable : ThingComp
{
    private string _name = string.Empty;

    public bool allowMerge;
    private string cachedLabel = string.Empty;

    public Color labelColour = Color.white;

    public CompProperties_Renamable Props => (CompProperties_Renamable)props;

    public bool Named
    {
        get
        {
            if (!_name.NullOrEmpty())
            {
                return _name.ToLower() != cachedLabel.ToLower();
            }

            return false;
        }
        set
        {
            if (!value)
            {
                _name = string.Empty;
            }
        }
    }

    public string Name
    {
        get => !Named ? string.Empty : _name;
        set => _name = value;
    }

    public bool Coloured
    {
        get => !labelColour.IndistinguishableFrom(Color.white);
        set
        {
            if (!value)
            {
                labelColour = Color.white;
            }
        }
    }

    public override bool Equals(object obj)
    {
        if (obj is not CompRenamable compRenamable)
        {
            return false;
        }

        return Name.Equals(compRenamable.Name) && labelColour.IndistinguishableFrom(compRenamable.labelColour);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    public override bool AllowStackWith(Thing other)
    {
        if (!base.AllowStackWith(other))
        {
            return false;
        }

        return allowMerge || Equals(other.TryGetComp<CompRenamable>());
    }

    public override string TransformLabel(string label)
    {
        cachedLabel = label;
        if (!Named)
        {
            return label;
        }

        var appendCachedLabel = RenameEverythingSettings.appendCachedLabel &&
                                (!RenameEverythingSettings.onlyAppendInThingHolder ||
                                 ThingOwnerUtility.GetFirstSpawnedParentThing(parent) != parent);
        return Name + (appendCachedLabel ? $" ({cachedLabel.CapitalizeFirst()})" : string.Empty);
    }

    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
        return RenameUtility.GetRenamableCompGizmos(this);
    }

    public override void PostSplitOff(Thing piece)
    {
        var compRenamable = piece.TryGetComp<CompRenamable>();
        if (compRenamable != null)
        {
            compRenamable.Name = Name;
            compRenamable.labelColour = labelColour;
            compRenamable.cachedLabel = cachedLabel;
            compRenamable.allowMerge = allowMerge;
        }
        else
        {
            Log.Warning($"pieceCompRenamable (piece={piece}) is null");
        }
    }

    public override void PostExposeData()
    {
        Scribe_Values.Look(ref cachedLabel, "cachedLabel", string.Empty);
        Scribe_Values.Look(ref _name, "name", string.Empty);
        Scribe_Values.Look(ref labelColour, "labelColour", Color.white);
        Scribe_Values.Look(ref allowMerge, "allowMerge");
        base.PostExposeData();
    }

    public override string CompInspectStringExtra()
    {
        return !Named ? null : $"{Props.inspectStringTranslationKey.Translate()}: {cachedLabel.CapitalizeFirst()}";
    }
}