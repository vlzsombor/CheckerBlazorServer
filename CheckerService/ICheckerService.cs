using CheckerBlazorServer.CheckerService.Model.BoardModelNS;
using CheckerBlazorServer.CheckerService.Model.CheckerModelNS;

namespace CheckerBlazorServer.CheckerService;

public interface ICheckerService
{
    void MoveChecker(CheckerModel checker, CheckerCoordinate checkerCoordinate, string hubId);
    IEnumerable<CheckerStep> ProbableSteps(CheckerModel checker, string hubId);
    CheckerColor GetColor(string connectionId);

}