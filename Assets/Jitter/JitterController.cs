using UnityEngine;
using Unity.Mathematics;

public sealed class JitterMovement : MonoBehaviour
{
    static float Halton(int index, int baseValue)
    {
        var result = 0f;
        var f = 1f / baseValue;
        var i = index;
        while (i > 0)
        {
            result += f * (i % baseValue);
            i /= baseValue;
            f /= baseValue;
        }
        return result;
    }

    [SerializeField] Transform _objectXform = null;
    [SerializeField] float _jitterAmount = 0.3f;
    [SerializeField] float _jitterWait = 0.5f;
    [SerializeField] float _jitterInterval = 0.5f;
    [SerializeField] CustomRenderTexture _accTexture = null;
    [SerializeField] int _indexOffset = 0;

    async Awaitable Start()
    {
        _accTexture.Initialize();
        _accTexture.Update();

        for (var i = 0; i < 21; i++)
        {
            await Awaitable.WaitForSecondsAsync(_jitterWait);

            var x = (Halton(i + _indexOffset, 2) - 0.5f) * _jitterAmount;
            var y = (Halton(i + _indexOffset, 3) - 0.5f) * _jitterAmount;

            if (i == 20) (x, y) = (0, 0); // return to center

            var p0 = _objectXform.localPosition;
            var p1 = new float3(x, y, 0);

            for (var t = 0f; t < 1f; t += Time.deltaTime / _jitterInterval)
            {
                _objectXform.localPosition = math.lerp(p0, p1, math.smoothstep(0, 1, t));
                await Awaitable.NextFrameAsync();
            }

            _objectXform.localPosition = p1;
            await Awaitable.NextFrameAsync();

            _accTexture.Update();
        }
    }
}


