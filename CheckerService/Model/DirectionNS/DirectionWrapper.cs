//using System;
//using CheckerBlazorServer.CheckerService.Model.BoardModelNS;
//using CheckerBlazorServer.CheckerService.Model.CheckerModelNS;

//namespace CheckerBlazorServer.CheckerService.Model.DirectionNS
//{
//    public class DirectionWrapper
//    {

//        private CheckerModel checker;
//        private CheckerDirectionEnum checkerDirectionEnum;

//        public DirectionWrapper(CheckerModel checkerCoordinate, CheckerDirectionEnum checkerDirectionEnum)
//        {
//            checker = checkerCoordinate;
//            this.checkerDirectionEnum = checkerDirectionEnum;
//        }

//        private ISet<CheckerDirectionEnum> TransformColorToDirection(CheckerColor checkerColor)
//        {
//            switch (checkerColor)
//            {
//                case CheckerColor.Black:
//                    return new HashSet<CheckerDirectionEnum> { CheckerDirectionEnum.UpLeft, CheckerDirectionEnum.UpRight };
//                case CheckerColor.White:
//                    return new HashSet<CheckerDirectionEnum> { CheckerDirectionEnum.DownLeft, CheckerDirectionEnum.DownRight };
//                case CheckerColor.King:
//                    return TransformColorToDirection(CheckerColor.Black)
//                        .Concat(TransformColorToDirection(CheckerColor.White)).ToHashSet();
//                default:
//                    break;
//            }
//            throw new ArgumentException($"{checkerColor} is unknown type");
//        }

//        public void NewCoordinateWrapper()
//        {

//        }


//        public ISet<CheckerDirectionEnum> a()
//        {
//            var directions = TransformColorToDirection(checker.CheckerColor);

//            foreach (var i in directions)
//            {
//                var field = DirectionBase.GetNewCoordinate(i, checker.CheckerCoordinate);

//                if (field == null)
//                {
//                    continue;
//                }

//            }
//            return directions;
//        }
//    }
//}

