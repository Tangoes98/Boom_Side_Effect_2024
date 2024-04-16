using System;
using System.Collections.Generic;
using System.Linq;
using static DamageType;

public static class ArchitectConfig
{

    public static MinionBase GetMinionBase(string code) => _minionBases[code];

    public static string GetMutantResult(string fromArchitectCode, string toArchitectCode) 
                            => _mutations[new Tuple<string, string>(fromArchitectCode,toArchitectCode)];


    /**
    PRIVATE
    DO NOT MAKE PUBLIC !!
    **/


    static readonly Dictionary<string, MinionBase> _minionBases = _minionBaseArray.ToDictionary(m=>m.Code, m=>m);
    


    
    static readonly MinionBase[] _minionBaseArray = new MinionBase[] {
        new("minion1",
            "Test Minion",
            20,
            20,
            20,
            true,
            POISON,
            100,
            10,
            2)
    };

    static readonly Dictionary<Tuple<string,string>,string> _mutations = new() { //加成建筑，被加成建筑 => 合体建筑
        {new("basic1","basic2"), "mutant12"},
        {new("basic2","basic1"), "mutant21"}
    };


}
