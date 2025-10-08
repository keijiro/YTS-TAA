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

    [SerializeField] Transform objectXform = null;
    [SerializeField] float jitterAmount = 0.3f;
    [SerializeField] float jitterWait = 0.5f;
    [SerializeField] float jitterInterval = 1;
    [SerializeField] CustomRenderTexture accTexture = null;

    async Awaitable Start()
    {
        accTexture.Initialize();
        accTexture.Update();

        for (var i = 0;; i++)
        {
            await Awaitable.WaitForSecondsAsync(jitterWait);

            var x = (Halton(i, 2) - 0.5f) * jitterAmount;
            var y = (Halton(i, 3) - 0.5f) * jitterAmount;

            var p0 = objectXform.localPosition;
            var p1 = new float3(x, y, 0);

            for (var t = 0f; t < 1f; t += Time.deltaTime / jitterInterval)
            {
                objectXform.localPosition = math.lerp(p0, p1, math.smoothstep(0, 1, t));
                await Awaitable.NextFrameAsync();
            }

            objectXform.localPosition = p1;
            await Awaitable.NextFrameAsync();

            accTexture.Update();
        }
    }
}


