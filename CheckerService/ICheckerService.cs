using CheckerBlazorServer.CheckerService.Model.BoardModelNS;
using CheckerBlazorServer.CheckerService.Model.CheckerModelNS;

namespace CheckerBlazorServer.CheckerService;

public interface ICheckerService
{
    void MoveChecker(CheckerModel checker, CheckerCoordinate checkerCoordinate);
    public IEnumerable<CheckerStep> ProbableSteps(CheckerModel checker);


}