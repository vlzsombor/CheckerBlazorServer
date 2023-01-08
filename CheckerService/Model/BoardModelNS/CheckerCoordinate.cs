namespace CheckerBlazorServer.CheckerService.Model.BoardModelNS;

public class CheckerCoordinate
{
    public int Row { get; set; }
    public int Column { get; set; }

    public CheckerCoordinate(int row, int column)
    {
        Row = row;
        Column = column;
    }
}

