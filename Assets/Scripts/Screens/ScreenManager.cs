using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ScreenManager : MonoBehaviour
{
    public GameObject deathScreen; // Asigna el GameObject de la pantalla de muerte en el inspector
    public TextMeshPro deadText; // Asigna el Text del DeadText en el inspector
    public float transitionDuration = 2.0f; // Duración de la transición

    public void ShowDeathScreen()
    {
        deathScreen.SetActive(true);
        StartCoroutine(TransitionDeadText());
    }

    public void HideDeathScreen()
    {
        deathScreen.SetActive(false);
    }

    private IEnumerator TransitionDeadText()
    {
        float elapsedTime = 0f;
        float softness = 1f;
        float dilate = 1f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);

            softness = Mathf.Lerp(1f, 0f, t);
            dilate = Mathf.Lerp(1f, 0f, t);

            UpdateDeadTextEffects(softness, dilate);

            yield return null;
        }

        UpdateDeadTextEffects(0f, 0f);
    }

    private void UpdateDeadTextEffects(float softness, float dilate)
    {
        Outline outline = deadText.GetComponent<Outline>();
        outline.effectColor = new Color(0, 0, 0, softness);
        outline.effectDistance = new Vector2(dilate, -dilate);
    }
}
