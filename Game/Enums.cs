using System;

namespace LiveSplit.Sonic1Forever
{
    enum ZoneIndicator : uint
    {
        MainMenu = 0x6E69614Du,
        Zones = 0x656E6F5Au,
        Ending = 0x69646E45u,
        SaveSelect = 0x65766153u,
    }

    enum Acts : int
    {
        GreenHill1 = 0,
        GreenHill2 = 1,
        GreenHill3 = 2,
        Marble1 = 3,
        Marble2 = 4,
        Marble3 = 5,
        SpringYard1 = 6,
        SpringYard2 = 7,
        SpringYard3 = 8,
        Labyrinth1 = 9,
        Labyrinth2 = 10,
        Labyrinth3 = 11,
        Starlight1 = 12,
        Starlight2 = 13,
        Starlight3 = 14,
        ScrapBrain1 = 15,
        ScrapBrain2 = 16,
        ScrapBrain3 = 17,
        FinalZone = 18,
    }
}
