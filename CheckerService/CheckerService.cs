using System;
using System.Drawing;
using Checker.Server.HubNS;
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
    private readonly TableManager tableManager;
    private CheckerColor lastColor;
    public static List<int> Tables = new();

    public CheckerColor GetColor(string connectionId)
    {
        int counter = 0;
        bool Result;
        while (!tableManager.ConnectionIdIsFirst.TryGetValue(connectionId, out Result) || counter++ > 100) ;
        if (counter > 100)
        {
            throw new Exception();
        }

        if (Result)
        {
            return CheckerColor.Black;
        }
        return CheckerColor.White;
    }

    public CheckerService(ICheckerRepository checkerRepository, TableManager tableManager)
    {
        this.checkerRepository = checkerRepository;
        this.tableManager = tableManager;
    }

    public void AddGameTable(int tableGuid)
    {
        Tables.Add(tableGuid);
    }

    public void MoveChecker(CheckerModel checker, CheckerCoordinate checkerCoordinate, string hubId)
    {
        var probableSteps = ProbableSteps(checker, hubId);

        var validStep = probableSteps.SingleOrDefault(coo =>
            coo.IntendedCoordinate.Column == checkerCoordinate.Column
            && coo.IntendedCoordinate.Row == checkerCoordinate.Row);

        if (validStep is null)
            return;

        checkerRepository.RelocateCheckerPosition(checker, validStep);

        if ((checker.CheckerCoordinate.Row == 0 && checker.CheckerType == CheckerType.Regular && checker.CheckerColor == CheckerColor.Black)
            ||
            checker.CheckerCoordinate.Row == Util.LENGTH && checker.CheckerType == CheckerType.Regular && checker.CheckerColor == CheckerColor.White)
            checker.CheckerType = CheckerType.King;

        lastColor = checker.CheckerColor;
    }


    public IEnumerable<CheckerStep> ProbableSteps(CheckerModel checker, string hubId)
    {
        var checkerColor = GetColor(hubId);
        if (checkerColor != checker.CheckerColor || lastColor == checker.CheckerColor)
        {
            return Enumerable.Empty<CheckerStep>();
        }

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
            return TransformColorToDirection(CheckerColor.Black, CheckerType.Regular)
                .Concat(TransformColorToDirection(CheckerColor.White, CheckerType.Regular)).ToHashSet();

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

