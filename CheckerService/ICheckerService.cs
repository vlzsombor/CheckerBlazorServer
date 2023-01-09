using CheckerBlazorServer.CheckerService.Model.BoardModelNS;
using CheckerBlazorServer.CheckerService.Model.CheckerModelNS;

namespace CheckerBlazorServer.CheckerService;

public interface ICheckerService
{
    BoardField[,] Board { get; }
    void MoveChecker(CheckerModel checker, int intendedRow, int intendedColumn);
    IEnumerable<CheckerCoordinate> ProbableSteps(CheckerModel checker);

}