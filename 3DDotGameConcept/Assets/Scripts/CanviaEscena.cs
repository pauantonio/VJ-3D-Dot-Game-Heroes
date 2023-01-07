using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanviaEscena : MonoBehaviour
{
    public string scene;
    private bool finished;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToScene()
    {
        StartCoroutine(waitForSound());
    }

    IEnumerator waitForSound()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(scene);
    }

    public void QuitGame() 
    {
        Application.Quit();
    }
}
