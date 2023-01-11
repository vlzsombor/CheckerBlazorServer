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
    private readonly CheckerColor lastColor;

    public CheckerService(ICheckerRepository checkerRepository)
    {
        this.checkerRepository = checkerRepository;
    }



    public void MoveChecker(CheckerModel checker, int intendedRow, int intendedColumn)
    {
        var probableSteps = ProbableSteps(checker);

        var validStep = probableSteps.SingleOrDefault(coo => coo.IntendedCoordinate.Column == intendedColumn
        && coo.IntendedCoordinate.Row == intendedRow);

        if (validStep is null)
            return;

        checkerRepository.RelocateCheckerPosition(checker, validStep);
    }


    public IEnumerable<CheckerStep> ProbableSteps(CheckerModel checker)
    {
        var probableCoordinates = new HashSet<CheckerStep>();

        var directions = TransformColorToDirection(checker.CheckerColor, checker.CheckerType);

        foreach (var i in directions)
        {
            bool ifJump = false;

            var newCoordinate = DirectionBase.GetNewCoordinate(i, checker.CheckerCoordinate);
            var jumpedChecker = checkerRepository.GetBoardFieldByCoordinate(newCoordinate)?.Checker;
            if (jumpedChecker != null
                && jumpedChecker.CheckerColor != checker.CheckerColor)
            {
                ifJump = true;
                newCoordinate = DirectionBase.GetNewCoordinate(i, newCoordinate);
            }


            if (!checkerRepository.CheckerValidation(newCoordinate))
                continue;

            probableCoordinates.Add(new CheckerStep(newCoordinate, ifJump));
        }

        return probableCoordinates;

    }

    private ISet<CheckerDirectionEnum> TransformColorToDirection(CheckerColor checkerColor, CheckerType checkerType)
    {

        if (checkerType == CheckerType.King)
            return TransformColorToDirection(CheckerColor.Black, checkerType)
                .Concat(TransformColorToDirection(CheckerColor.White, checkerType)).ToHashSet();

        switch (checkerColor)
        {
            case CheckerColor.Black:
                return new HashSet<CheckerDirectionEnum> { CheckerDirectionEnum.UpLeft, CheckerDirectionEnum.UpRight };
            case CheckerColor.White:
                return new HashSet<CheckerDirectionEnum> { CheckerDirectionEnum.DownLeft, CheckerDirectionEnum.DownRight };
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

