﻿using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using SevenWonder.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonder.Factories
{
    public static class WonderFactory
    {
        public static IWonder CreateWonder(WonderName name, WonderBoardSide side)
        {
            switch (name)
            {
                case WonderName.ColossusOfRhodes:
                    return new ColossusWonder(side);
                case WonderName.LighthouseOfAlexandria:
                    return new LighthouseWonder(side);
                case WonderName.TempleOfArthemisInEphesus:
                    return new TempleOfArthemisWonder(side);
                case WonderName.HangingGardensOfBabylon:
                    return new HangingGardensWonder(side);
                case WonderName.StatueOfZeusInOlimpia:
                    return new StatueOfZeusWonder(side);
                case WonderName.MausoleumOfHalicarnassus:
                    return new MausoleumWonder(side);
                case WonderName.PyramidsOfGiza:
                    return new PyramidsWonder(side);
                default:
                    return null;
            }
        }
    }
}
