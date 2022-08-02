using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class JsonController : MonoBehaviour
{
    public static JsonController instance;


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
    public string jsonURL;
   

    public List<Card> Deck = new List<Card>();
    private List<Card> PickedCards = new List<Card>();
    private CardList cardList;

   // public Action<List<Card>> OnPickCards;
    public Action<List<Card>> OnGeneratePile;
   
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetData());
    }

    IEnumerator GetData()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(jsonURL))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                GetFromJsonData(www.downloadHandler.text);
            }

        }
    }
    public List<int> CardsAlgorithm = new List<int>();//4,3,2



    private void GetFromJsonData(string _url)
    {
        cardList = JsonUtility.FromJson<CardList>(_url); // 1 types : defense
       
        for (int i = 0; i < 3; i++)//second type
        {
            Card pickedCard = GetRandomElements<Card>(cardList.cards, 1).First();

            cardList.cards.RemoveAll(x => (x.effects.First().type == pickedCard.effects.First().type));//remove all spell cards of the same effect

            for (int j = 0; j < CardsAlgorithm[i]; j++)
            {
                Deck.Add(pickedCard);
                //Pile.instance.Deck = Deck;
            }
        }
        OnGeneratePile?.Invoke(Deck);
      //  PickedCards = GetRandomElements<Card>(Deck, 4);
       // OnPickCards?.Invoke(PickedCards);

        // CardsManager.instance.ShowCards(PickedCards);
    }
    #region Ext
    public static List<t> GetRandomElements<t>(IEnumerable<t> list, int elementsCount)
    {
        return list.OrderBy(x => Guid.NewGuid()).Take(elementsCount).ToList();
    }
    #endregion
}
