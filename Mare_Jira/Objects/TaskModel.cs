using Discord.WebSocket;

namespace MareJira.Objects;

public class TaskModel {
    
    public string Name { get; set; }
    public DiscordSocketClient Assignee { get; set; }
    public DateTimeOffset Deadline { get; set; }
    public string Description { get; set; }
    public string Progress { get; set; }
}