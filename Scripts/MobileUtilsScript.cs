using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MobileUtilsScript : MonoBehaviour
{

    private int FramesPerSec;
    private float frequency = 1.0f;
    private string fps;
    public Text text;


    void Start()
    {
        StartCoroutine(FPS());
    }

    private IEnumerator FPS()
    {
        for (; ; )
        {
            // Capture frame-per-second
            int lastFrameCount = Time.frameCount;
            float lastTime = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(frequency);
            float timeSpan = Time.realtimeSinceStartup - lastTime;
            int frameCount = Time.frameCount - lastFrameCount;

            // Display it

            fps = string.Format("FPS: {0}", Mathf.RoundToInt(frameCount / timeSpan));
            text.text = fps;
        }
    }
}