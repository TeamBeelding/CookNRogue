using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BulletDecal : MonoBehaviour
{
    [SerializeField] DecalProjector _decalProjector;
    [SerializeField] float _timeToFade;
    [SerializeField] float _minScale;
    [SerializeField] float _maxScale;

    public void Init(Color color)
    {
        var mat = _decalProjector.material;
        mat.SetColor("_Color", color);

        //_decalProjector.material = mat;

        float randomRot = Random.Range(-360.0f, 360.0f);
        transform.rotation = Quaternion.Euler(0,randomRot,0);
        float randomScale = Random.Range(_minScale,_maxScale);

        transform.localScale = new Vector3(randomScale, randomScale, 1);
        StartCoroutine(decalFade());
    }

    IEnumerator decalFade()
    {
        yield return new WaitForSecondsRealtime(_timeToFade);
        gameObject.SetActive(false);
    }

}
