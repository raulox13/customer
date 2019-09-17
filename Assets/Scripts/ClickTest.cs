using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickTest : MonoBehaviour
{
    Transform parent;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Canvas ending;
    [SerializeField] private Image region;
    [SerializeField] private Text title;

    private bool showDetails = false;

    private Transform place;

    private string target = "";

    private bool answerPicked = false;

    private float xlimit;

    private void Start()
    {
        parent = transform.parent;
        xlimit = region.GetComponent<RectTransform>().position.x - region.GetComponent<RectTransform>().rect.width;
        //Debug.Log(xlimit);
    }

    public void ReturnToMenu()
    {
        target = ""; place = null;
    }

    // Update is called once per frame
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Backspace)) { target = ""; place = null; }

        if (Input.mousePosition.x >= region.GetComponent<RectTransform>().position.x - region.GetComponent<RectTransform>().rect.width &&
            Input.mousePosition.y <= region.GetComponent<RectTransform>().position.y &&
            Input.mousePosition.y >= region.GetComponent<RectTransform>().position.y - region.GetComponent<RectTransform>().rect.height) ;

        else
        {

            if (Input.GetMouseButtonDown(0) && canvas.GetComponent<CanvasGroup>().alpha == 0f && !Input.GetMouseButton(1)
                && ending.GetComponent<CanvasGroup>().alpha == 0f
                )
            {
                transform.parent = null;

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 50f))
                {
                    if (hit.transform != null && hit.transform.gameObject.name != "Planet")
                    {
                        if (hit.transform.gameObject.name != target)
                        {
                            place = hit.transform;
                            
                            target = hit.transform.gameObject.name;
                        }

                        else
                        {
                            target = "";
                            place = null;
                        }
                    }
                }

                transform.parent = parent;
            }
        }

        if (place != null && place.GetComponent<MeshCollider>() && place.GetComponent<MeshCollider>().enabled == false) { target = ""; place = null; }

        title.text = target;

        if (target != "" && region.GetComponent<RectTransform>().position.x > xlimit)
        {
            region.GetComponent<RectTransform>().position = region.GetComponent<RectTransform>().position - new Vector3(10f, 0, 0);
        }

        else if (target == "" && region.GetComponent<RectTransform>().position.x < xlimit + region.GetComponent<RectTransform>().rect.width)
        {
            region.GetComponent<RectTransform>().position = region.GetComponent<RectTransform>().position + new Vector3(10f, 0, 0);
        }


    }
}
