using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReticleScript : MonoBehaviour
{

    [SerializeField] TMP_Text interactText;
    [SerializeField] Image reticle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(interactText.text == "" || !interactText.gameObject.activeInHierarchy)
        {
            reticle.gameObject.SetActive(true);
        }
        else
        {
            reticle.gameObject.SetActive(false);
        }
    }


}
