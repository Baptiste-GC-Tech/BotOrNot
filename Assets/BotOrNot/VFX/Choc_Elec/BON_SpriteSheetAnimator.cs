using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class BON_SpriteSheetAnimator : MonoBehaviour
{
    [Header("Sprite Sheet Settings")]
    public int columns = 5;  // 2500px / 500px = 5
    public int rows = 8;     // 4000px / 500px = 8
    public float framesPerSecond = 30f;

    [Header("Control")]
    public bool play = false;
    public bool stop = false;

    private Renderer _renderer;
    private int totalFrames;
    private float timer;
    private int currentFrame;
    private bool _isPlaying = false;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        totalFrames = columns * rows;

        // Ajuste l'échelle UV pour correspondre à une seule frame
        _renderer.material.SetTextureScale("_MainTex", new Vector2(1f / columns, 1f / rows));
    }

    void Update()
    {
        if (play && !_isPlaying)
        {
            _isPlaying = true;
            stop = false;
            timer = 0f;
            currentFrame = 0;
        }

        if (stop && _isPlaying)
        {
            _isPlaying = false;
            play = false;
        }

        if (_isPlaying && totalFrames > 0)
        {
            timer += Time.deltaTime;

            if (timer >= 1f / framesPerSecond)
            {
                timer -= 1f / framesPerSecond;
                currentFrame = (currentFrame + 1) % totalFrames;

                // Calcul de la position UV
                float uIndex = currentFrame % columns;
                float vIndex = currentFrame / columns;

                Vector2 offset = new Vector2(uIndex / (float)columns, 1f - (vIndex + 1) / (float)rows);
                _renderer.material.SetTextureOffset("_MainTex", offset);
            }
        }
    }
}
