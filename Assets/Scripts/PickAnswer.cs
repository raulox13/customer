using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickAnswer : MonoBehaviour
{
    [SerializeField] Transform planet;
    [SerializeField] Text title;
    [SerializeField] Text description;
    [SerializeField] Text statements;
    [SerializeField] RawImage picture;
    [SerializeField] Image nature;
    [SerializeField] Image pollution;
    [SerializeField] Text barText;
    [SerializeField] Text question;

    private Button[] buttons;

    private int correctButton = 4;

    private float xlimit, ammount = 0;

    private int barChanger = 0;

    private List<Transform> regions = new List<Transform>();

    private void RemoveCollider(int b)
    {
        if (transform.GetComponent<RectTransform>().position.x <= xlimit)// && correctButton != 4)
        {
            //Debug.Log(correctButton);

            if (b == correctButton)
            {
                ammount = 0.5f;
            }

            else ammount = -1f;

            barChanger = (int)(ammount * 10);

            transform.GetComponent<RectTransform>().position += new Vector3(transform.GetComponent<RectTransform>().rect.width, 0, 0);
            correctButton = 4;

            foreach (Transform region in regions)
            {
                if (region.name == title.text)
                {
                    string effect = barChanger > 0 ? "Good" : "Bad";

                    if (region.Find(effect))
                    {
                        foreach(MeshRenderer mesh in region.Find(effect).GetComponentsInChildren<MeshRenderer>())
                        {
                            mesh.enabled = true;
                        }
                    }
                    
                    region.GetComponent<MeshCollider>().enabled = false;
                    break;
                }
            }

            title.text = "";
        }
    }

    public void FirstButton(int b = 0) { RemoveCollider(b); }

    public void SecondButton(int b = 1) { RemoveCollider(b); }

    public void ThirdButton(int b = 2) { RemoveCollider(b); }

    public void FourthButton(int b = 3) { RemoveCollider(b); }

    // Start is called before the first frame update
    private void Start()
    {
        xlimit = transform.GetComponent<RectTransform>().position.x - transform.GetComponent<RectTransform>().rect.width;

        buttons = transform.GetComponentsInChildren<Button>();

        foreach(Transform region in planet.GetComponentsInChildren<Transform>())
        {
            if (region.GetType() != transform.GetComponent<RectTransform>().GetType() && 
                region.GetComponent<MeshCollider>())
            {
                //Debug.Log(region.name);
                regions.Add(region);
            }
        }

        //Debug.Log(regions.Count);
    }

    // Update is called once per frame
    private void Update()
    {
        float timer = Time.deltaTime * 2;
        float barLimit = 0.05f;

        if (barChanger != 0)
        {
            barText.enabled = true;
            barText.text = (ammount > 0 ? "+" : "") + barChanger;
            barText.color = ammount > 0 ? nature.color : pollution.color;

            if (ammount > 0)
            {
                nature.fillAmount += 0.1f * timer;
                pollution.fillAmount -= 0.1f * timer;
                ammount -= timer;
            }

            else if (ammount < 0)
            {
                nature.fillAmount -= 0.1f * timer;
                pollution.fillAmount += 0.1f * timer;
                ammount += timer;
            }

            if (ammount < barLimit && ammount > -barLimit)
            {
                if (ammount < 0)
                {
                    float f = pollution.fillAmount;
                    
                    if ((int)(f * 100) % 5 != 0)
                    {
                        //Debug.Log((int)(f * 10 + (barChanger < 0 ? 1 : -1)));
                        pollution.fillAmount = (int)(f * 100 + (barChanger < 0 ? 1 : -1)) / 100f;
                        nature.fillAmount = 1f - pollution.fillAmount;
                    }
                }

                else
                {
                    float f = nature.fillAmount;

                    if ((int)(f * 100) % 10 != 0)
                    {
                        //Debug.Log((int)(f * 100));
                        nature.fillAmount = (float)((int)(f * 100 + (barChanger > 0 ? 1 : -1)) / 100f);
                        pollution.fillAmount = 1f - nature.fillAmount;
                    }
                }

                //Debug.Log(nature.fillAmount + " " +pollution.fillAmount);

                barChanger = 0;
            }
        }

        else
        {
            barText.enabled = false;
        }

        foreach(Transform region in regions)
        {
            if (region.name == title.text)
            {
                if (region.Find("Description"))
                    description.text = region.Find("Description").GetComponentInParent<Text>().text;
                else description.text = "Miaw";

                if (region.Find("Statements"))
                    statements.text = region.Find("Statements").GetComponentInParent<Text>().text;
                else statements.text = "Ouf";

                if (region.Find("Picture"))
                    picture.texture = region.Find("Picture").GetComponentInParent<RawImage>().texture;
                else picture.texture = null;

                if (region.Find("Button"))
                {
                    for (int i = 0; i < buttons.Length; i++)
                    {
                        if(region.Find("Button").GetComponentInParent<Text>().text == buttons[i].name)
                        {
                            buttons[0].GetComponentInChildren<Text>().text = "1st";
                            buttons[1].GetComponentInChildren<Text>().text = "2nd";
                            buttons[2].GetComponentInChildren<Text>().text = "both";
                            buttons[3].GetComponentInChildren<Text>().text = "none";

                            question.text = "Which statements are correct?";
                            correctButton = i;
                            break;
                        }

                        else
                        {
                           if ((region.Find("Button").GetComponentInParent<Text>().text == "A" && buttons[i].name == "1st")  ||
                               (region.Find("Button").GetComponentInParent<Text>().text == "B" && buttons[i].name == "2nd")  ||
                               (region.Find("Button").GetComponentInParent<Text>().text == "C" && buttons[i].name == "Both") ||
                               (region.Find("Button").GetComponentInParent<Text>().text == "D" && buttons[i].name == "None"))  
                           {
                                correctButton = i;
                                buttons[0].GetComponentInChildren<Text>().text = "A";
                                buttons[1].GetComponentInChildren<Text>().text = "B";
                                buttons[2].GetComponentInChildren<Text>().text = "C";
                                buttons[3].GetComponentInChildren<Text>().text = "D";

                                question.text = "Which answer is correct?";
                                break;
                           }
                        }
                    }
                }
            }
        }
    }
}
