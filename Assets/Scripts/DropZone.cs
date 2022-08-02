using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IDropHandler
{
    [Header ("DropZone")]
    [SerializeField]
    private Transform parent;
   

    private Vector2 inialposition;
   


 
    private void Start()
    {
        inialposition = this.transform.position;
    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " ondrop object to dead zone " + gameObject.name);


        if (eventData.pointerDrag.TryGetComponent<DraggableCard> (out var dgc))
        {
            dgc.originalParent = parent;
            dgc.finalposition = inialposition;
            dgc.GetComponent<Button>().enabled = true;
            dgc.GetComponent<Button>().OnSubmit(eventData);
       
            StartCoroutine(waiteforfewseconds(dgc));
          
          
        }

    }

    IEnumerator waiteforfewseconds(DraggableCard dg)
    {
        yield return new WaitForSeconds(0.5f);
        dg.enabled = false;
        dg.GetComponent<Button>().enabled = false;
    }
}
