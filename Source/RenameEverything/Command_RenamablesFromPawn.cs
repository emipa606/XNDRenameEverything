using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RenameEverything;

public abstract class Command_RenamablesFromPawn : Command
{
    protected List<Pair<Pawn, List<CompRenamable>>> allPawnRenamables;
    public Pair<Pawn, List<CompRenamable>> pawnRenamables;

    protected abstract IEnumerable<FloatMenuOption> DoFloatMenuOptions();

    protected string FloatMenuOptionLabel(Pawn pawn, Thing renamableThing)
    {
        if (allPawnRenamables.Count > 1)
        {
            return $"{pawn.LabelShort}: {renamableThing.LabelCap}";
        }

        return renamableThing.LabelCap;
    }

    public override void ProcessInput(Event ev)
    {
        base.ProcessInput(ev);
        if (allPawnRenamables == null)
        {
            allPawnRenamables = [];
        }

        allPawnRenamables.Add(pawnRenamables);
        var options = DoFloatMenuOptions().ToList();
        Find.WindowStack.Add(new FloatMenu(options));
    }

    public override bool InheritInteractionsFrom(Gizmo other)
    {
        var command_RenamablesFromPawn = (Command_RenamablesFromPawn)other;
        if (allPawnRenamables == null)
        {
            allPawnRenamables = [];
        }

        allPawnRenamables.Add(command_RenamablesFromPawn.pawnRenamables);
        return false;
    }
}