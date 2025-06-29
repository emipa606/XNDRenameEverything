using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RenameEverything;

public abstract class Command_Renamable : Command
{
    public CompRenamable renamable;

    protected List<CompRenamable> renamables;

    public override void ProcessInput(Event ev)
    {
        base.ProcessInput(ev);
        renamables ??= [];

        renamables.Add(renamable);
    }

    public override bool InheritInteractionsFrom(Gizmo other)
    {
        renamables ??= [];

        renamables.Add(((Command_Renamable)other).renamable);
        return false;
    }
}