using UnityEngine;

public class UI_ClickerIndicator : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float maxScale = 1.5f;

    [Header("Indicator Infos")]
    [SerializeField] private Renderer indicatorRenderer;

    private Material indicatorMat;
    private Vector3 startScale;
    private float timer;

    private void Awake()
    {
        if (indicatorRenderer == null)
            indicatorRenderer = GetComponent<Renderer>();
       
        startScale = transform.localScale;  //prendo la scala iniziale

        indicatorMat = new Material(indicatorRenderer.material);
        indicatorRenderer.material = indicatorMat;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        float progress = timer / duration;

        transform.localScale = Vector3.Lerp(startScale, startScale * maxScale, progress);   //espansione graduale

        var color = indicatorMat.color;
        color.a = 1 - progress;    //cambio la trasparenza del materiale, 1 visibile 0 invisibile
        indicatorMat.color = color;

        if (timer >= duration)
            Destroy(gameObject);
    }
}