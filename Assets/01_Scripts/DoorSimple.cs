using UnityEngine;

public class SimpleDoor : MonoBehaviour
{
    public Transform rotateTarget;
    public float distanceToOpen = 3f;
    public float openAngle = 90f;
    public float rotateSpeed = 60f;

    Transform player;
    Quaternion startRot, targetRot;
    bool opening, opened;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (rotateTarget == null) rotateTarget = transform;
        startRot = rotateTarget.localRotation;
        targetRot = startRot * Quaternion.Euler(0f, openAngle, 0f);
    }

    void Update()
    {
        if (opened || player == null) return;

        if (!opening && Vector3.Distance(player.position, transform.position) <= distanceToOpen)
            opening = true;

        if (opening)
        {
            rotateTarget.localRotation = Quaternion.RotateTowards(rotateTarget.localRotation, targetRot, rotateSpeed * Time.deltaTime);
            if (Quaternion.Angle(rotateTarget.localRotation, targetRot) < 0.5f)
            {
                rotateTarget.localRotation = targetRot;
                opened = true;
            }
        }
    }
}
