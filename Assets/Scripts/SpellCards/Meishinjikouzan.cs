﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meishinjikouzan : MonoBehaviour,ISpellCard
{
    public int ID { get; }
    public int Cost { get; }
    public string SpellCardName { get; }
    public string SpellCardDesc { get; }

    public Player Player;

    public Meishinjikouzan()
    {
        ID = 4;
        Cost = 4;
        SpellCardName = "断迷剑「迷津慈航斩」";
        SpellCardDesc =
            "往楼观剑中注入大量灵力，放出巨大的光束冲击。\n" +
            "对前方具有压倒性的攻击力，几乎无法回避。";
    }

    public void SpellCardRelease()
    {
        Player.enemy.GetComponent<IHealthPoint>().HP -= 100;
    }
}
