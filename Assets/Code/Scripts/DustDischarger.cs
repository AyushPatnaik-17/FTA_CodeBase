using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using AlligatorUtils;
using UnityEngine.UI;
using Unity.VisualScripting;

public class DustDischarger : MonoBehaviour
{
    public Transform Target;
    public Transform Reference;
    public float DistanceThreshold, AlignmentThreshold;
    public float LerpTime, DischargeTime, DischargeQuantinty, DischargeRate;
    
    public Button button;

    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private bool _isDischarging = false;
    private void Awake()
    {
        _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        button.onClick.AddListener(delegate
        {
            _isDischarging = !_isDischarging;
            if (_isDischarging)
            {
                StartCoroutine(MoveDischarger(100f, 0f));
                //conver this to async and then start discharging
            }
            else
            {
                StartCoroutine(MoveDischarger(0f, 100f));
            }
        });
    }

    public void Update()
    {
        var distance = Vector3.Distance(Reference.position, Target.position);
        var yAlignment = Mathf.Abs(Reference.position.y - Target.position.y);
        Debug.Log($"distance {distance}, yAlignment {yAlignment}");
        // bool isAlignedOnYAxis = yAlignment < AlignmentThreshold;
        bool isAlignedOnYAxis = AreTransformsAligned(Reference, Target);
        if (distance <= DistanceThreshold && !_isDischarging && isAlignedOnYAxis)
        {
            _isDischarging = true;
            StartCoroutine(MoveDischarger(100f, 0f));
        }
        // if(_isDischarging)
        // {
        //     _isDischarging = false;
        //     StartCoroutine(MoveDischarger(0f, 100f));
        // }
    }

    private IEnumerator MoveDischarger(float fromPercent, float toPercent)
    {
        float elapsedTime = 0f;
        while (elapsedTime <= LerpTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpFactor = elapsedTime / LerpTime;
            _skinnedMeshRenderer.SetBlendShapeWeight(0, Mathf.Lerp(fromPercent, toPercent, lerpFactor));
            yield return null;
        }
    }

    private IEnumerator StartDischarging()
    {
        yield return null;
    }

    bool AreTransformsAligned(Transform a, Transform b)
    {
        Vector3 directionAB = (b.position - a.position).normalized;
        float dotProduct = Mathf.Abs(Vector3.Dot(a.forward.normalized, directionAB));
        Debug.Log("dotProduct: " + dotProduct);
        return dotProduct <= 1 - AlignmentThreshold;
    }
}
