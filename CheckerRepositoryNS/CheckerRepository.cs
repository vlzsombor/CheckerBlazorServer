using System;
using CheckerBlazorServer.CheckerService.Model.BoardModelNS;
using CheckerBlazorServer.CheckerService.Model.CheckerModelNS;
using CheckerBlazorServer.Constant;

namespace CheckerBlazorServer.CheckerRepositoryNS;

public class CheckerRepository : ICheckerRepository
{
    private BoardField[,] innerBoard = new BoardField[Util.LENGTH, Util.LENGTH];

    public CheckerRepository()
    {
        InitializeBoard();
    }


    public BoardField[,] Board => innerBoard;
    private BoardField GetByCoordinate(CheckerCoordinate checkerCoordinate) => innerBoard[checkerCoordinate.Row, checkerCoordinate.Column];

    public BoardField? GetBoardFieldByCoordinate(CheckerCoordinate checkerCoordinate)
    {
        if (!CheckerValidation(checkerCoordinate))
        {
            return null;
        }
        return innerBoard[checkerCoordinate.Row, checkerCoordinate.Column];
    }

    public void RelocateCheckerPosition(CheckerModel checkerModel, int intendedRow, int intendedColumn)
    {
        var checker = innerBoard[checkerModel.CheckerCoordinate.Row, checkerModel.CheckerCoordinate.Column].Checker;

        innerBoard[checkerModel.CheckerCoordinate.Row, checkerModel.CheckerCoordinate.Column].Checker = null;
        innerBoard[checkerModel.CheckerCoordinate.Row, checkerModel.CheckerCoordinate.Column].FieldAttributes.Clear();

        checker!.CheckerCoordinate.Row = intendedRow;
        checker.CheckerCoordinate.Column = intendedColumn;

        innerBoard[intendedRow, intendedColumn].Checker = checker;
        innerBoard[intendedRow, intendedColumn].FieldAttributes = innerBoard[checkerModel.CheckerCoordinate.Row, checkerModel.CheckerCoordinate.Column].FieldAttributes;
    }

    public bool CheckerValidation(CheckerCoordinate checkerCoordinate)
    {
        return NumberValidRule(checkerCoordinate.Row) && NumberValidRule(checkerCoordinate.Column);
    }
    private bool NumberValidRule(int num)
    {
        return num >= 0 && num < Util.LENGTH;
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

}

