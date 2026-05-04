using UnityEngine;
using Verse;

namespace RenameEverything;

public class Command_Rename : Command_Renamable
{
    public override void ProcessInput(Event ev)
    {
        base.ProcessInput(ev);
        var dialog = new Dialog_RenameThings(renamables);
        dialog.WasOpenedByHotkey();
        Find.WindowStack.Add(dialog);
    }
}