using UnityEngine.UI;
using System.Collections;
using UnityEngine;

public class UI_BlackCanvasDropWindow : UI_WindowBase
{
    public Image bgImage;
    private float fadeSpeed;

    public void Fade(float speed)
    {
        StopAllCoroutines();
        StartCoroutine(DoFade(speed));
    }

    private IEnumerator DoFade(float fadeSpeed)
    {
        float timer = 1;
        Color color = bgImage.color;
        color.a = 1;
        while (timer > 0)
        {
            timer -= Time.deltaTime * fadeSpeed;
            color.a = timer;
            bgImage.color = color;
            yield return null;
        }
        UIManager.Instance.CloseWindow<UI_BlackCanvasDropWindow>();
    }
}
