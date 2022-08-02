using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsManager : MonoBehaviour
{
    public static CardsManager instance;

    [Header("Inspector Values")]
    public Transform _parentCardPrefab;
    public Transform startPoint;
    public GameObject CardPrefab;
    public Transform _discordPileTransform;


   public  List<GameObject> _handCards = new List<GameObject>();


    public Action<List<GameObject>> AddCardsToDiscordPile;
    public Action<GameObject,int, Card> AddCardToDiscordPile;

    //new
    private List<Card> _handCardsCard = new List<Card>();
    public Action<List<Card>> AddCardsToDiscordPileCard;
    public Action<Card, int> AddCardToDiscordPileCard;


    private void Awake()
    {
        if(instance != null)
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
        Pile.instance.OnPickCards += ShowCards;
       
    }
    public void ShowCards(List<Card> _pickedCards)
    {
        foreach (var card in _pickedCards)
        {
            //init cards
            CardObject co = CardPrefab.GetComponent<CardObject>();

            co._name.text = card.name;
            co._cost.text = card.cost.ToString();

            co._power.text = card.type;

            foreach (var item in card.effects)
            {
                co._description.text = item.type + " " + item.value + " " + item.target;
                if (item.type.Equals("damage"))
                {
                    Sprite ss = Resources.Load<Sprite>("Sprites/AttackIcon");
                    co._imageEffect.GetComponent<Image>().sprite = ss;
                }
                else
                    if (item.type.Equals("shield"))
                {
                    Sprite ss = Resources.Load<Sprite>("Sprites/DefenceIcon");
                    co._imageEffect.GetComponent<Image>().sprite = ss;
                }
                else
                    if (item.type.Equals("strength"))
                {
                    Sprite ss = Resources.Load<Sprite>("Sprites/strength");
                    co._imageEffect.GetComponent<Image>().sprite = ss;
                }
            }
            //  CardObject co = new CardObject(card.name, card.cost.ToString(), card.type, card.effects.ToString());
            _handCardsCard.Add(card);
            GameObject cardInhand = Instantiate(CardPrefab, _parentCardPrefab);
            cardInhand.SetActive(false);
            cardInhand.transform.GetComponent<Button>().onClick.AddListener(delegate { ParameterOnClick(cardInhand.gameObject, card); });
            cardInhand.transform.GetComponent<Button>().enabled=false;
           
          

            _handCards.Add(cardInhand);
        }


        StartCoroutine(ShowCardsWithAnim());
    
        

    }

    IEnumerator ShowCardsWithAnim()
    {
        for (int i = 0; i < _handCards.Count; i++)
        {
          
            yield return new WaitForSeconds(0.1f);
            if(i==0)
            {
                _handCards[i].transform.position = new Vector3(startPoint.transform.position.x, startPoint.transform.position.y, startPoint.transform.position.z);
                _handCards[i].SetActive(true);
            }
            else
            {
                _handCards[i].transform.position = new Vector3(_handCards[i-1].transform.position.x + 250f, _handCards[i-1].transform.position.y, _handCards[i-1].transform.position.z);
                _handCards[i].SetActive(true);
            }
           
        }
       
    }

    //When EndTurn Pressed
    public void HideCards()
    {
       
        for (int i = _handCards.Count - 1; i >= 0; i--)
        {
            GameObject Clone = _handCards[i];
         
            _handCards.RemoveAt(i);
          
            Destroy(Clone);
        }

        //new
        for (int j = _handCardsCard.Count - 1; j >= 0; j--)
        {
            Card Clone = _handCardsCard[j];

            _handCardsCard.RemoveAt(j);

          
        }
    }

   public void SendListToDiscardPile()
   {

        StartCoroutine(SendListToDiscardPileWithAnim());
 
   }

    IEnumerator SendListToDiscardPileWithAnim()
    {
        Sequence s = DOTween.Sequence();
        foreach (var item in _handCards)
        {
            s.Append(item.transform.DOMove(new Vector3(item.transform.position.x, item.transform.position.y + 70f, item.transform.position.z), 1f));
             yield return new WaitForSeconds(0.1f);
            s.Join(item.transform.DORotate(new Vector3(item.transform.rotation.x, item.transform.rotation.y, item.transform.rotation.z - 50f), 1));
             yield return new WaitForSeconds(0.1f);
            s.Append(item.transform.DOMove(new Vector3(_discordPileTransform.position.x, _discordPileTransform.position.y, _discordPileTransform.position.z), 1f));
            s.Append(item.transform.DOScale(new Vector3(0, 0, 0), 1f));
        }
        AddCardsToDiscordPileCard?.Invoke(_handCardsCard);
        AddCardsToDiscordPile?.Invoke(_handCards);
        //new
       
    }

    private void ParameterOnClick(GameObject _playedCard,Card _card)
    {
          int _costCard;
            int.TryParse(_playedCard.GetComponent<CardObject>()._cost.text, out _costCard);
            if (Energy.instance.CheckEnergy(_costCard))
            {
                StartCoroutine(WaitToMoveCard(_playedCard, _costCard, _card));

            }
            else
            {
                _playedCard.GetComponent<CardObject>()._cost.color = Color.red;
            }

        
      
    }

    IEnumerator WaitToMoveCard(GameObject _playedCard,int _costCard, Card _card)
    {
        if(_playedCard!=null)
        {
            yield return new WaitForSeconds(2);
            Sequence s = DOTween.Sequence();

            s.Append(_playedCard.transform.DOMove(new Vector3(_playedCard.transform.position.x, _playedCard.transform.position.y + 70f, _playedCard.transform.position.z), 1f));
            yield return new WaitForSeconds(0.1f);
            s.Join(_playedCard.transform.DORotate(new Vector3(_playedCard.transform.rotation.x, _playedCard.transform.rotation.y, _playedCard.transform.rotation.z - 50f), 1));
            yield return new WaitForSeconds(0.1f);
            s.Append(_playedCard.transform.DOMove(new Vector3(_discordPileTransform.position.x, _discordPileTransform.position.y, _discordPileTransform.position.z), 1f));
            s.Append(_playedCard.transform.DOScale(new Vector3(0, 0, 0), 1f));
            _handCards.Remove(_playedCard);
            _handCardsCard.Remove(_card);

            AddCardToDiscordPile?.Invoke(_playedCard, _costCard, _card);
            //yield return new WaitForSeconds(0.5f);
         //   Destroy(_playedCard);
        }
  
    }
    private void OnDisable()
    {
        Pile.instance.OnPickCards -= ShowCards;

    }
}
