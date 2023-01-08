namespace CheckerBlazorServer.CheckerService.Model.CheckerModelNS;

public class CheckerModel
{
    public CheckerColor CheckerColor { get; set; }
    public CheckerModel(CheckerColor checkerColor)
    {
        CheckerColor = checkerColor;
    }
}