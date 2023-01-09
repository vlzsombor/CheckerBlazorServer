using CheckerBlazorServer.CheckerService.Model.BoardModelNS;
using CheckerBlazorServer.CheckerService.Model.CheckerModelNS;

namespace CheckerBlazorServer.CheckerRepositoryNS
{
    public interface ICheckerRepository
    {
        void RelocateCheckerPosition(CheckerModel checkerModel, int intendedRow, int intendedColumn);
        BoardField? GetBoardFieldByCoordinate(CheckerCoordinate checkerCoordinate);
        bool CheckerValidation(CheckerCoordinate checkerCoordinate);
        BoardField[,] Board { get; }
        void RemoveChecker(CheckerCoordinate checkerCoordinate);
    }
}
