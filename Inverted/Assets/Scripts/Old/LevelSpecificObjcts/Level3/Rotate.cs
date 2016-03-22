using UnityEngine;
using System.Collections;
/// <summary>
/// Rotate is attached to the tree top object(attached to the falling tree) in level 3 - scene 2, 
/// so that after the tree top falls it would rotate to achieve the rotationg falling animation effect
/// </summary>
public class Rotate : MonoBehaviour
{
	
		public Transform _trans;
		public float speed;
		public bool isRotating = false;

		void Start ()
		{
				_trans = transform;
		}

		void Update ()
		{
				if (isRotating) {
						Vector3 angle = _trans.eulerAngles;
						angle.z += speed * Time.deltaTime;
						_trans.eulerAngles = angle;
				}
		}
}
