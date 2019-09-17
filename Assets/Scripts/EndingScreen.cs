using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingScreen : MonoBehaviour
{
    [SerializeField] private Texture firstBack;
    [SerializeField] private Texture secondBack;
    [SerializeField] private Texture thirdBack;
    [SerializeField] private Texture fourthBack;
    [SerializeField] private Texture firstPersona;
    [SerializeField] private Texture secondPersona;
    [SerializeField] private Texture thirdPersona;
    [SerializeField] private Texture fourthPersona;

    [SerializeField] private Text firstDesc;
    [SerializeField] private Text secondDesc;
    [SerializeField] private Text thirdDesc;
    [SerializeField] private Text fourthDesc;

    [SerializeField] private RawImage background;
    [SerializeField] private RawImage persona;
    [SerializeField] private Text description;
    [SerializeField] private Text title;
    [SerializeField] private Image pollution;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void ChangeScreen(Texture b, Texture p, Text d, string t)
    {
        background.texture = b;
        persona.texture = p;
        description.text = d.text;
        title.text = t;

    }

    // Update is called once per frame
    void Update()
    {
        if(pollution.fillAmount >= 0.75f)
        {
            ChangeScreen(firstBack, firstPersona, firstDesc, "Bad and Doomed");
        }

        else if(pollution.fillAmount >= 0.5f)
        {
            ChangeScreen(secondBack, secondPersona, secondDesc, "Uninformed");
        }

        else if(pollution.fillAmount >= 0.25f)
        {
            ChangeScreen(thirdBack, thirdPersona, thirdDesc, "Average");
        }

        else
        {
            ChangeScreen(fourthBack, fourthPersona, fourthDesc, "Planet Saver");
        }
    }
}
