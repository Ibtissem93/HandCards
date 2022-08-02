using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerEnterHandler,IPointerExitHandler
{
 
    [HideInInspector]
    public Transform originalParent = null;
    [HideInInspector]
    public Vector2 finalposition;

    [Header ("Canvas Group")]
    [SerializeField]
    private CanvasGroup cansvasgroup;

    private Vector2 initialposition;
    private bool _disabletranslate;

    private void OnEnable()
    {
        initialposition = this.transform.position;

    }

   

    public void OnBeginDrag(PointerEventData eventData)
    {
        
        cansvasgroup.blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        cansvasgroup.blocksRaycasts = true;
        if (originalParent != null)
        {
            this.transform.SetParent(originalParent);
            this.transform.position = finalposition;
            _disabletranslate = true;
        }
        else
        {
            _disabletranslate = false;
            this.transform.position = initialposition;

        }


    }

  

    public void OnPointerEnter(PointerEventData eventData)
    {
        if ( _disabletranslate == false)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 100f, transform.position.z);
            //   transform.DOMove(new Vector3(transform.position.x, transform.position.y + 300f, transform.position.z), 0.5f);
        }



    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if ( _disabletranslate == false)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 100f, transform.position.z);
            //transform.DOMove(new Vector3(transform.position.x, transform.position.y - 300f, transform.position.z), 0.5f);
        }

    }

    
}
