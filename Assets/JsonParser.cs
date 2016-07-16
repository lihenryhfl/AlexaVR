using UnityEngine;
using System.Collections;
using System;
//using System.Web;
using System.Collections.Generic;

public class JsonParser : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public static Boolean crouch_action(string utterance) {
		if(utterance.Contains("crouch")) {
			return true;
		}
		if(utterance.Contains("hide")) {
			if (utterance.Contains("under") || utterance.Contains("below")) {
				return true;
			}
		}
		return false;
	}

	public static Boolean run_type(string utterance) {
		if (utterance.Contains ("run") || utterance.Contains ("sprint") || utterance.Contains ("fast")) {
			return true;
		}
		return false;
	}

	public string[] processASR(string inputASR)
	{
		HashSet<string> objects = new HashSet<string>{"couch", "table", "light", "bed", "stool", "chair", "fan", "wall"};
		HashSet<string> directions = new HashSet<string>{"left", "right", "east", "west", "south", "north", "forward", "backward", "around"};
		HashSet<string> distances = new HashSet<string> {
			"one",
			"two",
			"three",
			"four",
			"five",
			"six",
			"seven",
			"eight",
			"nine",
			"ten"
		};

		Hashtable dist_to_num = new Hashtable();
		dist_to_num ["one"] = "1";
		dist_to_num ["two"] = "2";
		dist_to_num ["three"] = "3";
		dist_to_num ["four"] = "4";
		dist_to_num ["five"] = "5";
		dist_to_num ["six"] = "6";
		dist_to_num ["seven"] = "7";
		dist_to_num ["eight"] = "8";
		dist_to_num ["nine"] = "9";
		dist_to_num ["ten"] = "10";

		string[] rets = new string[4];

		string action = "";
		string target = "";
		string unit = "";

		foreach (string obj in objects) {
			if (inputASR.Contains(obj)) {
				action = "GoToObject";
				target = obj;
				break;
			}
		}

		if (run_type (inputASR))
			rets [3] = "run";
		else
			rets [3] = "walk";

		if (action.Length != 0) {
			if (crouch_action(inputASR)) {
				action = "GoAndCrouch";
				rets [0] = action;
				rets [1] = target;
				rets [2] = "";
				return rets;
			}
			rets [0] = action;
			rets [1] = target;
			rets [2] = "";
			return rets;
		}

		foreach (string dire in directions) {
			if (inputASR.Contains(dire)) {
				action = "GoToDirection";
				target = dire;
				break;
			}
		}

		if (action.Length != 0) {
			foreach(string dist in distances) {
				if (inputASR.Contains(dist)) {
					unit = dist;
					break;
				}
			}

			rets [0] = action;
			rets [1] = target;
			if (unit.Equals ("")) {
				unit = "three";
			}
			rets [2] = dist_to_num[unit].ToString();
			return rets;
		}

		if (crouch_action (inputASR)) {
			action = "Crouch";
			rets [0] = action;
			rets [1] = "";
			rets [2] = "";
			return rets;
		}
		return rets;
	}
}
