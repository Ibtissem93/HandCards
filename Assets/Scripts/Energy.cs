using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Energy : MonoBehaviour
{
    public static Energy instance;
    public int _energyCounter;
    [SerializeField]
    private Text _energyCounterTxt;

  


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
    void Start()
    {
        _energyCounter = 4;
    }

    // Update is called once per frame
    void Update()
    {
        _energyCounterTxt.text = _energyCounter.ToString() + "/" + "4";
    }

   public void DesincreamentEnergie(int _cost)
    {
       _energyCounter -= _cost;
        
    }

    public bool CheckEnergy(int _cardCost)
    {
       
        if (_cardCost > _energyCounter || _energyCounter == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
