using System;
using System.Collections.Generic;
using UnityEngine;
public class Effect
{
    public Dictionary<SpecialEffect,EffectStruct> effect;

    Dictionary<SpecialEffect,List<EffectStruct>> effectDict;

    public Effect()
    {
        effectDict = new()
        {
            { SpecialEffect.WEAK, new() },
            { SpecialEffect.DIZZY, new() },
            { SpecialEffect.POSION, new() },
            { SpecialEffect.SLOW, new() }
        };
        effect = new ();
    }

    public void AddEffect(SpecialEffect type, float modifier, float lastTime)
    {
        var effectList = effectDict[type];
        var es = new EffectStruct(type,modifier,lastTime);
        if(effectList.Count == 0)
        {
            effectList.Add(es);
        }
        else
        {
            bool inserted = false;
            for(int i = 0; i < effectList.Count; i++)
            {
                if (Mathf.Abs(effectList[i].modifier) < Mathf.Abs(modifier))
                {
                    effectList.Insert(i, es);
                    inserted = true;
                    break;
                }
            }
            if(!inserted) effectList.Add(es);
        }
        effect[type]= effectList[0];
    }
    public List<SpecialEffect> UpdateEffect(float time)
    {
        List<SpecialEffect> doneEffects = new();
        foreach(var entry in effectDict) {
            var effectList = entry.Value;
            SpecialEffect type = entry.Key;
            if(effectList.Count == 0) {
                continue;
            }

            foreach(EffectStruct es in effectList)
            {
                es.CountDown(time);
                if (es.lastTime <= 0)
                {
                    effectList.Remove(es);
                }
            }
            if(effectList.Count==0) {
                doneEffects.Add(type);
                effect.Remove(type);
            } else {
                effect[type] = effectList[0];
            }
        }
        return doneEffects;
    }

}
public struct EffectStruct
{
    public float modifier;
    public float lastTime;

    public SpecialEffect type;

    public EffectStruct(SpecialEffect type, float modifier, float lastTime)
    {
        this.type = type;
        this.modifier = modifier;
        this.lastTime = lastTime;
    }

    public void CountDown(float cd)
    {
        lastTime -= cd;
    }
}
