using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Station : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject gameManager;
    public int drawnLines;
    // Start is called before the first frame update
    void Start()
    {
        drawnLines = 0;
    }

    // Update is called once per frame
    void Update()
    {    }

    public void drawInLine()
    {
        drawnLines += 1;
    }

    public void eraseLines()
    {
        drawnLines = 0;
    }

    public int getNumberOfDrawnLines()
    {
        return drawnLines;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        gameManager.GetComponent<GameManager>().stationClicked(this.gameObject);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        gameManager.GetComponent<GameManager>().mouseReleased();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameManager.GetComponent<GameManager>().mouseEnteredStation(this.gameObject);
        transform.localScale = new Vector3(1.1f,1.1f,1.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //lineManager.GetComponent<GameManager>()
        transform.localScale = Vector3.one;
    }
}
