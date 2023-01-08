using System;
using CheckerBlazorServer.CheckerService.Model.BoardModelNS;
using CheckerBlazorServer.CheckerService.Model.CheckerModelNS;
using CheckerBlazorServer.Constant;
namespace CheckerBlazorServer.CheckerService;

public class CheckerService : ICheckerService
{
    private BoardField[,] innerBoard = new BoardField[Util.LENGTH, Util.LENGTH];

    public CheckerService()
    {
        InitializeBoard();
    }

    public BoardField[,] Board => innerBoard;



    public void MoveChecker(CheckerModel checker, int intendedRow, int intendedColumn)
    {
        var probableSteps = ProbableSteps(checker.CheckerCoordinate.Row, checker.CheckerCoordinate.Column, checker.CheckerColor);

        var validStep = probableSteps.SingleOrDefault(coo => coo.Column == intendedColumn && coo.Row == intendedRow);

        if (validStep is null)
            return;

        RelocateCheckerPosition(checker, intendedRow, intendedColumn);
    }

    public static IEnumerable<CheckerCoordinate> ProbableSteps(int row, int column, CheckerColor checkerColor)
    {
        List<CheckerCoordinate> checkerCoordinates = new List<CheckerCoordinate>();
        if (checkerColor == CheckerColor.Black || checkerColor == CheckerColor.King)
        {
            checkerCoordinates.Add(new CheckerCoordinate(row - 1, column + 1));
            checkerCoordinates.Add(new CheckerCoordinate(row - 1, column - 1));
        }
        if (checkerColor == CheckerColor.White || checkerColor == CheckerColor.King)
        {
            checkerCoordinates.Add(new CheckerCoordinate(row + 1, column + 1));
            checkerCoordinates.Add(new CheckerCoordinate(row + 1, column - 1));
        }

        var except = checkerCoordinates.Where(c => c.Row < 0 || c.Row > Util.LENGTH || c.Column < 0 || c.Column > 7);

        return checkerCoordinates.Except(except);
    }

    private void InitializeBoard()
    {
        //rows
        for (int i = 0; i < innerBoard.GetLength(0); i++)
        {
            //columns
            for (int j = 0; j < innerBoard.GetLength(1); j++)
            {
                innerBoard[i, j] = new BoardField();
                if (i % 2 == j % 2)
                {
                    innerBoard[i, j].BoardFieldType = BoardFieldType.AlwaysEmpty;
                    continue;
                }

                innerBoard[i, j].BoardFieldType = BoardFieldType.MightBeEmpty;

                if (i < 3)
                {
                    innerBoard[i, j].Checker = new CheckerModel(CheckerColor.White, new CheckerCoordinate(i, j));
                }

                if (i > 4)
                {
                    innerBoard[i, j].Checker = new CheckerModel(CheckerColor.Black, new CheckerCoordinate(i, j));
                }
            }
        }
    }

    private void RelocateCheckerPosition(CheckerModel checkerModel, int intendedRow, int intendedColumn)
    {
        var checker = innerBoard[checkerModel.CheckerCoordinate.Row, checkerModel.CheckerCoordinate.Column].Checker;

        innerBoard[checkerModel.CheckerCoordinate.Row, checkerModel.CheckerCoordinate.Column].Checker = null;
        innerBoard[checkerModel.CheckerCoordinate.Row, checkerModel.CheckerCoordinate.Column].FieldAttributes.Clear();

        checker!.CheckerCoordinate.Row = intendedRow;
        checker.CheckerCoordinate.Column = intendedColumn;

        innerBoard[intendedRow, intendedColumn].Checker = checker;
        innerBoard[intendedRow, intendedColumn].FieldAttributes = innerBoard[checkerModel.CheckerCoordinate.Row, checkerModel.CheckerCoordinate.Column].FieldAttributes;

    }
}

