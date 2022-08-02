using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Pile : MonoBehaviour
{
    public static Pile instance;

    [HideInInspector]
    public List<Card> Deck = new List<Card>();
    [HideInInspector]
    public Action<List<Card>> OnPickCards;

    [SerializeField]
    private TextMeshProUGUI _pileCounterText;

    private List<Card> PickedCards = new List<Card>();
    private int _pileCounter,SavepileCounter;


   
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
       
      JsonController.instance.OnGeneratePile += DeckList;
      

    }
    private void Update()
    {
        _pileCounterText.text = _pileCounter.ToString();
    }

    public void DeckList(List<Card> _generatedPileCard)
    {
        StartCoroutine(PickedHandCard());
        foreach (var item in _generatedPileCard)
        {
           
            _pileCounter = _generatedPileCard.Count;
            Deck.Add(item);
        }
     
    }


    IEnumerator PickedHandCard()
    {
        yield return new WaitForSeconds(1);
        PickedHandCardd();


    }

    public void PickedHandCardd()
    {
        if(_pileCounter<4)
        {
          
            PickedCards = GetRandomElements<Card>(Deck, _pileCounter);
            SavepileCounter = _pileCounter;
            OnPickCards?.Invoke(PickedCards);
            _pileCounter -= PickedCards.Count;
            DiscardPile.instance.SendDiscardPileToPile();
        }
        else
        {
           
            PickedCards = GetRandomElements<Card>(Deck, 4-SavepileCounter);
            Debug.Log(SavepileCounter.ToString());
            OnPickCards?.Invoke(PickedCards);
            _pileCounter -= PickedCards.Count;
        }
       
    }





    public static List<t> GetRandomElements<t>(IEnumerable<t> list, int elementsCount)
    {
        return list.OrderBy(x => Guid.NewGuid()).Take(elementsCount).ToList();
    }

}
