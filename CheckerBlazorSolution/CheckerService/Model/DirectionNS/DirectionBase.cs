using System.Data.Common;
using CheckerBlazorServer.CheckerService.Model.BoardModelNS;
using CheckerBlazorServer.Constant;

namespace CheckerBlazorServer.CheckerService.Model.DirectionNS;

public abstract class DirectionBase
{
    protected readonly CheckerCoordinate checkerCoordinate;

    public DirectionBase(CheckerCoordinate checkerCoordinate)
    {
        this.checkerCoordinate = checkerCoordinate;
    }

    protected abstract CheckerCoordinate TransformDirectionToNewCoordinate();

    public static CheckerCoordinate GetNewCoordinate(CheckerDirectionEnum checkerDirectionEnum,
        CheckerCoordinate checkerCoordinate)
    {

        switch (checkerDirectionEnum)
        {
            case CheckerDirectionEnum.DownLeft:
                return new DownLeft(checkerCoordinate).TransformDirectionToNewCoordinate();
            case CheckerDirectionEnum.DownRight:
                return new DownRight(checkerCoordinate).TransformDirectionToNewCoordinate();
            case CheckerDirectionEnum.UpLeft:
                return new UpLeft(checkerCoordinate).TransformDirectionToNewCoordinate();
            case CheckerDirectionEnum.UpRight:
                return new UpRight(checkerCoordinate).TransformDirectionToNewCoordinate();

            default:
                break;
        }
        throw new ArgumentException($"{checkerDirectionEnum} is not known");
    }

}


public class UpLeft : DirectionBase
{

    public UpLeft(CheckerCoordinate checkerCoordinate) : base(checkerCoordinate)
    {
    }

    protected override CheckerCoordinate TransformDirectionToNewCoordinate()
    {
        return new CheckerCoordinate(checkerCoordinate.Row - 1, checkerCoordinate.Column - 1);
    }
}

public class UpRight : DirectionBase
{
    public UpRight(CheckerCoordinate checkerCoordinate) : base(checkerCoordinate)
    {
    }

    protected override CheckerCoordinate TransformDirectionToNewCoordinate()
    {
        return new CheckerCoordinate(checkerCoordinate.Row - 1, checkerCoordinate.Column + 1);
    }
}

public class DownRight : DirectionBase
{
    public DownRight(CheckerCoordinate checkerCoordinate) : base(checkerCoordinate)
    {
    }

    protected override CheckerCoordinate TransformDirectionToNewCoordinate()
    {
        return new CheckerCoordinate(checkerCoordinate.Row + 1, checkerCoordinate.Column + 1);
    }
}

public class DownLeft : DirectionBase
{
    public DownLeft(CheckerCoordinate checkerCoordinate) : base(checkerCoordinate)
    {
    }

    protected override CheckerCoordinate TransformDirectionToNewCoordinate()
    {
        return new CheckerCoordinate(checkerCoordinate.Row + 1, checkerCoordinate.Column - 1);
    }
}
