using System;
using System.Collections.Generic;
using UnityEngine;
public class Effect
{
    public EffectStruct effect;

    List<EffectStruct> effectList;
    public Effect()
    {
        effectList = new List<EffectStruct>();
        effect = new EffectStruct(0,0);
    }

    public void AddEffect(SpecialEffect type, int level)
    {
        if(effectList.Count == 0)
        {
            effectList.Add(new EffectStruct(type,level));
        }
        else
        {
            for(int i = 0; i < effectList.Count; i++)
            {
                if (effect.modifierLevel < level)
                {
                    effectList.Insert(i, new EffectStruct(type, level));
                    break;
                }
            }
        }
        effect = effectList[0];
    }
    public void UpdateEffect(float time)
    {
        foreach(EffectStruct es in effectList)
        {
            es.CountDown(time);
            if (es.lastTime <= 0)
            {
                effectList.Remove(es);
            }
        }
        effect = effectList[0];
    }

}
public struct EffectStruct
{
    public float modifierLevel;
    public float modifier;
    public float lastTime;

    public EffectStruct(SpecialEffect type,int level)
    {
        modifierLevel = level; modifier = 0;lastTime = 0;
        switch (type)
        {
            case SpecialEffect.WEAK:
                modifier = level * 0.05f;
                lastTime = level * 1;
                break;
            case SpecialEffect.POSION:
                modifier = (5 + level) * 0.01f;
                lastTime = level + 2f;
                break;
            case SpecialEffect.SLOW:
                modifier = level *2f;
                lastTime = level + 2f;
                break;
            case SpecialEffect.DIZZY:
                modifier = 0;
                lastTime = (level+1)*0.1f;
                break;
        }
    }
    public void CountDown(float cd)
    {
        lastTime -= cd;
    }
}
