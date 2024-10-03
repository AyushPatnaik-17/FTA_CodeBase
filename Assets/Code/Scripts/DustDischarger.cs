using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using AlligatorUtils;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Threading.Tasks;

public class DustDischarger : MonoBehaviour
{
    public Transform Target, Reference;
    public float DistanceThreshold, AlignmentThreshold;
    public float LerpTime, DischargeQuantinty, DischargeRate;

    private float _dischargeTime;


    
    public Button button;

    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private bool _isAtPosition = false;
    private void Awake()
    {
        _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        _dischargeTime = DischargeQuantinty / DischargeRate;
        button.onClick.AddListener(async delegate
        {
            _isAtPosition = !_isAtPosition;
            if (_isAtPosition)
            {
                // StartCoroutine(MoveDischarger(100f, 0f));
                await MoveDischarger(100f, 0f);
                Debug.Log("discharge moved");
                await StartDischarging();
            }
            else
            {
                await MoveDischarger(0f, 100f);
            }
        });
    }

    public void Update()
    {
        var distance = Vector3.Distance(Reference.position, Target.position);
        var yAlignment = Mathf.Abs(Reference.position.y - Target.position.y);
        //Debug.Log($"distance {distance}, yAlignment {yAlignment}");
        // bool isAlignedOnYAxis = yAlignment < AlignmentThreshold;
        bool isAlignedOnYAxis = AreTransformsAligned(Reference, Target);
        if (distance <= DistanceThreshold && !_isAtPosition && isAlignedOnYAxis)
        {
            _isAtPosition = true;
            StartDischarger();
        }
    }

    private async void StartDischarger()
    {
        if (!_isAtPosition)
        {
            "Discharger not in position".Print();
            return;
        }

        await MoveDischarger(100f, 0f);
        await StartDischarging();

    }
    private async Task MoveDischarger(float fromPercent, float toPercent)
    {
        float elapsedTime = 0f;
        while (elapsedTime <= LerpTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpFactor = elapsedTime / LerpTime;
            _skinnedMeshRenderer.SetBlendShapeWeight(0, Mathf.Lerp(fromPercent, toPercent, lerpFactor));
            await Task.Yield();
        }
         _skinnedMeshRenderer.SetBlendShapeWeight(0, toPercent);
    }

    private async Task StartDischarging()
    {
        float elapsedTime = 0f;
        while (elapsedTime <= _dischargeTime)
        {
            elapsedTime += Time.deltaTime;
            await Task.Yield();
        }
        Debug.Log("discharge complete");
        await MoveDischarger(0f, 100f);

    }

    bool AreTransformsAligned(Transform a, Transform b)
    {
        Vector3 directionAB = (b.position - a.position).normalized;
        float dotProduct = Mathf.Abs(Vector3.Dot(a.forward.normalized, directionAB));
        //Debug.Log("dotProduct: " + dotProduct);
        return dotProduct <= 1 - AlignmentThreshold;
    }
}
