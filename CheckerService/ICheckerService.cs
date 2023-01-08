using CheckerBlazorServer.CheckerService.Model.BoardModelNS;

namespace CheckerBlazorServer.CheckerService;

public interface ICheckerService
{
    public BoardField[,] Board { get; }

}