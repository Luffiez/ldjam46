using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HsCreditPanel : MonoBehaviour
{
    Animator anim;
    public GameObject hsPanel, creditsPanel;
    [SerializeField]
    AudioClip ClickSound;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ShowHighscore()
    {
        MusicManager.Instance.PlayOneShot(ClickSound, 2);
        if(hsPanel.activeSelf)
            hsPanel.SetActive(false);
        else
            hsPanel.SetActive(true);

        if (!creditsPanel.activeSelf)
        {
            anim.SetTrigger("Click");
        }
        creditsPanel.SetActive(false);
    }

    public void ShowCredits()
    {
        MusicManager.Instance.PlayOneShot(ClickSound, 2);
        if (creditsPanel.activeSelf)
            creditsPanel.SetActive(false);
        else
            creditsPanel.SetActive(true);
        
        if (!hsPanel.activeSelf)
        {
            anim.SetTrigger("Click");
        }
        hsPanel.SetActive(false);
    }
}
