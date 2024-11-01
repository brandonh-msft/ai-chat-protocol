using Client.Extensions;

using Console;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder b = new(args);
b.Configuration
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables("AICHATPROTOCOL_")
    .AddCommandLine(args);

b.Services
    .AddHostedService<Repl>()
    .AddAiChatProtocolClient();

b.Build().Run();
