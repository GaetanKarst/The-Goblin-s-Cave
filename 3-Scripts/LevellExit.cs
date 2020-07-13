using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevellExit : MonoBehaviour
{
    [SerializeField] float delayToLoad = 2f;
    [SerializeField] GameObject fadeNextLevel;
    ParticleSystem particules;
    

    private void Start()
    {
        fadeNextLevel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.tag == "Player")
        {
            Debug.Log("Go to next Level");
            fadeNextLevel.SetActive(true);
            StartCoroutine(LoadNextLevel());  
        }
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(delayToLoad);
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
