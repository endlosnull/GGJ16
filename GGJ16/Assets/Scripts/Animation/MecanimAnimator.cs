using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GGJ16
{
	[RequireComponent(typeof(Animator))]
	public class MecanimAnimator : MonoBehaviour
	{
		public delegate void Callback();

		public Animator animator;

		int[] previousTags;
		Dictionary<int, Delegate> tagBeginCallbackMap = new Dictionary<int, Delegate>();
		Dictionary<int, Delegate> tagEndCallbackMap = new Dictionary<int, Delegate>();

		void Reset()
		{
			animator = GetComponent<Animator>();
		}

		void Awake()
		{
			if (animator == null)
			{
				return;
			}

			previousTags = new int[animator.layerCount];
		}

		void Update()
		{
			if (animator == null)
			{
				return;
			}

			UpdateCallbacks();
		}

		void UpdateCallbacks()
		{
			for (int i = 0; i < animator.layerCount; ++i)
			{
				int tempTag = animator.GetCurrentAnimatorStateInfo(i).tagHash;

				int previousTag = previousTags[i];
				if (tempTag != previousTag)
				{
					Delegate endCallback;
					if (tagEndCallbackMap.TryGetValue(previousTag, out endCallback))
					{
						((Callback)endCallback)();
					}

					Delegate beginCallback;
					if (tagBeginCallbackMap.TryGetValue(tempTag, out beginCallback))
					{
						((Callback)beginCallback)();
					}

					previousTag = tempTag;
				}
			}
		}

		public int GetControlId(string parameter)
		{
			return Animator.StringToHash(parameter);
		}

		public int GetCurrentTag(int layer)
		{
			return animator.GetCurrentAnimatorStateInfo(layer).tagHash;
		}

		public bool GetBool(int id)
		{
			return animator.GetBool(id);
		}

		public int GetInteger(int id)
		{
			return animator.GetInteger(id);
		}

		public float GetFloat(int id)
		{
			return animator.GetFloat(id);
		}

		public void SetTrigger(int id)
		{
			animator.SetTrigger(id);
		}

		public void SetBool(int id, bool value)
		{
			animator.SetBool(id, value);
		}

		public void SetInteger(int id, int value)
		{
			animator.SetInteger(id, value);
		}

		public void SetFloat(int id, float value)
		{
			animator.SetFloat(id, value);
		}

		public void RegisterCallback(Dictionary<int, Delegate> dictionary, int hash, Callback callback)
		{
			if (dictionary.ContainsKey(hash))
			{
				dictionary[hash] = (Callback)dictionary[hash] + callback;
			}
			else
			{
				dictionary.Add(hash, callback);
			}
		}

		public void UnregisterCallback(Dictionary<int, Delegate> dictionary, int hash, Callback callback)
		{
			if (dictionary.ContainsKey(hash))
			{
				dictionary[hash] = (Callback)dictionary[hash] - callback;
			}
		}

		public void RegisterOnTagBegin(int tagHash, Callback callback)
		{
			RegisterCallback(tagBeginCallbackMap, tagHash, callback);
		}

		public void UnregisterOnTagBegin(int tagHash, Callback callback)
		{
			UnregisterCallback(tagBeginCallbackMap, tagHash, callback);
		}

		public void RegisterOnTagEnd(int tagHash, Callback callback)
		{
			RegisterCallback(tagEndCallbackMap, tagHash, callback);
		}

		public void UnregisterOnTagEnd(int tagHash, Callback callback)
		{
			UnregisterCallback(tagEndCallbackMap, tagHash, callback);
		}
	}
}
