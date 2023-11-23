using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class BulletDecal : MonoBehaviour
{
    [Header("Projector")]
    [SerializeField] DecalProjector _decalProjector;

    [Header("Scale Values")]
    [SerializeField] float _minScale;
    [SerializeField] float _maxScale;
    [SerializeField] AnimationCurve _scaleCurve;
    [SerializeField] float _ScalingSpeed;
    float scale;

    [Header("Alpha Values")]
    [SerializeField] float _minAlpha;
    [SerializeField] float _maxAlpha;
    [SerializeField] float _alphaFadeSpeed;
    float _alpha;
    [SerializeField] AnimationCurve _alphaCurve;

    
    Color _decalColor;

    public void Init(Color color)
    {
        StopAllCoroutines();
        float randomRot = Random.Range(-360.0f, 360.0f);
        transform.rotation = Quaternion.Euler(0,randomRot,0);
        float randomScale = Random.Range(_minScale,_maxScale);
        scale = randomScale;
        transform.localScale = new Vector3(scale, scale, 1);
        StartCoroutine(decalFade(color));
    }
   
    IEnumerator decalFade(Color color)
    {
        _decalProjector.material = new Material(_decalProjector.material);
        var mat = _decalProjector.material;
        _alpha = Random.Range(_minAlpha, _maxAlpha);
        color.a = _alpha;
        _decalColor = color;
        mat.SetColor("_Color", _decalColor);

        //SCALE
        float scaleProgress = 0;
        while (scaleProgress < 1)
        {
            float newScale = scale * _scaleCurve.Evaluate(scaleProgress);
            transform.localScale = new Vector3(newScale, newScale, 1);
            scaleProgress += Time.deltaTime * _ScalingSpeed;
            yield return new WaitForEndOfFrame();
        }

        //FADE
        float alphaProgress = 0;
        while (alphaProgress < 1)
        {
            float newAlpha = _alpha * _alphaCurve.Evaluate(alphaProgress);
            _decalColor.a = newAlpha;
            mat.SetColor("_Color", _decalColor);
            alphaProgress += Time.deltaTime * _alphaFadeSpeed;
            yield return new WaitForEndOfFrame();
        }

        gameObject.SetActive(false);
    }

}
