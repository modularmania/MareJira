using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using MareJira.Objects;
using JiraTask = MareJira.Objects.JiraTask;

namespace MareJira.Commands;

public class AssigneeTasks {
    
    [DefaultMemberPermissions(GuildPermission.ManageNicknames)]
    public async Task HandleAssignTaskCommand(SocketSlashCommand command) {
        SocketGuildUser assignee = (SocketGuildUser)command.User; 
        SocketGuildUser assignedTo = null;
        var taskName = "";
        var description = "";
        string priority = "";
        DateTime deadline = new DateTime(9398, 12, 20);

        foreach (var option in command.Data.Options)
        {
            switch (option.Name) {
                
                case "assigned_to":
                    assignedTo = ((SocketGuildUser)option.Value);
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
                    try { // Makes sure that no exceptions occur here.
                        deadline = DateTime.ParseExact(option.Value.ToString(), "d", null);
                    } catch {
                        deadline = new DateTime(9398, 12, 20);
                    }
                    break;
                default:
                    await command.RespondAsync("Unrecognized command.", ephemeral: true);
                    break;
            }
        }

        Priority.TryParse(priority, out Priority myPriority);
        
        var embedBuilder = new EmbedBuilder()
            .WithAuthor(assignee.Nickname + " → " + assignedTo.Nickname)
            .WithTitle(taskName + "     " + myPriority.GetEnumDescription())
            .WithDescription(description)
            .WithFooter("Progress: TO-DO")
            .WithColor(myPriority.GetEnumColors());

        if (deadline < DateTime.Now) {
            await command.RespondAsync("That deadline is in the past!", ephemeral: true);
            return;
        } if (deadline.Year != 9398) {
            embedBuilder.WithFooter("Progress: TO-DO\nDeadline: " + deadline.DayOfWeek + ", " + deadline.ToString("MMMM") + " " + deadline.Day + ", " + deadline.Year);
        }
        
        await command.RespondAsync(text: "This message has been sent to both you and the other member: ",embed: embedBuilder.Build(), ephemeral: true);
        UserExtensions.SendMessageAsync(assignedTo, "", false, embedBuilder.Build());
        UserExtensions.SendMessageAsync(assignee, "", false, embedBuilder.Build());

        using (var context = new TaskContext()) {
            
            context.Database.EnsureCreated();
            
            var task = new JiraTask {
                Name = taskName,
                Assignee = assignedTo,
                Description = description,
                Priority = priority,
                Progress = "TO-DO"
            };
            if (deadline.Year != 9398) {
                task.Deadline = deadline;
            }

            var user = new JiraUser
            {
                ID = assignedTo.AvatarId,
                Tasks = new List<JiraTask> { task } // fix this. it creates an empty task list every time something is added, and gets rid of everything before.
            };
            
            context.SaveChanges();
        }
    }
}