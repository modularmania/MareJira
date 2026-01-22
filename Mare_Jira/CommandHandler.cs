using Discord;
using Discord.Net;
using Discord.WebSocket;
using MareJira.Commands;
using Newtonsoft.Json;

namespace MareJira;
public class CommandHandler {
    
    private readonly DiscordSocketClient _client;
    private readonly IServiceProvider _serviceProvider;
    private AssigneeTasks _assigneeTasks;
    private AssignedTasks _assignedTasks;
    private ViewTasks _viewTasks;

    public CommandHandler(DiscordSocketClient client, 
                          IServiceProvider serviceProvider,
                          AssigneeTasks assigneeTasks,
                          AssignedTasks assignedTasks,
                          ViewTasks viewTasks) {
        _client = client;
        _client.SlashCommandExecuted += SlashCommandHandler;
        _serviceProvider = serviceProvider;
        _assigneeTasks = assigneeTasks;
    }
    public async Task RegisterCommands(ulong guildId) {
        
        var guild = _client.GetGuild(guildId);

        var assignTask = new SlashCommandBuilder()
            .WithName("assigntasktest")
            .WithDescription("Assigns a task to the given user.")
            .AddOption("assigned_to", ApplicationCommandOptionType.User, "The member receiving the task", isRequired: true)
            .AddOption("task_name", ApplicationCommandOptionType.String, "The name of the task", isRequired: true)
            .AddOption("description", ApplicationCommandOptionType.String, "The description of the task; the task itself", isRequired: true)
            .AddOption(new SlashCommandOptionBuilder()
                        .WithName("priority").WithDescription("The priority of the task").WithRequired(true)
                        .AddChoice("Lowest", 1).AddChoice("Low", 2).AddChoice("Medium", 3).AddChoice("High", 4).AddChoice("Highest", 5)
                        .WithType(ApplicationCommandOptionType.Integer))
            .AddOption("deadline", ApplicationCommandOptionType.String, "The set deadline; must be in the format \"MM/DD/YYYY\"", isRequired: false);
        
        try {
            await guild.DeleteApplicationCommandsAsync();
            await guild.CreateApplicationCommandAsync(assignTask.Build());
        } catch (ApplicationCommandException exception) {
            var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
            Console.WriteLine(json);
        }
    }
    private async Task SlashCommandHandler(SocketSlashCommand command) {
        switch(command.Data.Name) {
            case "assigntasktest":
                await _assigneeTasks.HandleAssignTaskCommand(command);
                break;
            default:
                await command.RespondAsync("Unrecognized command.", ephemeral: true);
                break;
        }
    }
}