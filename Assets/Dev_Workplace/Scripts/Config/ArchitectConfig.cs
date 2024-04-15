using System;
using System.Collections.Generic;
using System.Linq;
using static ArchitectBase.ArchitectType;
using static DamageType;

public static class ArchitectConfig
{

    public static ArchitectBase[] GetAllBasicArchitects() => _architectBases.Values.Where(b=>!b.IsMutant).ToArray();
    public static ArchitectBase GetArchitectBase(string code) => _architectBases[code];
    public static MinionBase GetMinionBase(string code) => _minionBases[code];

    public static string GetMutantResult(string fromArchitectCode, string toArchitectCode) 
        => _mutations[new Tuple<string, string>(fromArchitectCode,toArchitectCode)];



    //PRIVATE, DO NOT MAKE PUBLIC

    static readonly Dictionary<string, ArchitectBase> _architectBases = _architectBaseArray.ToDictionary(b=>b.Code, b=>b);
    
    static readonly ArchitectBase[] _architectBaseArray = new ArchitectBase[] {
        new("basic1",
            "Test Tower",
            DEFENCE_TOWER,
            false,
            new() {
                {0, new(4, // maxLinkNum
                    100, // range
                    null, // 基础建筑没有modifer
                    20, // damage
                    FIRE,
                    20 // attack speed
                    )} 
            }
        ),
        new("basic2",
            "Test Barrack",
            BARRACK,
            false,
            new() {
                {0, new(4, // maxLinkNum
                    100, // range
                    null, // 基础建筑没有modifer
                    20, // damage
                    FIRE,
                    20 // attack speed
                    )} 
            }
        ),
        new("mutant12",
            "Test Mutant12",
            DEFENCE_TOWER,
            true,
            new() {
                {0, new(4, // maxLinkNum
                    100, // range
                    new() {
                            {1, new(1,0,0,0,SAME_AS_BEFORE,0)} // 加成建筑1连接时,属性修正
                        }, 
                    20, // damage
                    FIRE,
                    20 // attack speed
                    )} 
            }
        )
        
    };

    
    static readonly Dictionary<string, MinionBase> _minionBases = new() {
        
    };

    static readonly Dictionary<Tuple<string,string>,string> _mutations = new() {
        {new("basic1","basic2"), "mutant3"},
        {new("basic2","basic1"), "mutant3"}
    };


}
