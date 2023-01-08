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
        var probableSteps = ProbableSteps(checker);

        var validStep = probableSteps.SingleOrDefault(coo => coo.Column == intendedColumn && coo.Row == intendedRow);

        if (validStep is null)
            return;

        RelocateCheckerPosition(checker, intendedRow, intendedColumn);
    }


    public IEnumerable<CheckerCoordinate> ProbableSteps(CheckerModel checker, bool secondTime = false)
    {
        List<CheckerCoordinate> checkerCoordinates = new List<CheckerCoordinate>();
        checkerCoordinates.AddRange(DefaultProbabilities(checker));

        var except = checkerCoordinates.Where(c => c.Row < 0 || c.Row >= Util.LENGTH || c.Column < 0 || c.Column >= Util.LENGTH);

        for (int i = 0; i < checkerCoordinates.Count(); i++)
        {
            var proabableCheckerCoordinate = checkerCoordinates[i];

            if (GetByCoordinate(proabableCheckerCoordinate).Checker is null)
                continue;
            if (!secondTime)
            {

            }
        }

        foreach (var proabableCheckerCoordinate in checkerCoordinates)
        {

        }

        return checkerCoordinates.Except(except);
    }
    private IEnumerable<CheckerCoordinate> DefaultProbabilities(CheckerModel checker)
    {
        List<CheckerCoordinate> checkerCoordinates = new List<CheckerCoordinate>();
        if (checker.CheckerColor == CheckerColor.Black || checker.CheckerColor == CheckerColor.King)
        {
            checkerCoordinates.Add(new CheckerCoordinate(checker.CheckerCoordinate.Row - 1, checker.CheckerCoordinate.Column + 1));
            checkerCoordinates.Add(new CheckerCoordinate(checker.CheckerCoordinate.Row - 1, checker.CheckerCoordinate.Column - 1));
        }
        if (checker.CheckerColor == CheckerColor.White || checker.CheckerColor == CheckerColor.King)
        {
            checkerCoordinates.Add(new CheckerCoordinate(checker.CheckerCoordinate.Row + 1, checker.CheckerCoordinate.Column + 1));
            checkerCoordinates.Add(new CheckerCoordinate(checker.CheckerCoordinate.Row + 1, checker.CheckerCoordinate.Column - 1));
        }
        return checkerCoordinates;
    }

    private BoardField GetByCoordinate(CheckerCoordinate checkerCoordinate) => innerBoard[checkerCoordinate.Row, checkerCoordinate.Column];

    private bool GenericBoundryValidator(Func<CheckerCoordinate, int> selector, CheckerCoordinate checkerModel)
    {
        var number = selector(checkerModel);

        if (number < 0 || number >= Util.LENGTH)
        {
            return false;
        }
        return true;
    }

    private bool Validate(CheckerModel sourceField, CheckerCoordinate targetField)
    {
        var returnBool = true;

        var result = GenericBoundryValidator(n => n.Row, targetField) && GenericBoundryValidator(n => n.Column, targetField);

        if (!result)
        {
            return false;
        }


        if (sourceField.CheckerColor == CheckerColor.Black || sourceField.CheckerColor == CheckerColor.King)
        {
            returnBool = GenericBoundryValidator(n => n.Row, targetField) && GenericBoundryValidator(n => n.Column, targetField);
        }
        if (sourceField.CheckerColor == CheckerColor.White || sourceField.CheckerColor == CheckerColor.King)
        {
            checkerCoordinates.Add(new CheckerCoordinate(checker.CheckerCoordinate.Row + 1, checker.CheckerCoordinate.Column + 1));
            checkerCoordinates.Add(new CheckerCoordinate(checker.CheckerCoordinate.Row + 1, checker.CheckerCoordinate.Column - 1));
        }
    }

    private bool ValidateIfTargetEmpty(CheckerModel targetField)
    {
        if (targetField.Checker is null)
            return false;
        return true;
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

