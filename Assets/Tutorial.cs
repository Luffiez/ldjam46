using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{

    public PauseMenu pauser;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        Time.timeScale = 0;
    }

    void Update()
    {
        if(Input.anyKeyDown)
        {
            anim.SetTrigger("Resume");
        }
    }

    public void Close()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
