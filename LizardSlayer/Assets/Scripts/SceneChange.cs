using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStart()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenMenual()
    {
        GameObject.Find("Canvas").transform.Find("Menual").gameObject.SetActive(true);
    }

    public void CloseMenual()
    {
        GameObject.Find("Menual").SetActive(false);
    }
}
