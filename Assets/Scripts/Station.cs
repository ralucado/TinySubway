﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Station : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject gameManager;
    private int drawnLines;
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
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //lineManager.GetComponent<GameManager>()

    }
}
