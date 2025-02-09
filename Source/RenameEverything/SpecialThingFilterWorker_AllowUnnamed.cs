using Verse;

namespace RenameEverything;

public class SpecialThingFilterWorker_AllowUnnamed : SpecialThingFilterWorker
{
    public override bool Matches(Thing t)
    {
        if (!AlwaysMatches(t.def))
        {
            return !t.TryGetComp<CompRenamable>().Named;
        }

        return true;
    }

    public override bool AlwaysMatches(ThingDef def)
    {
        return !def.HasComp(typeof(CompRenamable));
    }
}