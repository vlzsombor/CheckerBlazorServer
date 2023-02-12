using System;
using CheckerBlazorServer.CheckerService.Model.BoardModelNS;
using CheckerBlazorServer.CheckerService.Model.CheckerModelNS;
using CheckerBlazorServer.Constant;

namespace CheckerBlazorServer.CheckerRepositoryNS;

public class CheckerRepository : ICheckerRepository
{
    public BoardField[,] InnerBoard { get; set; } = new BoardField[Util.LENGTH, Util.LENGTH];

    public CheckerRepository()
    {
        InitializeBoard();
    }


    public void RemoveHighlighted()
    {

        for (int i = 0; i < InnerBoard.GetLength(0); i++)
        {
            for (int j = 0; j < InnerBoard.GetLength(1); j++)
            {
                InnerBoard[i, j].FieldAttributes.Remove(FieldAttribute.Highlighted);
            }
        }
    }

    private BoardField GetByCoordinate(CheckerCoordinate checkerCoordinate) => InnerBoard[checkerCoordinate.Row, checkerCoordinate.Column];

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
        return InnerBoard[checkerCoordinate.Row, checkerCoordinate.Column];
    }

    public void RelocateCheckerPosition(CheckerModel checkerModel, CheckerStep checkerStep)
    {
        if (checkerStep.ifJump)
        {
            var jumpedRow = (checkerModel.CheckerCoordinate.Row + checkerStep.IntendedCoordinate.Row) / 2.0;
            var jumpedColumn = (checkerModel.CheckerCoordinate.Column + checkerStep.IntendedCoordinate.Column) / 2.0;
            if (jumpedRow % 1 != 0 || jumpedColumn % 1 != 0)
            {
                throw new Exception($"{jumpedRow} or {jumpedColumn} was not even");
            }

            var jumpedField = GetBoardFieldByCoordinate(new CheckerCoordinate((int)jumpedRow, (int)jumpedColumn));

            if (jumpedField is null)
            {
                throw new Exception($"There was no check under the jumped field Row: {jumpedRow} or Column: {jumpedColumn}");
            }

            jumpedField.Checker = null;
        }

        var originalCheckerField = GetBoardFieldByCoordinate(checkerModel.CheckerCoordinate);
        var checker = originalCheckerField!.Checker;




        if (originalCheckerField is null)
        {
            throw new ArgumentException($"Either row: {checkerModel.CheckerCoordinate.Row} or column: {checkerModel.CheckerCoordinate.Row} is invalid.");
        }

        originalCheckerField.Checker!.CheckerCoordinate = checkerStep.IntendedCoordinate;

        var intendedCheckerField = GetBoardFieldByCoordinate(checkerStep.IntendedCoordinate);

        if (intendedCheckerField is null)
        {
            throw new ArgumentException($"Either row: {checkerStep.IntendedCoordinate.Row} or column: {checkerStep.IntendedCoordinate.Row} is invalid.");
        }
        intendedCheckerField.Checker = originalCheckerField!.Checker;
        intendedCheckerField.FieldAttributes = originalCheckerField.FieldAttributes.ToHashSet();

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
        for (int i = 0; i < InnerBoard.GetLength(0); i++)
        {
            //columns
            for (int j = 0; j < InnerBoard.GetLength(1); j++)
            {
                InnerBoard[i, j] = new BoardField();
                if (i % 2 == j % 2)
                {
                    InnerBoard[i, j].BoardFieldType = BoardFieldType.AlwaysEmpty;
                    continue;
                }

                InnerBoard[i, j].BoardFieldType = BoardFieldType.MightBeEmpty;

                if (i < 3)
                {
                    InnerBoard[i, j].Checker = new CheckerModel(CheckerColor.White, new CheckerCoordinate(i, j));
                }

                if (i > 4)
                {
                    InnerBoard[i, j].Checker = new CheckerModel(CheckerColor.Black, new CheckerCoordinate(i, j));
                }
            }
        }
    }


}

