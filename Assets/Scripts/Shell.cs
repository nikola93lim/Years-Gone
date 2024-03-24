using System.Collections;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Shell : Flyweight 
{
    private Rigidbody rb;
    private Renderer _renderer;
    private Material _material;
    private Color _originalColour;
    new ShellSettings Settings => (ShellSettings)base.Settings;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _renderer = GetComponentInChildren<Renderer>();

        _material = _renderer.material;
        _originalColour = _material.color;
    }
    private void OnEnable()
    {
        float force = Random.Range(Settings.MinForce, Settings.MaxForce);
        rb.AddForce(transform.right * force);
        rb.AddTorque(Random.insideUnitSphere * force);

        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        yield return new WaitForSeconds(Settings.Lifetime);

        float percent = 0;
        float fadeSpeed = 1 / Settings.FadeTime;
        

        while (percent < 1)
        {
            percent += Time.deltaTime * fadeSpeed;
            _material.color = Color.Lerp(_originalColour, Color.clear, percent);
            yield return null;
        }

        FlyweightFactory.ReturnToPool(this);
    }

    public void ResetColour()
    {
        _material.color = _originalColour;
    }
}

