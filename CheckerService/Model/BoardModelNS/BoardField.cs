using CheckerBlazorServer.CheckerService.Model.CheckerModelNS;
using CheckerBlazorServer.Constant;

namespace CheckerBlazorServer.CheckerService.Model.BoardModelNS;

public class BoardField
{
    public CheckerModel? Checker { get; set; }
    public BoardFieldType BoardFieldType { get; set; }
    public HashSet<FieldAttribute> FieldAttributes { get; set; } = new HashSet<FieldAttribute>();
}
