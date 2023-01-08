﻿using CheckerBlazorServer.CheckerService.Model.BoardModelNS;

namespace CheckerBlazorServer.CheckerService.Model.CheckerModelNS;

public class CheckerModel
{
    public CheckerColor CheckerColor { get; set; }
    public CheckerCoordinate CheckerCoordinate { get; set; }
    public CheckerModel(CheckerColor checkerColor, CheckerCoordinate checkerCoordinate)
    {
        CheckerColor = checkerColor;
        CheckerCoordinate = checkerCoordinate;
    }
}