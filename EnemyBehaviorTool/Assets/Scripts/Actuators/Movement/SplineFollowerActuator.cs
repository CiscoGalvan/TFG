using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(Rigidbody2D))]
public class SplineFollowerActuator : Actuator
{
    private enum TeleportToClosestPoint
    {
        None = 0,
        Player = 1,
        Spline = 2
    }
    [SerializeField] private SplineContainer _splineContainer;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private TeleportToClosestPoint _teleportToClosestPoint = TeleportToClosestPoint.None;
    private Rigidbody2D _rb;
    private float _distancePercentage=0;
    float splinelength;
    float _distanceBetweenPoints;
    public override void StartActuator()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_splineContainer == null || _splineContainer.Spline == null)
        {
            Debug.LogError("SplineContainer no asignado o vacío.");
            enabled = false;
            return;
        }
        splinelength = _splineContainer.CalculateLength();
        _distanceBetweenPoints = 0.05f;
        //pasar el gameobject a spline (punto más cercano)
        _distancePercentage = GetClosestPointOnSpline(transform.position);
        switch (_teleportToClosestPoint)
        {
            case TeleportToClosestPoint.Player:
                Vector3 currentPosition = _splineContainer.EvaluatePosition(_distancePercentage);
                transform.position = new Vector3(currentPosition.x, currentPosition.y, 0);
                break;
            case TeleportToClosestPoint.Spline:
                // Encuentra el punto más cercano en el spline
               
                Vector3 closestPoint = _splineContainer.EvaluatePosition(_distancePercentage);

                // Desplaza la spline sin deformarla
                Vector3 offset = transform.position - closestPoint;
                _splineContainer.transform.position += offset;
                break;
            

        }
       
       
      
       
       

       
    }

    public override void UpdateActuator()
    {


        // Avanzar en la curva segun la velocidad
        _distancePercentage += _speed  * Time.deltaTime / splinelength;

        Vector3 currentPosition = _splineContainer.EvaluatePosition(_distancePercentage); 
        Vector3 nextPosition = _splineContainer.EvaluatePosition(_distancePercentage + _distanceBetweenPoints);
        // transform.position = new Vector3(currentPosition.x, currentPosition.y, 0);
        _rb.MovePosition(new Vector2(currentPosition.x, currentPosition.y));
        Vector3 direction = (nextPosition - currentPosition);
        Vector3 directionnormalized = (nextPosition - currentPosition).normalized;
        // _rb.velocity = new Vector2(directionnormalized.x, directionnormalized.y) * _speed;
      
        // Si el spline es cerrado, se reinicia el progreso al llegar al final
        if (_distancePercentage > 1f)
        {
            _distancePercentage = 0f;
            Vector3 c = _splineContainer.EvaluatePosition(0);
            transform.position = new Vector3(c.x, c.y, 0);
        }


        // Rotar en el eje Z
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _rb.MoveRotation(angle);
    }

    public void SetSpeed(float newSpeed)
    {
        _speed = newSpeed;
    }

    public override void DestroyActuator()
    {
        
    }

    public float GetSpeed()
    {
        return _speed;
    }
    private float GetClosestPointOnSpline(Vector3 position)
    {
        float closestDistance = float.MaxValue;
        float closestT = 0f;
        float step = 0.01f; 

        for (float t = 0f; t <= 1f; t += step)
        {
            Vector3 splinePoint = _splineContainer.EvaluatePosition(t);
            float distance = Vector3.Distance(position, splinePoint);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestT = t;
            }
        }

        return closestT;
    }
  
}
