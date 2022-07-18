using UnityEngine;

public class Mark : MonoBehaviour
{
    [SerializeField] private Gradient gradient;
    [SerializeField] private float evaporationStrength = 0.01f;

    private float gradientValue = 1f;
    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();

        InvokeRepeating("Evaporate", 0.5f, 0.2f);
    }

    private void Evaporate()
    {
        if (gradientValue < 0.1f)
        {
            Destroy(gameObject);
        }

        gradientValue -= evaporationStrength;
        _renderer.color = gradient.Evaluate(gradientValue);
    }
}
