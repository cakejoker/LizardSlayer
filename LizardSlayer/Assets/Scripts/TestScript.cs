using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestScript : MonoBehaviour
{
    public InputField input;
    string SceneName = "";
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SceneChange()
    {
        SceneName = input.text;
        SceneManager.LoadScene(SceneName);
    }
}
