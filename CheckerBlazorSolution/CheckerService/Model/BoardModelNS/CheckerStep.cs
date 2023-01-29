using System;
namespace CheckerBlazorServer.CheckerService.Model.BoardModelNS;

public class CheckerStep
{
    public CheckerCoordinate IntendedCoordinate { get; set; }

    public bool ifJump { get; set; }

    public CheckerStep(CheckerCoordinate checkerCoordinate, bool ifJump)
    {
        IntendedCoordinate = checkerCoordinate;
        this.ifJump = ifJump;
    }

}

