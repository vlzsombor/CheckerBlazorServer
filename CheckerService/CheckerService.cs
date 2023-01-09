using System;
using System.Drawing;
using CheckerBlazorServer.CheckerService.Model;
using CheckerBlazorServer.CheckerService.Model.BoardModelNS;
using CheckerBlazorServer.CheckerService.Model.CheckerModelNS;
using CheckerBlazorServer.CheckerService.Model.DirectionNS;
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


    private BoardField? GetBoardFieldByCoordinate(CheckerCoordinate checkerCoordinate)
    {
        if (!CheckerValidation(checkerCoordinate))
        {
            return null;
        }
        return innerBoard[checkerCoordinate.Row, checkerCoordinate.Column];
    }


    public void MoveChecker(CheckerModel checker, int intendedRow, int intendedColumn)
    {
        var probableSteps = ProbableSteps(checker);

        var validStep = probableSteps.SingleOrDefault(coo => coo.Column == intendedColumn && coo.Row == intendedRow);

        if (validStep is null)
            return;

        RelocateCheckerPosition(checker, intendedRow, intendedColumn);
    }


    public IEnumerable<CheckerCoordinate> ProbableSteps(CheckerModel checker)
    {
        List<CheckerCoordinate> checkerCoordinates = new List<CheckerCoordinate>();
        checkerCoordinates.AddRange(DefaultProbableStep(checker));

        return checkerCoordinates;
    }

    private CheckerCoordinate GetNewCoordinates(CheckerDirectionEnum checkerDirectionEnum, CheckerCoordinate checkerCoordinate)
    {
        var field = DirectionBase.GetNewCoordinate(checkerDirectionEnum, checkerCoordinate);

        return field;
    }

    private static bool CheckerValidation(CheckerCoordinate checkerCoordinate)
    {
        return NumberValidRule(checkerCoordinate.Row) && NumberValidRule(checkerCoordinate.Column);
    }
    private static bool NumberValidRule(int num)
    {
        return num >= 0 && num < Util.LENGTH;
    }


    private IEnumerable<CheckerCoordinate> DefaultProbableStep(CheckerModel checker)
    {
        var probableCoordinates = new HashSet<CheckerCoordinate>();

        var directions = TransformColorToDirection(checker.CheckerColor);

        foreach (var i in directions)
        {

            var newCoordinate = GetNewCoordinates(i, checker.CheckerCoordinate);

            if (GetBoardFieldByCoordinate(newCoordinate)?.Checker != null)
            {
                newCoordinate = GetNewCoordinates(i, newCoordinate);
            }

            if (!CheckerValidation(newCoordinate))
                continue;

            probableCoordinates.Add(newCoordinate);

        }

        return probableCoordinates;
    }

    private ISet<CheckerDirectionEnum> TransformColorToDirection(CheckerColor checkerColor)
    {
        switch (checkerColor)
        {
            case CheckerColor.Black:
                return new HashSet<CheckerDirectionEnum> { CheckerDirectionEnum.UpLeft, CheckerDirectionEnum.UpRight };
            case CheckerColor.White:
                return new HashSet<CheckerDirectionEnum> { CheckerDirectionEnum.DownLeft, CheckerDirectionEnum.DownRight };
            case CheckerColor.King:
                return TransformColorToDirection(CheckerColor.Black)
                    .Concat(TransformColorToDirection(CheckerColor.White)).ToHashSet();
            default:
                break;
        }
        throw new ArgumentException($"{checkerColor} is unknown type");
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

