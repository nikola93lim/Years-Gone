using System.Collections;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Shell : MonoBehaviour 
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private float _minForce;
    [SerializeField] private float _maxForce;
    private Rigidbody rb;

    private float _lifetime = 4f;
    private float _fadeTime = 2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        float force = Random.Range(_minForce, _maxForce);
        rb.AddForce(transform.right * force);
        rb.AddTorque(Random.insideUnitSphere * force);

        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        yield return new WaitForSeconds(_lifetime);

        float percent = 0;
        float fadeSpeed = 1 / _fadeTime;
        Material mat = _renderer.material;
        Color originalColour = mat.color;

        while (percent < 1)
        {
            percent += Time.deltaTime * fadeSpeed;
            mat.color = Color.Lerp(originalColour, Color.clear, percent);
            yield return null;
        }

        Destroy(gameObject);
    }
}

