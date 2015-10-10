using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUI : MonoBehaviour {
    public static Vector2 player_ScreenPos;
    private static Image aim;
    private static Image xButton;
    private static Image energyBar; private static Vector2 energyBar_startSize;
    private static Canvas canvas;
    // Use this for initialization
    void Start() {
        canvas = GameObject.FindObjectOfType<Canvas>();
        Image[] images = GameObject.FindObjectsOfType<Image>();
        foreach (Image img in images)
        {
            if (img.name == "Aim") aim = img;
            if (img.name == "EnergyBar_front") energyBar = img;
            if (img.name == "XButton") xButton = img;
        }
        energyBar_startSize = energyBar.rectTransform.sizeDelta;
    }
    // Update is called once per frame
    void Update() {
        player_ScreenPos = RectTransformUtility.WorldToScreenPoint(Player.playerCam, Player.player.transform.position);
        DrawAim();
        DrawXButton();
        DrawEnergyBar();
    }
    private void DrawAim()
    {
        Vector2 target = Player.pad_right.sqrMagnitude > 1.0f ? player_ScreenPos + Player.pad_right * 200.0f : player_ScreenPos + Player.pad_aim * 80.0f;
        Vector2 pos = (target - (Vector2)aim.transform.position) * Time.deltaTime * 6.0f;
        aim.transform.position += (Vector3)pos;
    }
    private void DrawXButton()
    {
        xButton.gameObject.SetActive(false);
    }
    private void DrawEnergyBar()
    {
        Vector2 size = energyBar.rectTransform.sizeDelta;
        size.x = energyBar_startSize.x * ((float)Player.currentEnergy / (float)Player.player.energy);
        energyBar.rectTransform.sizeDelta = size;
    }
}
