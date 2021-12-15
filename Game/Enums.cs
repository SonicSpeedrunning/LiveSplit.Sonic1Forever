namespace LiveSplit.Sonic1Forever
{
    enum GameVersion
    {
        x86_1_2_1_and_below = 0x362B000,
        x86_1_3_4_and_over  = 0x3623000,
        x86_1_4_0_and_over  = 0x4474000,
        x64_1_2_1_and_below = 0x364B000,
        x64_1_3_4_and_over  = 0x3645000
    }

    enum StartTrigger
    {
        NewGame,
        NewGamePlus
    }

    enum ZoneIndicator : uint
    {
        MainMenu = 0x6E69614D,
        Zones = 0x656E6F5A,
        Ending = 0x69646E45,
        SaveSelect = 0x65766153
    }

    enum Acts : byte
    {
        GreenHillAct1 = 0,
        GreenHillAct2 = 1,
        GreenHillAct3 = 2,
        MarbleAct1 = 3,
        MarbleAct2 = 4,
        MarbleAct3 = 5,
        SpringYardAct1 = 6,
        SpringYardAct2 = 7,
        SpringYardAct3 = 8,
        LabyrinthAct1 = 9,
        LabyrinthAct2 = 10,
        LabyrinthAct3 = 11,
        StarLightAct1 = 12,
        StarLightAct2 = 13,
        StarLightAct3 = 14,
        ScrapBrainAct1 = 15,
        ScrapBrainAct2 = 16,
        ScrapBrainAct3 = 17,
        FinalZone = 18,
        Ending = 19
    }
}
