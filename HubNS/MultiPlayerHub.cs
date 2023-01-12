using CheckerBlazorServer.CheckerService.Model.BoardModelNS;
using CheckerBlazorServer.CheckerService.Model.CheckerModelNS;
using Microsoft.AspNetCore.SignalR;

namespace Checker.Server.HubNS;

public class MultiPlayerHub : Hub
{
    private readonly TableManager tableManager;

    public MultiPlayerHub(TableManager tableManager)
    {
        this.tableManager = tableManager;
    }

    public async Task JoinTable(string tableId)
    {
        if (tableManager.Tables.ContainsKey(tableId))
        {
            if (tableManager.Tables[tableId] < 2)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, tableId);

                await Clients
                    .GroupExcept(tableId, Context.ConnectionId)
                    .SendAsync("TableJoined");
                tableManager.Tables[tableId]++;

            }
        }
        else
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, tableId);
            tableManager.Tables.Add(tableId, 1);
        }


    }
    public async Task SendMove(string tableId, CheckerModel checkerModel, CheckerCoordinate checkerCoordinate)
    {
        await Clients
            .GroupExcept(tableId, Context.ConnectionId)
            .SendAsync("ReceiveMove", checkerModel, checkerCoordinate);
    }

}
