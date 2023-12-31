﻿using UnityEngine;
using UnityEngine.SceneManagement;
namespace SG.Util
{
	/// <summary> Utility class to reset or quit Unity Scenes. Can be called from anywhere trhough static functions, or be placed in a scene to access via buttons / hotkeys. </summary>
	public class SG_SceneControl : MonoBehaviour
	{
		//--------------------------------------------------------------------------------------------------------------------------
		// Member Variables

		/// <summary> Hotkey to reset the current scene </summary>
		public KeyCode resetKey = KeyCode.R;
		public KeyCode SceneChangeKey = KeyCode.F4;

		/// <summary> Hotkey to shut down the application </summary>
		public KeyCode quitKey = KeyCode.Escape;


		//--------------------------------------------------------------------------------------------------------------------------
		// Functions

		/// <summary> Resets the current scene. </summary>
		/// <remarks> Placed in a static function so we can call it from anywhere. </remarks>
		public static void ResetScene()
		{
			if (Application.isPlaying)
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
			}
		}


		public static void CompareScene()
		{
			if (Application.isPlaying)
			{
				string currentSceneName = SceneManager.GetActiveScene().name;
				Debug.Log(currentSceneName);
				if (currentSceneName == "13_VR_Tryout OriginTest")
                {
					SceneManager.LoadScene("13_VR_Tryout Origin");
				}
				else if (currentSceneName == "13_VR_Tryout Origin")
                {					
					SceneManager.LoadScene("13_VR_Tryout OriginTest");					
				}
				
			}
		}

		/// <summary> Resets the current scene </summary>
		/// <remarks> nonstatic function to call via Buttons / UI. </remarks>
		public void ResetCurrent()
		{
			SG_SceneControl.ResetScene();
		}

		/// <summary> Shuts down this application. </summary>
		/// <remarks> Placed in a static function so we can call it from anywhere. </remarks>
		public static void QuitApplication()
		{
			Application.Quit();
		}


		/// <summary> Shuts down this application </summary>
		/// <remarks> nonstatic function to call via Buttons / UI. </remarks>
		public void Quit()
        {
			SG_SceneControl.QuitApplication();
		}

		


		/// <summary> Reset to the first scene. </summary>
		public static void ToFirstScene()
        {
			UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		}

		/// <summary> Reset back to the initial Scene </summary>
		public void ResetSimulation()
        {
			SG_SceneControl.ToFirstScene();
        }

		//--------------------------------------------------------------------------------------------------------------------------
		// Monobehaviour

		// Update is called once per frame
		void Update()
		{
#if ENABLE_INPUT_SYSTEM //if Unitys new Input System is enabled....
#else
			if (Input.GetKeyDown(resetKey))
			{
				ResetScene();
			}
			if(Input.GetKeyDown(SceneChangeKey))
			{
				CompareScene();
			}
			else if (Input.GetKeyDown(quitKey))
			{
				Quit();
			}
#endif
		}
	}
}