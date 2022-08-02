using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiscardPile : MonoBehaviour
{
    #region Variables
    [HideInInspector]
    public static DiscardPile instance;

    [Header("Inspector Variables")]
    [SerializeField]
    private TextMeshProUGUI _discardPileCounterText;

    [HideInInspector]
    public List<Card> DeckDiscordPile = new List<Card>();
    [HideInInspector]
    public List<Card> DeckDiscordPileCards = new List<Card>();

    private int _discardPileCounter = 0;
    #endregion

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
        CardsManager.instance.AddCardsToDiscordPileCard  += AddHandCardsToDiscardPile;
        CardsManager.instance.AddCardToDiscordPile += AddHandCardToDiscardPiles;

    }
    // Update is called once per frame
    void Update()
    {
        _discardPileCounterText.text = _discardPileCounter.ToString();
    }


  

    public void AddHandCardsToDiscardPile(List<Card> _restCards)
    {
        foreach (var item in _restCards)
        {
            IncreamentCounter();
            DeckDiscordPile.Add(item);
        }
        CardsManager.instance.HideCards();
        Energy.instance._energyCounter = 4;

        StartCoroutine(ShowNewCards());
    }


    public void AddHandCardToDiscardPiles(GameObject _oneCards,int _cost, Card _card)
    {
             
            IncreamentCounter();
           
            Energy.instance.DesincreamentEnergie(_cost);
            DeckDiscordPile.Add(_card);
          
      
    }


    //Action to Show New Card and New Round
    IEnumerator ShowNewCards()
    {
        yield return new WaitForSeconds(1f);
        Pile.instance.PickedHandCardd();
    }


    public void SendDiscardPileToPile()
    {
        JsonController.instance.OnGeneratePile?.Invoke(DeckDiscordPile);
        DeckDiscordPile.Clear();
         _discardPileCounter = DeckDiscordPile.Count;
    }

    void IncreamentCounter()
    {
        _discardPileCounter++;
    }
}
