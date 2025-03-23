using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDistanceAttack : MonoBehaviour
{
	[SerializeField]
	private GameObject _bulletPrefab;

	[SerializeField]
	private float _shootingCooldown = 0.5f; // Tiempo entre disparos

	private float _nextShootTime = 0f;

	void Update()
	{
		if (Input.GetMouseButtonDown(0) && Time.time >= _nextShootTime)
		{
			_nextShootTime = Time.time + _shootingCooldown;

			// Obtener la posici�n del rat�n en coordenadas del mundo
			Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mouseWorldPos.z = 0f; // Para asegurar que quede en el plano 2D

			// Calcular la direcci�n desde el jugador hacia el rat�n
			Vector2 direction = (mouseWorldPos - transform.position).normalized;

			// Calcular el �ngulo en grados para rotar el prefab y que apunte hacia la direcci�n
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));

			// Instanciar la bala en la posici�n del jugador con la rotaci�n calculada
			GameObject bullet = Instantiate(_bulletPrefab, transform.position, rotation);
			var directionalMovement = bullet.GetComponent<Directional_Actuator>();
			directionalMovement.SetAngle(angle);

			// Opcional: Si la bala tiene un script para gestionar su movimiento, podr�as pasarle la direcci�n
			// bullet.GetComponent<Bullet>().SetDirection(direction);
		}
	}
}
