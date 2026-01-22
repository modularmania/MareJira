using Discord;
using Discord.WebSocket;
using MareJira.Commands;
using MareJira.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MareJira;
public class Program {
    
    private DiscordSocketClient _client;
    private ulong _guildId;
    private CommandHandler _commandHandler;
    private static IServiceProvider _serviceProvider;

    public static async Task Main()
        => await new Program().RunAsync();

    public async Task RunAsync() {
        
        var token = File.ReadAllText("D:\\Users\\modularmania\\RiderProjects\\MareJira\\Mare_Jira\\Secrets\\token.txt");
        _guildId = ulong.Parse(File.ReadAllText("D:\\Users\\modularmania\\RiderProjects\\MareJira\\Mare_Jira\\Secrets\\devguildid.txt"));
        
        _serviceProvider = CreateProvider();
        _client = _serviceProvider.GetRequiredService<DiscordSocketClient>();
        _commandHandler = _serviceProvider.GetRequiredService<CommandHandler>();
        
        _client.Log += Log;
        _client.Ready += async () => {
            await _commandHandler.RegisterCommands(_guildId);
        };
        
        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();
        await _client.SetActivityAsync(new CustomStatusGame("At Docks..."));
        
        await Task.Delay(-1);
    }
    
    private static Task Log(LogMessage msg) {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
    
    static IServiceProvider CreateProvider() {
        
        var config = new DiscordSocketConfig();
        return new ServiceCollection()
            .AddSingleton(config)
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton<CommandHandler>()
            .AddSingleton<AssigneeTasks>()
            .AddSingleton<AssignedTasks>()
            .AddSingleton<ViewTasks>()
            .AddDbContext<TaskContext>(
                options => options.UseMySQL("server=localhost;database=library;user=user;password=password"))
            .BuildServiceProvider();
    }
}