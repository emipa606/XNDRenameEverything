using System.Collections.Generic;
using Multiplayer.API;
using RimWorld;
using UnityEngine;
using Verse;

namespace RenameEverything;

public class Dialog_RenameThings : Window
{
    private readonly List<CompRenamable> renamableComps;
    private string curName;

    private bool focusedRenameField;

    private int startAcceptingInputAtFrame;

    public Dialog_RenameThings(List<CompRenamable> renamableComps)
    {
        this.renamableComps = renamableComps;
        if (renamableComps.Count == 1)
        {
            var compRenamable = renamableComps[0];
            curName = compRenamable.Named ? compRenamable.Name : compRenamable.parent.LabelCapNoCount;
        }
        else
        {
            curName = string.Empty;
        }
    }

    public Dialog_RenameThings(CompRenamable renamableComp)
    {
        forcePause = true;
        doCloseX = true;
        absorbInputAroundWindow = true;
        closeOnAccept = false;
        closeOnClickedOutside = true;
        renamableComps = [renamableComp];
        curName = renamableComp.Named ? renamableComp.Name : renamableComp.parent.LabelCapNoCount;
    }

    private static int MaxNameLength => 28;

    private bool AcceptsInput => startAcceptingInputAtFrame <= Time.frameCount;

    public override Vector2 InitialSize => new(280f, 175f);

    public void WasOpenedByHotkey()
    {
        startAcceptingInputAtFrame = Time.frameCount + 1;
    }

    private static AcceptanceReport nameIsValid(string name)
    {
        return name.Length != 0;
    }

    public override void DoWindowContents(Rect inRect)
    {
        Text.Font = GameFont.Small;
        var returnPressed = false;
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
        {
            returnPressed = true;
            Event.current.Use();
        }

        GUI.SetNextControlName("RenameField");
        var text = Widgets.TextField(new Rect(0f, 15f, inRect.width, 35f), curName);
        switch (AcceptsInput)
        {
            case true when text.Length < MaxNameLength:
                curName = text;
                break;
            case false:
                ((TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl)).SelectAll();
                break;
        }

        if (!focusedRenameField)
        {
            UI.FocusControl("RenameField", this);
            focusedRenameField = true;
        }

        if (!Widgets.ButtonText(new Rect(15f, inRect.height - 35f - 15f, inRect.width - 15f - 15f, 35f), "OK") &&
            !returnPressed)
        {
            return;
        }

        var acceptanceReport = nameIsValid(curName);
        if (!acceptanceReport.Accepted)
        {
            if (acceptanceReport.Reason.NullOrEmpty())
            {
                Messages.Message("NameIsInvalid".Translate(), MessageTypeDefOf.RejectInput, false);
                return;
            }

            Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
        }
        else
        {
            setName(curName);
            Find.WindowStack.TryRemove(this);
        }
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