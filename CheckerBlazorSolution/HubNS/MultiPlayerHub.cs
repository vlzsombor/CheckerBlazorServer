using CheckerBlazorServer.CheckerService.Model.BoardModelNS;
using CheckerBlazorServer.CheckerService.Model.CheckerModelNS;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis;
using NuGet.Protocol;
using System.Text.Json;

namespace Checker.Server.HubNS;

public class MultiPlayerHub : Hub
{
    private readonly TableManager tableManager;

    public MultiPlayerHub(TableManager tableManager)
    {
        this.tableManager = tableManager;
    }
    public override Task OnDisconnectedAsync(Exception exception)
    {
        Console.WriteLine("hello");
        return Task.CompletedTask;
    }
    public async Task JoinTable(string tableId)
    {
        
        if (!tableManager.Tables.ContainsKey(tableId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, tableId);
            tableManager.Tables.Add(tableId, 1);
            tableManager.ConnectionIdIsFirst.Add(Context.ConnectionId, true);
            return;
        }

        if (tableManager.Tables[tableId] < 2)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, tableId);

            var a = tableManager.TableState.TryGetValue(tableId, out var values);



            await Clients.Group(tableId)
                //.GroupExcept(tableId, Context.ConnectionId)
                .SendAsync("TableJoined", values);
            tableManager.Tables[tableId]++;
            tableManager.ConnectionIdIsFirst[Context.ConnectionId] = false;
        }
    }

    public async Task DisconnectAsync(string tableId, BoardField[] innerBoard)
    {
        //if (tableManager.Tables.ContainsKey(tableId))
        //{
        //    await Groups.RemoveFromGroupAsync(Context.ConnectionId, tableId);
        //    tableManager.Tables[tableId]--;
        //}



        //if (!tableManager.Tables.ContainsKey(tableId))
        //{
        //    await Groups.AddToGroupAsync(Context.ConnectionId, tableId);
        //    tableManager.Tables.Add(tableId, 1);
        //    tableManager.ConnectionIdIsFirst.Add(Context.ConnectionId, true);
        //    return;
        //}
        tableManager.ConnectionIdIsFirst.Remove(Context.ConnectionId);
        if (tableManager.Tables[tableId] == 1)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, tableId);

            tableManager.Tables.Remove(tableId);
            tableManager.TableState.Remove(tableId);
            return;
            //tableManager.ConnectionIdIsFirst[Context.ConnectionId] = true;
        }

        if (tableManager.Tables[tableId] == 2)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, tableId);

            //await Clients
            //    .GroupExcept(tableId, Context.ConnectionId)
            //    .SendAsync("TableJoined");
            tableManager.Tables[tableId]--;
            tableManager.TableState.Add(tableId, innerBoard);
            return;
            //tableManager.ConnectionIdIsFirst[Context.ConnectionId] = false;
        }

    }
    public async Task SendMove(string tableId, CheckerModel checkerModel, CheckerCoordinate checkerCoordinate)
    {
        await Clients
            .GroupExcept(tableId, Context.ConnectionId)
            .SendAsync("ReceiveMove", checkerModel, checkerCoordinate, Context.ConnectionId);
    }

}
