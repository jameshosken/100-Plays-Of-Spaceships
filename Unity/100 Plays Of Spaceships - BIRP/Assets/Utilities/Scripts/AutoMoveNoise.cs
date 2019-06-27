using UnityEngine;

public class AutoMoveNoise : MonoBehaviour
{

    [SerializeField] private Vector3 axisAmount;
    [SerializeField] private Vector3 axisSpeeds;
    private Vector3 noiseOffsets = Vector3.zero;
    private Vector3 origin;

    // Start is called before the first frame update
    private void Start()
    {
        origin = transform.position;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 offset = new Vector3(
            (Mathf.PerlinNoise(noiseOffsets.x, 0) * 2 - 1) * axisAmount.x,
            (Mathf.PerlinNoise(noiseOffsets.y, 0) * 2 - 1) * axisAmount.y,
            (Mathf.PerlinNoise(noiseOffsets.z, 0) * 2 - 1) * axisAmount.z);

        transform.position = origin + offset;

        noiseOffsets += axisSpeeds;

    }
}
