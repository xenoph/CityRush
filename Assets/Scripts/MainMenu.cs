using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public GameObject keyBindings;
    public GameObject options;

    void Awake() {
        GameManager.Instance.SetGameState(GameState.StartMenu);
    }

    public void StartLevel1Button() {
        Application.LoadLevel("Level_01");
        GameManager.Instance.SetGameState(GameState.Level1);
    }

    public void StartTutorialButton() {
        Application.LoadLevel("Tutorial");
        GameManager.Instance.SetGameState(GameState.Tutorial1);
    }

    public void QuitGameButton() {
        Application.Quit();
    }
    //Keybind menu
    public void KeybindActive()
    {
        keyBindings.gameObject.SetActive(true);
    }
    public void KeybindDisable()
    {
        keyBindings.gameObject.SetActive(false);
    }

    //Options menu
    public void OptionsActive()
    {
        options.gameObject.SetActive(true);
    }
    public void OptionsDisable()
    {
        options.gameObject.SetActive(false);
    }
}