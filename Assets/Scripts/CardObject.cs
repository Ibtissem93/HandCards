using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CardTypes
{
    attack,
    defense,
    spell
}


public class CardObject : MonoBehaviour
{
    public TextMeshProUGUI _name;
    public TextMeshProUGUI _cost;
    public TextMeshProUGUI _power;
    public Image _imageEffect;
    public TextMeshProUGUI _description;

    public CardTypes cardTypes;

    public void Init()
    {

    }

    public CardObject(string name, string cost, string power, Sprite image,string description)
    {
        //_name.text = name;
        //_cost.text = cost;
        //_power.text = power;
        //_description.text = description;
        //_imageEffect = image;
        //this.cardTypes = cardTypes;
    }

    public CardObject()
    {

    }
}
