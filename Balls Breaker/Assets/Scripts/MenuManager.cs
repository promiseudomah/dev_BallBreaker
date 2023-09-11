using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuManager : MonoBehaviour
{
    public Toggle muteToggle; 
    public AudioMixer mixer;
    public string parameterName = "Volume";
    
    public void Play(){

        SceneManager.LoadScene(1);
    }
    public void ToggleMute()
    {
        
        if (!muteToggle.isOn)
        {
            
            mixer.SetFloat(parameterName, -80f);
        }
        else
        {
            
            mixer.ClearFloat(parameterName);
        }
    }

}
