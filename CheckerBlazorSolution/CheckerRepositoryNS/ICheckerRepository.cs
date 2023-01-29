using CheckerBlazorServer.CheckerService.Model.BoardModelNS;
using CheckerBlazorServer.CheckerService.Model.CheckerModelNS;

namespace CheckerBlazorServer.CheckerRepositoryNS
{
    public interface ICheckerRepository
    {
        void RelocateCheckerPosition(CheckerModel checkerModel, CheckerStep checkerStep);
        BoardField? GetBoardFieldByCoordinate(CheckerCoordinate checkerCoordinate);
        bool CheckerValidation(CheckerCoordinate checkerCoordinate);
        void RemoveChecker(CheckerCoordinate checkerCoordinate);
        public void RemoveHighlighted();
    }
}
