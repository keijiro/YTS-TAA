using UnityEngine;

public sealed class NaiveScene : MonoBehaviour
{
    [SerializeField] Camera _camera = null;
    [SerializeField] CustomRenderTexture _accTexture = null;
    [SerializeField] float _jitterAmount = 0.01f;

    void Start()
      => _accTexture.Initialize();

    void Update()
    {
        var i = Time.frameCount;
        var x = (Jitter.Halton(i, 2) - 0.5f) * _jitterAmount;
        var y = (Jitter.Halton(i, 3) - 0.5f) * _jitterAmount;
        _camera.transform.localPosition = new Vector3(x, y, 0);
    }
}
