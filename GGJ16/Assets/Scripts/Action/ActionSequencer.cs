using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GGJ16.Pooling;

namespace GGJ16
{
	public class ActionSequencer : MonoBehaviour
	{
		public List<ActionSequence> sequences = new List<ActionSequence>();

		bool active;

		void OnSpawn()
		{
			// TEST SEQUENCE
			ActionSequence sequence = new ActionSequence();

			GameObject forwardGO = GameObjectFactory.Instance.Spawn("ForwardDash", gameObject.transform, Vector3.zero, Quaternion.identity);
			ForwardDash forwardDash = forwardGO.GetComponent<ForwardDash>();
			forwardDash.source = gameObject.GetComponentInParent<Actor>().gameObject;
			forwardDash.force = 1f;
			forwardDash.duration = 0.5f;
			sequence.actions.Add(forwardDash);

			GameObject sideGO = GameObjectFactory.Instance.Spawn("SideDash", gameObject.transform, Vector3.zero, Quaternion.identity);
			SideDash sideDash = sideGO.GetComponent<SideDash>();
			sideDash.source = gameObject.GetComponentInParent<Actor>().gameObject;
			sideDash.force = 2f;
			sideDash.duration = 0.75f;
			sequence.actions.Add(sideDash);

			GameObject forwardGO2 = GameObjectFactory.Instance.Spawn("ForwardDash", gameObject.transform, Vector3.zero, Quaternion.identity);
			ForwardDash forwardDash2 = forwardGO2.GetComponent<ForwardDash>();
			forwardDash2.source = gameObject.GetComponentInParent<Actor>().gameObject;
			forwardDash2.force = 1.5f;
			forwardDash2.duration = 0.33f;
			sequence.actions.Add(forwardDash2);

			AddSequence(sequence);
		}

		public void AddSequence(ActionSequence sequence)
		{
			sequences.Add(sequence);
		}

		public void RemoveSequence(ActionSequence sequence)
		{
			sequences.Remove(sequence);
		}

		public void RunSequence(ActionSequence sequence)
		{
			if (active)
			{
				return;
			}

			StartCoroutine(RunCoroutine(sequence));
		}

		IEnumerator RunCoroutine(ActionSequence sequence)
		{
			active = true;

			int i = 0;
			while (i < sequence.actions.Count)
			{
				GameAction action = sequence.actions[i++];
				action.Invoke();
				yield return new WaitForSeconds(action.duration);
			}

			active = false;
		}
	}
}
