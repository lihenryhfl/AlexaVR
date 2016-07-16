using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	[RequireComponent(typeof (NavMeshAgent))]
	[RequireComponent(typeof (BaddieController))]
	public class AIBaddie1Control : MonoBehaviour
	{
		public NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
		public BaddieController character { get; private set; } // the character we are controlling
		public Vector3 target;                                    // target to aim for
		public Vector3 location1;
		public Vector3 location2;
		private bool atLoc1;


		private void Start()
		{
			// get the components on the object we need ( should not be null due to require component so no need to check )
			agent = GetComponentInChildren<NavMeshAgent>();
			character = GetComponent<BaddieController>();

			agent.updateRotation = false;
			agent.updatePosition = true;
			target = location1;
			atLoc1 = true;
		}


		private void Update()
		{
			agent.SetDestination(target);

			if (agent.remainingDistance > agent.stoppingDistance)
				character.Move (agent.desiredVelocity, false, false);
			else {
				character.Move (Vector3.zero, false, false);
				if (atLoc1) {
					target = location2;
					atLoc1 = false;
				}
				else {
					target = location1;
					atLoc1 = true;
				}
			}
		}
	}
}
