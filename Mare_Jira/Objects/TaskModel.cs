using Discord.WebSocket;

namespace MareJira.Objects;

public class JiraTask {
    
    public string Name { get; set; }
    public SocketGuildUser Assignee { get; set; }
    public DateTime Deadline { get; set; }
    public string Description { get; set; }
    public string Priority { get; set; }
    public string Progress { get; set; }
}

public class JiraUser {
    
    public string ID { get; set; }
    public ICollection<JiraTask> Tasks { get; set; }
}