using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardList
{
    public List<Card> cards;
}

[Serializable]
public class Card
{
    public int id;
    public string name;
    public int cost;
    public string type;
    public int image_id;
    public List<Effects> effects;
}

[Serializable]
public class Effects
{
    public string type;
    public int value;
    public string target;
}