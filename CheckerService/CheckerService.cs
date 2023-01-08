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
                    innerBoard[i, j].Checker = new CheckerModel(CheckerColor.White);
                }

                if (i > 4)
                {
                    innerBoard[i, j].Checker = new CheckerModel(CheckerColor.Black);
                }
            }
        }
    }
}

