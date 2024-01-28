using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIController : MonoBehaviour
{
    [SerializeField] private GameObject _controlsObj;
    [SerializeField] private GameObject _creditsObj;
    [SerializeField] private GameObject _storyObj;

    public void BtnEvt_LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void BtnEvt_OpenGlitchCityTwitter()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer)
            Application.OpenURL("https://twitter.com/GlitchCityLA");
    }

    public void BtnEvt_ShowControls()
    {
        StartCoroutine(ShowUntilKeypress(_controlsObj, KeyCode.Escape));
    }

    public void BtnEvt_ShowCredits()
    {
        StartCoroutine(ShowUntilKeypress(_creditsObj, KeyCode.Escape));
    }

    public void BtnEvt_ShowStory()
    {
        StartCoroutine(ShowUntilKeypress(_storyObj, KeyCode.Escape));
    }

    private IEnumerator ShowUntilKeypress(GameObject obj, KeyCode key)
    {
        obj.SetActive(true);
        yield return new WaitUntil(() => Input.GetKeyDown(key));
        obj.SetActive(false);
    }
}
