using Obi;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;
using System;
using AYellowpaper.SerializedCollections;
using System.Linq;
using AlligatorUtils;

public enum MovementType
{
    UpDownOnly,
    WithCross
}

[Serializable]
public struct TravelDistance
{
    public float MinDistance;
    public float MaxDistance;
}
public class HoistBehaviour : MonoBehaviour
{
    private ControllerSetup _controllerSetup;
    private InputAction _cross, _vertical, _switcher, _malfunction, _reset;
    private float _moveAmtCross = 0f, _moveAmtVert = 0f;
    private Rigidbody _rigidbody;
    private float _initialDrag;


    [SerializeField] private float _maxSpeed = 10f, _minRopeLength = 0f, _malfunctionLength = 20f;
    public MovementType HoistType;
    public Transform Hoist;
    public TravelDistance TravelDistance = new() { MinDistance = 0f, MaxDistance = 10f };
    public SerializedDictionary<ObiRopeCursor, ObiRope> CursorRopes = new();

    private void Awake()
    {
        InitControls();
        _rigidbody = GetComponent<Rigidbody>();
        _initialDrag = _rigidbody.drag;
        _minRopeLength = CursorRopes.Values.First().restLength;

    }

    private void InitControls()
    {
        _controllerSetup = new();

        _cross = _controllerSetup.Hoist.Cross;
        _vertical = _controllerSetup.Hoist.Vertical;
        _switcher = _controllerSetup.Hoist.Switcher;
        _malfunction = _controllerSetup.Hoist.Malfunction;
        _reset = _controllerSetup.Hoist.Reset;

        _switcher.performed += ctx => SwitchHoist();
        _malfunction.performed += ctx => WreckHavoc();
        _reset.performed += ctx => ResetRope();
    }

    private void SwitchHoist()
    {
        Debug.Log("Switching Hoist");
    }

    public ForceMode ForceMode = ForceMode.Impulse;
    private void WreckHavoc()
    {
        _rigidbody.drag = 0f;

        foreach (var cursor in CursorRopes.Keys)
        {
            var rope = CursorRopes[cursor];

            float malfunctionLength = _malfunctionLength - rope.CalculateLength();
            cursor.ChangeLength(malfunctionLength);

            _rigidbody.AddForce(Vector3.down * 50f, ForceMode);
        }
    }
    private void ResetRope()
    {
        "Reset called".Print();
        if(_rigidbody.drag < _initialDrag) _rigidbody.drag = _initialDrag;
        foreach(var cursor in CursorRopes.Keys)
        {
            float currLength = CursorRopes[cursor].CalculateLength();
            cursor.ChangeLength(_minRopeLength - currLength);
        }
        
    }

    private void FixedUpdate()
    {  
        _moveAmtCross = _cross.ReadValue<Vector2>().x;
        _moveAmtVert = _vertical.ReadValue<Vector2>().y;

        PerformUpDown();

        if(HoistType == MovementType.WithCross && Hoist != null) PerformCross();
    }

    private Vector3 _currentVelocity;
    public float VelocityDampening = 1f;
    private void PerformCross()
    {
        
        Vector3 targetPosition = Hoist.position + (Vector3.forward * _moveAmtCross * _maxSpeed);
        float clampedDistance = Mathf.Clamp(targetPosition.z, TravelDistance.MinDistance, TravelDistance.MaxDistance);
        //  Hoist.position = new Vector3(Hoist.position.x, targetPosition.y, Mathf.Lerp(Hoist.position.z, targetPosition.z, Time.deltaTime));
        targetPosition.z = Mathf.Clamp(targetPosition.z, TravelDistance.MinDistance, TravelDistance.MaxDistance);
        Hoist.position = Vector3.SmoothDamp
        (
            Hoist.position,
            new Vector3(Hoist.position.x, Hoist.position.y, targetPosition.z),
            ref _currentVelocity,
            VelocityDampening
        );
        
    }
    private void PerformUpDown()
    {
        //TODO: Implement 1-9 logic
        float change = _moveAmtVert * Time.deltaTime * _maxSpeed;
        foreach(var kvp in CursorRopes)
        {
            var cursor = kvp.Key;
            var rope = kvp.Value;

            if(rope.restLength + change < _minRopeLength) change = _minRopeLength - rope.restLength;

            cursor.ChangeLength(change);
        }
    }

    

    private void OnEnable()
    {
        _controllerSetup.Hoist.Enable();
    }
    private void OnDisable()
    {
        foreach(var cursor in CursorRopes.Keys)
        {
            cursor.ChangeLength(CursorRopes[cursor].restLength);
        }
        _controllerSetup.Hoist.Disable();
    }

}

#region custom inspector
#if UNITY_EDITOR
[CustomEditor(typeof(HoistBehaviour))]
public class HoistBehaviourEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty iterator = serializedObject.GetIterator();
        iterator.NextVisible(true);

        while (iterator.NextVisible(false))
        {
            if (iterator.name == "Hoist" || iterator.name == "TravelDistance")
            {
                if (((HoistBehaviour)target).HoistType == MovementType.WithCross)
                {
                    EditorGUILayout.PropertyField(iterator, true);
                }
            }
            else
            {
                EditorGUILayout.PropertyField(iterator, true);
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion