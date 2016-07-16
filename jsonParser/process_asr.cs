using System;
using System.Web;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace AlexaVR
{
	class MainClass
	{
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

        public string[] processASR(string inputASR)
        {

			HashSet<string> objects = new HashSet<string>{"couch", "table", "light", "bed", "stool", "chair", "fan", "wall"};
			HashSet<string> directions = new HashSet<string>{"left", "right", "forward", "backward", "around"};
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

			string[] rets = new string[3];

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
					action = "ToGoDirection";
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
				rets [2] = unit;
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

		//Hide, Next, Stop, Left
		public string[] processJson(string inputJsonFile)
		{
			string jsonS = System.IO.File.ReadAllText (inputJsonFile);
			JObject jo = JObject.Parse (jsonS);
			string[] ret = new string[3];

			HashSet<string> objects = new HashSet<string>{"table", "couch", "chair"};
			HashSet<string> directions = new HashSet<string>{"left", "right", "forward", "around"};
			//objects.Add ("table");
			//objects.Add ("couch");
			//objects.Add ("chair");
			//objects.Add ("light");
			string[] toks = jo ["request"] ["intent"] ["slots"] ["Action"] ["value"].ToString().Split();

			foreach (string tok in toks) {
				if (objects.Contains (tok)) {
					return new string[3]{ "GoTo", tok, "" };
				} else if (directions.Contains (tok)) {
					//int unit = 0;
					return new string[3]{ "ToGo", tok, "unit"};
				} else {
					Console.Write ("yes");
					return new string[3]{ "Crouch", "", "" };
				}
			}
			return ret;
			//foreach (string tok in toks) {
			//}
			//Console.WriteLine (toks);
			//string[] cmds = new string[4];
			//return cmds;
		}
		public static void Main (string[] args)
		{
			MainClass m = new MainClass();
			string[] rets = m.processASR ("go to the table");
			foreach (string s in rets) {
				Console.WriteLine (s);
			}
			//m.processJson ("/Users/xiaochp/code/AlexaVR/sim.json");
			//Console.WriteLine ("Hello World!");
		}
	}
}
