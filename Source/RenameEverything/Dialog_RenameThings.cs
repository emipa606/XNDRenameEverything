using System.Collections.Generic;
using Multiplayer.API;
using Verse;

namespace RenameEverything;

public class Dialog_RenameThings : Dialog_Rename<CompRenamable>
{
    private readonly List<CompRenamable> renamableComps;

    public Dialog_RenameThings(List<CompRenamable> renamableComps) : base(renamableComps.Count == 1
        ? renamableComps[0]
        : null)
    {
        this.renamableComps = renamableComps;
        if (renamableComps.Count != 1)
        {
            curName = string.Empty;
        }
    }

    public Dialog_RenameThings(CompRenamable renamableComp) : base(renamableComp)
    {
        renamableComps = [renamableComp];
    }

    protected override void OnRenamed(string name)
    {
        setName(name);
    }

    [SyncMethod]
    private void setName(string name)
    {
        foreach (var renamableComp in renamableComps)
        {
            renamableComp.Name = name;
        }
    }
}