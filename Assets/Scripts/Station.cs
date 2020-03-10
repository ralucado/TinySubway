using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Station : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject lineManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        lineManager.GetComponent<LineManager>().stationClicked(this.gameObject);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        lineManager.GetComponent<LineManager>().mouseReleased();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        lineManager.GetComponent<LineManager>().mouseEnteredStation(this.gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //lineManager.GetComponent<LineManager>()

    }
}
