using System;

[Serializable]
public class MinionBase
{

    public string name;

    public float speed;

    public float minRange;

    public DamageType baseType;
    public DamageType addonType;

    public AttackMode attackMode;

    public LockMode lockMode;

    public MinionType minionType;

    public float fireInterval;

    public float fireTime; // each fire take time

    public SpecialEffect specialEffect;

    public SpecialEffect secondSpecialEffect;

    public MinionProperty[] levelRelatedProperties;

}
public enum MinionStateType
{
    IDLE,//����״̬
    VIEW,//����׷��
    INTERVAL,//���״̬
    ATTACK,//����״̬
    DYING,//����
    DIZZY//ѣ��
}

public enum MinionType
{
    ENEMY,//�о� 
    FRIEND//�Ѿ�
}