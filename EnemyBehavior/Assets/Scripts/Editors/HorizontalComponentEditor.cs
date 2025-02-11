using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Horizontal))]
public class HorizontalComponentEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		//Coge la lista de sensores y por cada sensor nos creamos una lista en el editor, donde se pondrán los eventos a los que reaccionamos que son del tipo Action/UnityAction y con eso rellenamos el diccionario de la clase Horizontal.
		//Este funcionamiento tendrá que ser para todos los actuators así que a lo mejor deberíamos abstraer algo más este comportamiento.
	}

}
