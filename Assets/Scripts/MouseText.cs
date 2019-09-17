using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseText : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Canvas menu;
    [SerializeField] private Canvas ending;
    [SerializeField] private Image region;

    private CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        canvas.GetComponentInChildren<Image>().GetComponentInChildren<Text>().text = "none";
        canvasGroup = canvas.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        canvas.enabled = false;
    }

    private void Update()
    {
        canvas.GetComponentInChildren<Image>().GetComponent<RectTransform>().position = Input.mousePosition;

        if (Input.mousePosition.x >= region.GetComponent<RectTransform>().position.x - region.GetComponent<RectTransform>().rect.width &&
            Input.mousePosition.y <= region.GetComponent<RectTransform>().position.y  &&
            Input.mousePosition.y >= region.GetComponent<RectTransform>().position.y - region.GetComponent<RectTransform>().rect.height || Input.GetMouseButton(1))
        {
            canvas.enabled = false;
            canvasGroup.alpha = 0f;
        }

        else
        {
            if (Input.GetMouseButtonUp(1) && canvas.GetComponentInChildren<Image>().GetComponentInChildren<Text>().text != "none")
                canvasGroup.alpha = 1f;

            if (canvas.GetComponentInChildren<Image>().GetComponentInChildren<Text>().text != "none" &&
                menu.GetComponent<CanvasGroup>().alpha <= 0.5f)
            {
                canvas.enabled = true;
                canvasGroup.alpha = 1f;
            }

            else
            {
                canvas.enabled = false;
                canvasGroup.alpha = 0f;
            }
        }
    }

    private void OnMouseEnter()
    {
                if(ending.GetComponent<CanvasGroup>().alpha == 0f)
                canvas.GetComponentInChildren<Image>().GetComponentInChildren<Text>().text = transform.name;
    }

    private void OnMouseExit()
    {
        canvas.GetComponentInChildren<Image>().GetComponentInChildren<Text>().text = "none";
    }
}
