using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBTemplate.Patterns.Singleton
{
    public class AutoSingleton<T> : MonoBehaviour where T : AutoSingleton<T>
    {
		private static T _instance;
		private static object _lock = new object();
		public static T Instance
		{
			get
			{
				if (!Application.isPlaying)
				{
					Debug.LogError("Application is not playing");
					return null;
				}

				lock (_lock)
				{
					if (!CheckIfExist())
					{
						_instance = (T)FindObjectOfType(typeof(T));

						if (!CheckIfExist())
						{
							GameObject singleton = new GameObject();
							_instance = singleton.AddComponent<T>();
							singleton.name = typeof(T).ToString();
						}
					}

					return _instance;
				}
			}
		}
		public static bool CheckIfExist()
        {
			if (_instance == null) return false;
			return true;
        }
		public void Awake()
		{
			// check if there's another instance already exist in scene
			if (_instance != null && _instance.GetInstanceID() != this.GetInstanceID())
			{
				// Destroy this instances because already exist the singleton of EventsDispatcer
				Debug.Log($"An instance of EventDispatcher already exist : <{ _instance.name}>, So destroy this instance : <{name}>!!");
				Destroy(gameObject);
			}
			else
			{
				// set instance
				_instance = this as T;
				_instance.OnCreatedSingleton();
			}
		}
		public void OnDestroy()
		{
			if (_instance == this)
			{
				_instance = null;
				OnDestroySingleton();
			}
		}
		public virtual void OnCreatedSingleton()
		{

		}
		public virtual void OnDestroySingleton()
		{

		}
	}
}

