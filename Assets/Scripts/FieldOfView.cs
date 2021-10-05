using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float _viewRadius;
    private float _secondaryRadius;

    [Range(0,360)] public float _viewAngle;
    public float _meshResolution;

    private MeshFilter _viewMeshFilter;
    private Mesh _viewMesh;

    public LayerMask _targetMask;
    public LayerMask _obstacleMask;

    bool _isSearching;

    public EnemyMovement _enemyMovement;
    public Gunner _enemyGun;
    private EnemiesManager _manager;

    private List<Transform> _targetsInSight = new List<Transform>();

    public int edgeResolveIteration;
    public float edgeDistanceThreshold;

    private void Awake()
    {
        _viewMesh = new Mesh();
        _viewMesh.name = "View Mesh";

        _viewMeshFilter = GetComponent<MeshFilter>();
        if (_viewMeshFilter)
        {
            _viewMeshFilter.mesh = _viewMesh;
        }
        
        _isSearching = true;
        _manager = FindObjectOfType<EnemiesManager>();

        _secondaryRadius = _viewRadius *0.75f;
    }

    private void Update()
    {
        if (_isSearching && _enemyMovement)
        {
            SearchTarget();

            bool playerInSight = false;
            foreach (Transform target in _targetsInSight)
            {
                if (target.CompareTag("Player"))
                {
                    playerInSight = true;
                    if (!_enemyMovement.IsInPursuit)
                    {
                        CharacterAbilities abilities = target.gameObject.GetComponent<CharacterAbilities>();
                        if (abilities && !abilities.IsInvisible)
                        {
                            _enemyMovement.SpotTarget(target);
                            _enemyGun.SpotTarget(target.gameObject);
                            _manager.GetAlerted();
                        }
                    }
                }
                else if(target.CompareTag("Target") && _manager)
                {
                    WanderAroundSpawn targetComp = target.gameObject.GetComponent<WanderAroundSpawn>();
                    if (targetComp && !targetComp.enabled)
                    {
                        _manager.GetAlerted(true);
                    }
                }
            }
            if (!playerInSight)
            {
                Collider[] targetInRadius = Physics.OverlapSphere(transform.position, _secondaryRadius, _targetMask, 
                    QueryTriggerInteraction.Ignore);
                bool isPlayerInSecondaryRadius = false;
                foreach (Collider coll in targetInRadius)
                {
                    if (coll.CompareTag("Player"))
                    {
                        isPlayerInSecondaryRadius = true;
                        break;
                    }
                }

                if (_enemyMovement.IsInPursuit && !isPlayerInSecondaryRadius)
                {
                    _enemyMovement.StopPursuit();
                }
                if (_enemyGun.HasTarget)
                {
                    _enemyGun.LoseTarget();
                }
            }
        }
    }
    private void LateUpdate()
    {
        DrawFieldOfView();
    }

    void SearchTarget()
    {
        _targetsInSight.Clear();
        Collider[] targetInRadius = Physics.OverlapSphere(transform.position, _viewRadius, _targetMask, QueryTriggerInteraction.Ignore);
        if (targetInRadius.Length ==0)
        {
            return;
        }
        List<Transform> inSight = new List<Transform>();

        for (int i = 0; i < targetInRadius.Length; i++) 
        {
            Transform target = targetInRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < _viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, _obstacleMask))
                {
                     inSight.Add(target);
                }
            }
        }
        _targetsInSight = inSight;
    }

    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(_viewAngle * _meshResolution);
        float stepAngleSize = _viewAngle / stepCount;

        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i < stepCount; i++)
        {
            float angle = transform.eulerAngles.y - _viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast._dst - newViewCast._dst) > edgeDistanceThreshold;
                if (oldViewCast._hit != newViewCast._hit || (oldViewCast._hit && newViewCast._hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge._pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge._pointA);
                    }
                    if (edge._pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge._pointB);
                    }
                }
            }

            viewPoints.Add(newViewCast._point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int triCount = (vertexCount - 2) * 3;
        int[] triangles = new int[triCount];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        _viewMesh.Clear();
        _viewMesh.vertices = vertices;
        _viewMesh.triangles = triangles;
        _viewMesh.RecalculateNormals();
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast._angle;
        float maxAngle = maxViewCast._angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIteration; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast._dst - newViewCast._dst) > edgeDistanceThreshold;
            if (newViewCast._hit == minViewCast._hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast._point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast._point;
            }
        }
        return new EdgeInfo(minPoint, maxPoint);
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirectionFromAngle(globalAngle, true);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, _viewRadius, _obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * _viewRadius, _viewRadius, globalAngle);
        }
    }

    public Vector3 DirectionFromAngle(float angle, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angle += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad) ,0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    

    public struct ViewCastInfo
    {
        public bool _hit;
        public Vector3 _point;
        public float _dst;
        public float _angle;

        public ViewCastInfo(bool hit, Vector3 point, float dst, float angle)
        {
            _hit = hit;
            _point = point;
            _dst = dst;
            _angle = angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 _pointA;
        public Vector3 _pointB;

        public EdgeInfo(Vector3 pointA, Vector3 pointB)
        {
            _pointA = pointA;
            _pointB = pointB;
        }
    }

}
