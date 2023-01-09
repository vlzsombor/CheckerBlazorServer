using System;
using System.Drawing;
using CheckerBlazorServer.CheckerRepositoryNS;
using CheckerBlazorServer.CheckerService.Model;
using CheckerBlazorServer.CheckerService.Model.BoardModelNS;
using CheckerBlazorServer.CheckerService.Model.CheckerModelNS;
using CheckerBlazorServer.CheckerService.Model.DirectionNS;
using CheckerBlazorServer.Constant;
namespace CheckerBlazorServer.CheckerService;

public class CheckerService : ICheckerService
{
    private readonly ICheckerRepository checkerRepository;


    public CheckerService(ICheckerRepository checkerRepository)
    {
        this.checkerRepository = checkerRepository;
    }



    public void MoveChecker(CheckerModel checker, int intendedRow, int intendedColumn)
    {
        var probableSteps = ProbableSteps(checker);

        var validStep = probableSteps.SingleOrDefault(coo => coo.Column == intendedColumn && coo.Row == intendedRow);

        if (validStep is null)
            return;

        checkerRepository.RelocateCheckerPosition(checker, intendedRow, intendedColumn);
    }


    public IEnumerable<CheckerCoordinate> ProbableSteps(CheckerModel checker)
    {
        var probableCoordinates = new HashSet<CheckerCoordinate>();

        var directions = TransformColorToDirection(checker.CheckerColor);

        foreach (var i in directions)
        {

            var newCoordinate = DirectionBase.GetNewCoordinate(i, checker.CheckerCoordinate);

            if (checkerRepository.GetBoardFieldByCoordinate(newCoordinate)?.Checker != null)
            {
                newCoordinate = DirectionBase.GetNewCoordinate(i, newCoordinate);
            }


            if (!checkerRepository.CheckerValidation(newCoordinate))
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

    private bool GenericBoundryValidator(Func<CheckerCoordinate, int> selector, CheckerCoordinate checkerModel)
    {
        var number = selector(checkerModel);

        if (number < 0 || number >= Util.LENGTH)
        {
            return false;
        }
        return true;
    }
}

