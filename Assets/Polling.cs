using UnityEngine;
using System.Collections;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;
using System;
using Amazon;

public class Polling : MonoBehaviour {
	AmazonDynamoDBClient client;
	int numWaitFrames;
	int lastDatetime = 0;
	bool firstRun = true;
	JsonParser jsonParser;
	UnityStandardAssets.Characters.ThirdPerson.AICharacterControl aiKevin;

	// Use this for initialization
	void Start () {
		UnityInitializer.AttachToGameObject (this.gameObject);
		client = new AmazonDynamoDBClient("AKIAIBMC7ZDMT4JWAZMA", "zkyBzDyy1nqD3oSjZNtqHExf0kbi5EYZ46Nrtjub", Amazon.RegionEndpoint.USEast1);
		numWaitFrames = 0;
		jsonParser = GetComponent<JsonParser> ();
		aiKevin = GetComponent <UnityStandardAssets.Characters.ThirdPerson.AICharacterControl> ();
	}

	// Update is called once per frame
	void Update () {
		if (numWaitFrames == 30){
			numWaitFrames = 0;
			var request = new ScanRequest {
				TableName = "escapeCommands",
			};
			client.ScanAsync (request, (result) => {
				foreach (Dictionary<string, AttributeValue> item in result.Response.Items) {
					bool freshItem = false;
					foreach(KeyValuePair<string, AttributeValue> entry in item){
						if (entry.Key == "datetime") {
							int intDatetime = Int32.Parse(entry.Value.S.Substring(entry.Value.S.Length-5));
							if (intDatetime > lastDatetime) {
								// we have a fresh entry
								lastDatetime = intDatetime;
								freshItem = true;
								continue;
							}
						}
						if (firstRun == false && freshItem == true && entry.Key == "utterance") {
							string[] rets = jsonParser.processASR(entry.Value.S);
							aiKevin.OnAlexaCommand(rets);
							break;
						}
					}
					if (freshItem == true)
						break;
				}
				firstRun = true;
			});
		}else{
			numWaitFrames += 1;
	}
}

}
