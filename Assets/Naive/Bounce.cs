using UnityEngine;
using Unity.Mathematics;

public sealed class Bounce : MonoBehaviour
{
    [field:SerializeField] float2 Frequency = new float2(1f, 2.3f);
    [field:SerializeField] float2 Amplitude = new float2(1, 1);
    [field:SerializeField] float Delay = 3;

    void Update()
    {
        var t = Frequency * math.max(0, Time.time - Delay);
        var x = math.sin(2 * math.PI * t.x) * Amplitude.x;
        var y = math.frac(t.y) * 2 - 1;
        y = (1 - y * y) * Amplitude.y;
        transform.localPosition = new float3(x, y, 0);
    }
}
