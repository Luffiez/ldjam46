using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public static Transition instance;

    Animator anim;
    string sceneToLoad = "Menu";

    private void Awake()
    {
        if (instance)
            Destroy(gameObject);
        else
            instance = this;

        DontDestroyOnLoad(transform.root);
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        anim.SetBool("TransitionTo", false);
        Debug.Log("Loaded scene!");
        if(SceneManager.GetActiveScene().name == "Game")
        {
            MusicManager.Instance.ChangeBgmSong(MusicManager.Instance.gameMusic);
        }
        else
        {
            MusicManager.Instance.ChangeBgmSong(MusicManager.Instance.menuMusic);
        }
    }

    public void Play()
    {
        sceneToLoad = "Game";
        anim.SetBool("TransitionTo", true);
    }

    public void Menu()
    {
        sceneToLoad = "Menu";
        anim.SetBool("TransitionTo", true);
    }

    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
            SceneManager.LoadScene(sceneToLoad);
        else
            Debug.LogWarning("sceneToLoad has not been specified!");
    }
}
