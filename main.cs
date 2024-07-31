using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

public class Plugin : BasePlugin
{
    public string Prefix = $" {ChatColors.Red}";

    public override string ModuleName => "Force Say";
    public override string ModuleVersion => "";
    public override string ModuleAuthor => "exkludera";

    public override void Load(bool hotReload)
    {
        AddCommand($"css_ForceSay", "", Command_ForceSay!);
        AddCommand($"css_ForceSayAll", "", Command_ForceSayAll!);
    }
    public override void Unload(bool hotReload)
    {
        RemoveCommand($"css_ForceSay", Command_ForceSay!);
        RemoveCommand($"css_ForceSayAll", Command_ForceSayAll!);
    }

    public void Command_ForceSay(CCSPlayerController player, CommandInfo command)
    {
        if (string.IsNullOrEmpty(command.ArgString))
        {
            command.ReplyToCommand(Prefix + "no args");
            return;
        }

        var targetInput = command.ArgByIndex(1);
        if (string.IsNullOrEmpty(targetInput))
        {
            command.ReplyToCommand(Prefix + "no target arg");
            return;
        }

        var args = command.ArgString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        var messageInput = string.Join(" ", args.Skip(1));
        if (string.IsNullOrEmpty(messageInput))
        {
            command.ReplyToCommand(Prefix + "no message arg");
            return;
        }

        bool foundPlayer = false;
        CCSPlayerController targetPlayer = null!;

        foreach (var target in Utilities.GetPlayers())
        {
            if (target.PlayerName.ToLower() == targetInput.ToLower())
            {
                targetPlayer = target;
                foundPlayer = true;
                break;
            }
            if (target.SteamID.ToString() == targetInput)
            {
                targetPlayer = target;
                foundPlayer = true;
                break;
            }
        }

        if (foundPlayer) targetPlayer!.ExecuteClientCommandFromServer($"say {messageInput}");
        else command.ReplyToCommand(Prefix + "target not found");
    }

    public void Command_ForceSayAll(CCSPlayerController player, CommandInfo command)
    {
        if (string.IsNullOrEmpty(command.ArgString))
        {
            command.ReplyToCommand(Prefix + "no message arg");
            return;
        }

        var args = command.ArgString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        var messageInput = string.Join(" ", args.Skip(0));

        foreach (var target in Utilities.GetPlayers())
        {
            target.ExecuteClientCommandFromServer($"say {messageInput}");
        }
    }
}