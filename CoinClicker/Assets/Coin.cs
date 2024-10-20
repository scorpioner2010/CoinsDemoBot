using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Coin : MonoBehaviour
{
    public Image floatingImage;
    public float floatSpeed = 100f;
    public float lifeTime = 1f;
    
    public void Init()
    {
        StartCoroutine(AnimateFloatingImage(floatingImage));
    }
    
    private System.Collections.IEnumerator AnimateFloatingImage(Image floatingImage)
    {
        float elapsedTime = 0f;

        while (elapsedTime < lifeTime)
        {
            floatingImage.transform.localPosition += Vector3.up * floatSpeed * Time.deltaTime;
            Color newColor = floatingImage.color;
            newColor.a = Mathf.Lerp(1, 0, elapsedTime / lifeTime);
            floatingImage.color = newColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(floatingImage.gameObject);
    }
}
