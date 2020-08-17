using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NoiseScaler : MonoBehaviour
{
    public List<Transform> objects;

    private Vector3[] sizes;
    private float offset;
    private Vector3[] positions;

    // Start is called before the first frame update
    void Start()
    {
        sizes = new Vector3[objects.Count];
        positions = new Vector3[objects.Count];

        var i = 0;
        objects.ForEach(o =>
        {
            positions[i] = o.position;
            o.localScale = new Vector3(Random.value < 0.5f ? -1 : 1, Random.value < 0.5f ? -1 : 1, 1);
            sizes[i] = o.localScale;
            i++;
        });
    }

    // Update is called once per frame
    void Update()
    {
        var i = 0;
        objects.ForEach(o =>
        {
            var noise = Mathf.PerlinNoise(o.position.x + offset, o.position.y);
            var x = Mathf.PerlinNoise(o.position.x - offset, o.position.y);
            var y = Mathf.PerlinNoise(o.position.x, o.position.y + offset);
            o.transform.localScale = sizes[i] * (1f - 0.4f * Mathf.Abs(noise));
            o.position = positions[i] + Vector3.left * x * 0.15f + Vector3.up * y * 0.1f;
            i++;
        });

        offset += Time.deltaTime;
    }
}
