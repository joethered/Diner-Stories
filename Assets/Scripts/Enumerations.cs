public enum RoleType
{
    All,
    Servers,
    Cooks,
    Hosts,
    Managers,
    Daters,
    Bereaved,
    Widowed,
    Eulogists,
    Gaurds,
    Cops,
    Robbers,
    Patron,
    RobberyVictim,
    Lovers,
    Spurned,
    Friends,
    Enemies,
    Deceased,
    Player,
    PreviousPlayers
}

public enum ActionType
{
    witty,
    formal,
    awkward
}

public enum Gender
{
    male, female, transgenderM, transgenderF
}

public enum Sexuality
{
    hetero, gay, bi
}

public enum FemaleName
{
    Autumn, Beth, Cindy, Diane, Erin, Fallon, Grace, Hellen, Iris, Jane, Karen, Lola, Maria, Nina, Olive, Pollyana,
    Queen, Rosa, Sarah, Tina, Uma, Vicky, Wendy, Xandra, Yoko, Zoe
}

public enum MaleName
{
    Ari, Bob, Cristos, Diego, Eddy, Frank, George, Henry, Idris, James, Karl, Legolas, Mario, Nobuo, Oscar, Paul,
    Quinten, Red, Sean, Tiberius, Ulysses, Vincent, Wesley, Xander, Yusuke, Zed
}

public enum NeutralName
{
    Amari, Blake, Casey, Dakota, Emory, Harper, Justice, Kai, Landry, Milan, Oakley, Phoenix, Quinn, River, Sawyer, Tatum
}

public enum LastName
{
    Allen, Boreanaz, Chavez, Dimka, Estevez, Franco, Gilbertson, Hange, Ikuta, Jones, Kalama, Lymperopoulos, McCloud,
    Nquyen, Oppenheimer, Poe, Qadir, Rossi, Simpson, Tortelli, Umezaki, Vega, Whitehead, Xi, Yamauchi, Zardof
}

public enum Scene
{
    SpeedDating, Robbery, Funeral
}

public enum PlotThread
{
    //Possible Speed Date Plot threads
    what_will_happen_to_the_lovers,
    what_will_happen_to_the_spurned,
    what_was_the_signifigance_of_the_small_talk,

    //Robbery Plot threads
    what_happened_to_the_robber,
    aftermath_of_death,

    //Funeral Plot threads
    what_happened_to_the_widow

}

public enum SpeechPhase
{
    Call,
    Response
}