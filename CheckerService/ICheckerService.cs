using CheckerBlazorServer.CheckerService.Model.BoardModelNS;
using CheckerBlazorServer.CheckerService.Model.CheckerModelNS;

namespace CheckerBlazorServer.CheckerService;

public interface ICheckerService
{
    public BoardField[,] Board { get; }
    void MoveChecker(CheckerModel checker, int intendedRow, int intendedColumn);

}