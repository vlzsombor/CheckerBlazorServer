using CheckerBlazorServer.CheckerService.Model.BoardModelNS;

namespace Checker.Server.HubNS;

public class TableManager
{
    public Dictionary<string, int> Tables = new();
    public Dictionary<string, BoardField[]> TableState = new();
    public Dictionary<string, bool> ConnectionIdIsFirst = new();

}