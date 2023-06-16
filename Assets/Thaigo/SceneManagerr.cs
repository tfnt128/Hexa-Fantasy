using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerr : MonoBehaviour
{
    public string sceneName;
    public GameObject button1;
    public GameObject button2;
    public GameObject buttonLore;
    public GameObject loreText;
    public GameObject logo;
    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoreText()
    {
        button1.SetActive(false);   
        button2.SetActive(false);   
        logo.SetActive(false);  
        buttonLore.SetActive(true);   
        loreText.SetActive(true);   
        
    }

    public void quit()
    {
        
    }
}
