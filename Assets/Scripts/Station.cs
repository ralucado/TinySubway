using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(Collider))]
public class Station : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler {

    public GameObject gameManager;
    public event Action<GameObject> a_clicked;
    public event Action<GameObject> a_entered;
    public event Action a_released;

    public int drawnLines;
    // Start is called before the first frame update
    void Start() {
        drawnLines = 0;
    }
    public void Clicked() {
        a_clicked?.Invoke(this.gameObject);
    }
    public void Released() {
        a_released?.Invoke();
    }
    public void Entered() {
        a_entered?.Invoke(this.gameObject);
    }


    // Update is called once per frame
    void Update() {
        //CheckCollider();
    }

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

    public void OnPointerDown(PointerEventData eventData) {
        Clicked();
    }

    public void OnPointerUp(PointerEventData eventData){
        Released();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Entered();
        transform.localScale = new Vector3(1.1f,1.1f,1.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }
}
