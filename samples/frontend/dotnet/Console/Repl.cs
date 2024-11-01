namespace Console;

using Backend.Model;

using Client.Interfaces;

using Microsoft.Extensions.Hosting;

using System;
using System.Threading;
using System.Threading.Tasks;

internal class Repl(IAiChatProtocolClient _protocolClient) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            Console.Write("Enter your message: ");
            var message = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine("Message cannot be empty.");
                continue;
            }

            var request = new AIChatRequest([new() { Content = message }]);
            var response = await _protocolClient.CompleteAsync(request, cancellationToken);

            Console.WriteLine($"Response: {response.Message.Content}");
            Console.WriteLine();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
