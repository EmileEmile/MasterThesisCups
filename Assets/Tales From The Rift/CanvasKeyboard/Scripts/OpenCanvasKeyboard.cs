using UnityEngine;
using System.Collections;

namespace TalesFromTheRift
{
	public class OpenCanvasKeyboard : MonoBehaviour 
	{
		// Canvas to open keyboard under
		public Canvas CanvasKeyboardObject;

		// Optional: Input Object to receive text 
		public GameObject inputObject;

		public void OpenKeyboard() 
		{
			Debug.Log("Keyboard open");
            CanvasKeyboard.Open(CanvasKeyboardObject, inputObject != null ? inputObject : gameObject);
        }

		public void CloseKeyboard() 
		{
            Debug.Log("Keyboard close");
            CanvasKeyboard.Close ();
		}
	}
}