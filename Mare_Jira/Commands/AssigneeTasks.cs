using Discord;
using Discord.WebSocket;

namespace MareJira.Commands;

public class AssigneeTasks {
    public async Task HandleAssignTaskCommand(SocketSlashCommand command) {

        var guildUser = (SocketGuildUser)command.Data.Options.First().Value;

        var assignedTo = "";
        var taskName = "";
        var description = "";
        var priority = "";
        var deadline = "";

        foreach (var option in command.Data.Options)
        {
            switch (option.Name)
            {
                case "assigned_to":
                    assignedTo = ((SocketGuildUser)option.Value).Mention;
                    break;
                case "task_name":
                    taskName = option.Value.ToString();
                    break;
                case "description":
                    description = option.Value.ToString();
                    break;
                case "priority":
                    priority = option.Value.ToString();
                    break;
                case "deadline":
                    deadline = option.Value.ToString();
                    break;
                default:
                    await command.RespondAsync("Unrecognized command.");
                    break;
            }
        }

        var embedBuiler = new EmbedBuilder()
            .WithAuthor(guildUser.ToString(), guildUser.GetAvatarUrl() ?? guildUser.GetDefaultAvatarUrl())
            .WithTitle(taskName)
            .WithDescription(description)
            .WithColor(Color.Green)
            .WithCurrentTimestamp();

        await command.RespondAsync(embed: embedBuiler.Build());
    }
}