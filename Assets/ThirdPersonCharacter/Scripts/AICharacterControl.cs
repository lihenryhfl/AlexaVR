using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        public NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Vector3 dest;                                    // destination to aim for
		private bool crouchOnArrive = false;

        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;
        }


        private void Update()
        {
			// set dest if there is one
            if (dest != null)
                agent.SetDestination(dest);

			// character is still on the way, keep moving
			if (agent.remainingDistance > agent.stoppingDistance)
				character.Move (agent.desiredVelocity, false, false);
			else {
				// character has arrived
				character.Move (Vector3.zero, false, false);
				if (crouchOnArrive == true) {
					crouchOnArrive = false;
					character.m_Crouching = true;
				}
			}
        }


        public void SetTarget(Transform target)
        {
			this.dest = target.position;
        }

		public void OnAlexaCommand(String[] commands)
		{
			// walk or run
			if (commands [3] == "walk")
				character.m_MoveSpeedMultiplier = 0.5f;
			else
				character.m_MoveSpeedMultiplier = 1f;

			// rest of the commands

			if (commands [0].Equals ("GoAndCrouch")) {
				//dest = TODO (Object).transform.position;
				crouchOnArrive = true;
			} else if (commands [0].Equals ("Crouch")) {
				character.m_Crouching = true;
			} else if (commands [0].Equals ("GoToDirection")) {
				Debug.Log (commands [1]);
				if (commands [1].Equals ("forward"))
					dest = transform.forward * Int32.Parse (commands [2]) + transform.position;
				else if (commands [1].Equals ("left"))
					dest = -transform.right * Int32.Parse (commands [2]) + transform.position;
				else if (commands [1].Equals ("right"))
					dest = transform.right * Int32.Parse (commands [2]) + transform.position;
				else if (commands [1].Equals ("backward"))
					dest = -transform.forward * Int32.Parse (commands [2]) + transform.position;
				else
					Debug.Log ("Error, GoToDirection fell through!!");
			} else
				Debug.Log ("Error, commands[0] fell through!!");

			/*
			switch (commands[0]) 
			{
				case "GoAndCrouch":
					//dest = TODO (Object).transform.position;
					crouchOnArrive = true;
					break;
				case "Crouch":
					character.m_Crouching = true;
					break;
				case "GoToDirection":
					switch (commands [1]) {
					case "forward":
						dest = transform.forward * Int32.Parse (commands [2]) + transform.position;
						break;
					case "left":
						dest = -transform.right * Int32.Parse (commands [2]) + transform.position;
						break;
					case "right":
						dest = transform.right * Int32.Parse (commands [2]) + transform.position;
						break;
					case "backward":
						dest = -transform.forward * Int32.Parse (commands [2]) + transform.position;
						break;
					default:
						break;
					}
				default:
					break;
			}*/
		}
    }
}
