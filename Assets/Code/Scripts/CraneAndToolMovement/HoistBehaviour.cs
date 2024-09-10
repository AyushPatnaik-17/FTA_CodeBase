using Obi;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;
using System;
using AYellowpaper.SerializedCollections;
using System.Linq;
using AlligatorUtils;

public enum Direction
{
    None,Up,Down,Left,Right,Forward,Backward
}
public enum MovementType
{
    UpDownOnly,WithCross
}

[Serializable]
public struct TravelDistance
{
    public float MinDistance, MaxDistance;
}

[Serializable]
public struct ObiSolverData
{
    public float OgVelocity, OgAngVelocity, SetVel, SetAngVel;

    public ObiSolverData(float ogVel, float ogAng, float setVel, float setAng)
    {
        OgVelocity = ogVel;
        OgAngVelocity = ogAng;
        SetVel = setVel;
        SetAngVel = setAng;
    }
}
public class HoistBehaviour : MonoBehaviour
{
    public ObiSolver ObiSolver;
    public MovementType HoistType;
    public GameObject TrackedObject;
    public Transform Hoist;
    public ForceMode ForceMode = ForceMode.Impulse;
    public Direction DirectionToMove = Direction.None;
    public SerializedDictionary<ObiRopeCursor, ObiRope> CursorRopes = new();
    public TravelDistance TravelDistance = new() { MinDistance = 0f, MaxDistance = 10f };
    
    [Range(0f,1f)]
    public float VelocityDampening = 0.5f;
    [Range(0,200)]
    public int MalfunctionForceAmt = 50;


    [SerializeField] 
    private float _maxSpeed = 10f, _minRopeLength = 0f, _malfunctionLength = 20f;
    private float _moveAmtCross = 0f, _moveAmtVert = 0f;
    private float _initialRbDrag;
    private ObiSolverData _obisolverData;
    private ControllerSetup _controllerSetup;
    private InputAction _cross, _vertical, _switcher, _malfunction, _reset;
    private Rigidbody _rigidbody;
    private Vector3 _currentVelocity;



    private void Awake()
    {
        InitControls();
        _obisolverData = new ObiSolverData
                        (
                            ogVel: ObiSolver.parameters.maxVelocity, 
                            ogAng: ObiSolver.parameters.maxAngularVelocity,
                            setVel: 5f, 
                            setAng: 5f
                        );
        _rigidbody = GetComponent<Rigidbody>();
        _initialRbDrag = _rigidbody.drag;
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
        _malfunction.performed += ctx => TriggerMalfunction();
        _reset.performed += ctx => ResetRope();
    }

    private void SwitchHoist()
    {
        Debug.Log("Switching Hoist");
    }

    private void TriggerMalfunction()
    {
        _rigidbody.drag = 0f;

        SetObiSolverProperties
        (
            vel: _obisolverData.SetVel, 
            angVel: _obisolverData.SetAngVel
        );

        foreach (var cursor in CursorRopes.Keys)
        {
            var rope = CursorRopes[cursor];

            float malfunctionLength = _malfunctionLength - rope.CalculateLength();
            cursor.ChangeLength(malfunctionLength);

            _rigidbody.AddForce(Vector3.down * MalfunctionForceAmt, ForceMode);
        }
    }

    private void SetObiSolverProperties(float vel , float angVel)
    {
        ObiSolver.parameters.maxVelocity = vel;
        ObiSolver.parameters.maxAngularVelocity = angVel;
    }
    private void ResetRope()
    {
        "Reset called".Print();

        if(_rigidbody.drag < _initialRbDrag) 
            _rigidbody.drag = _initialRbDrag;

        SetObiSolverProperties
        (
            vel: _obisolverData.OgVelocity, 
            angVel: _obisolverData.OgAngVelocity
        );

        foreach(var cursor in CursorRopes.Keys)
        {
            ObiRope rope = CursorRopes[cursor];
            float currRopeLength = rope.CalculateLength();
            //switch _minRopeLength with last length maybe.
            cursor.ChangeLength(_minRopeLength - currRopeLength);
        }
        
    }

    // void Update()
    // {
    //     float distance = Vector3.Distance(transform.position, TargetObject.position);

    //     if (distance <= DetectionRadius)
    //     {
    //         Vector3 direction = (TargetObject.position - transform.position).normalized;
    //         DetermineToDirection(direction);
    //         //MultidimensionalCheck(direction);
    //     }
    // }
    private void FixedUpdate()
    {  
        DirectionToMove.Print("Move ", "yellow");
        _moveAmtCross = _cross.ReadValue<Vector2>().x;
        _moveAmtVert = _vertical.ReadValue<Vector2>().y;

        PerformUpDown();

        if(HoistType == MovementType.WithCross && Hoist != null) PerformCrossTravel();
    }

    private void PerformCrossTravel()
    {
        
        Vector3 targetPosition = Hoist.position + (Vector3.forward * _moveAmtCross * _maxSpeed);

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

        // dont remove Time.delta time,  when i removed it, it started behaving weirdly, idk why; didnt bother figuring out, try it if you want. if it doesnt malfunction then my bad fam
        float change = _moveAmtVert * Time.deltaTime * _maxSpeed; 

        foreach(var kvp in CursorRopes)
        {
            var cursor = kvp.Key;
            var rope = kvp.Value;

            if(rope.restLength + change < _minRopeLength) change = _minRopeLength - rope.restLength;

            cursor.ChangeLength(change);
        }
    }

    private void MultidimensionalCheck(Vector3 direction)
    {
        if (direction.x > 0)
        {
            Debug.Log("Move Right");
        }
        else if (direction.x < 0)
        {
            Debug.Log("Move Left");
        }

        if (direction.y > 0)
        {
            Debug.Log("Move Up");
        }
        else if (direction.y < 0)
        {
            Debug.Log("Move Down");
        }

        if (direction.z > 0)
        {
            Debug.Log("Move Forward");
        }
        else if (direction.z < 0)
        {
            Debug.Log("Move Backward");
        }
    }

    private void OnEnable()
    {
        _controllerSetup.Hoist.Enable();
    }
    private void OnDisable()
    {
        foreach (var cursor in CursorRopes.Keys)
        {
            ObiRope rope = CursorRopes[cursor];
            cursor.ChangeLength(rope.restLength);
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