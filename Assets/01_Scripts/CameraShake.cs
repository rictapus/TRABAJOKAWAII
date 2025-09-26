using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Intensidad del nerviosismo")]
    public float positionAmount = 0.02f;   // cu�nto se mueve la c�mara
    public float rotationAmount = 0.5f;    // cu�nto rota en grados
    public float frequency = 3f;           // rapidez de las variaciones

    private Vector3 originalPos; //guardar el valor original para que no se vaya muy lejos
    private Quaternion originalRot; //Lo mismo con la rotacion de la camara

    void Start()
    {
        originalPos = transform.localPosition;
        originalRot = transform.localRotation;
    }

    void Update()
    {
        float noiseX = (Mathf.PerlinNoise(Time.time * frequency, 0f) - 0.5f) * 2f; //PerlinNoise es un codigo que entrega numeros random dentro de un parametro ej:-1 y 1 pero que cambia de manera fluida y suave al contrario de random que te tira numeros al azar
        float noiseY = (Mathf.PerlinNoise(0f, Time.time * frequency) - 0.5f) * 2f;

        Vector3 posOffset = new Vector3(noiseX, noiseY, 0) * positionAmount;// Peque�o temblor en posici�n

        transform.localPosition = originalPos + posOffset;

        Quaternion rotOffset = Quaternion.Euler(noiseY * rotationAmount, noiseX * rotationAmount, 0);// Peque�o temblor en rotaci�n

        transform.localRotation = originalRot * rotOffset;
    }
}
