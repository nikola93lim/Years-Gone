using UnityEngine;

public class MuzzleFlash : MonoBehaviour 
{
    [SerializeField] private GameObject _muzzle;
    [SerializeField] private float _flashTime;
    [SerializeField] private Sprite[] _flashSprites;
    [SerializeField] private SpriteRenderer[] _spriteRenderers;

    private void Start()
    {
        Deactivate();
    }

    public void Activate()
    {
        _muzzle.SetActive(true);

        int randomIndex = Random.Range(0, _flashSprites.Length);
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _spriteRenderers[i].sprite = _flashSprites[randomIndex];
        }

        Invoke(nameof(Deactivate), _flashTime);
    }

    public void Deactivate()
    {
        _muzzle.SetActive(false);
    }
}

