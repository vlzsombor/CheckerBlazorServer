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


    public void RemoveHighlighted()
    {

        for (int i = 0; i < innerBoard.GetLength(0); i++)
        {
            for (int j = 0; j < innerBoard.GetLength(1); j++)
            {
                innerBoard[i,j].FieldAttributes.Remove(FieldAttribute.Highlighted);

            }
        }
    }

    private BoardField GetByCoordinate(CheckerCoordinate checkerCoordinate) => innerBoard[checkerCoordinate.Row, checkerCoordinate.Column];

    public void RemoveChecker(CheckerCoordinate checkerCoordinate)
    {
        var field = GetBoardFieldByCoordinate(checkerCoordinate);
        if (field is null)
        {
            return;
        }
        field.Checker = null;
    }

    public BoardField? GetBoardFieldByCoordinate(CheckerCoordinate checkerCoordinate)
    {
        if (!CheckerValidation(checkerCoordinate))
        {
            return null;
        }
        return innerBoard[checkerCoordinate.Row, checkerCoordinate.Column];
    }

    public void RelocateCheckerPosition(CheckerModel checkerModel, CheckerStep checkerStep)
    {
        var originalCheckerField = GetBoardFieldByCoordinate(checkerModel.CheckerCoordinate);
        var checker = originalCheckerField!.Checker;

        if (originalCheckerField is null)
        {
            throw new ArgumentException($"Either row: {checkerModel.CheckerCoordinate.Row} or column: {checkerModel.CheckerCoordinate.Row} is invalid.");
        }

        originalCheckerField.Checker!.CheckerCoordinate = checkerStep.IntendedCoordinate;

        var intendedCheckerField = GetBoardFieldByCoordinate(checkerStep.IntendedCoordinate);

        if(intendedCheckerField is null)
        {
            throw new ArgumentException($"Either row: {checkerStep.IntendedCoordinate.Row} or column: {checkerStep.IntendedCoordinate.Row} is invalid.");
        }
        intendedCheckerField.Checker = originalCheckerField!.Checker;


        originalCheckerField.Checker = null;
        originalCheckerField.FieldAttributes.Clear();
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

