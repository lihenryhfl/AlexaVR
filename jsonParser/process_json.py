import json
import re

def lambda_handler(event, context):
    def crouch_action(utterance):
        if 'crouch' in utterance:
            return True
        if 'hide' in utterance:
            if 'under' in utterance or 'below' in utterance:
                return True
        return False


    #utterance = json.loads(event)["request"]["intent"]["slots"]["Utterance"]["value"].strip()
    utterance = json.loads(event)["request"]["intent"]["slots"]["Action"]["value"].strip()
    key_values = {}

    objects = set(["couch", "table", "light", "bed", "stool", "chair", "fan", "wall"])
    directs = set(["left", "right", "forward", "backward", "around"])

    rets = []

    action = None
    direction = None
    target = None
    unit = None

    for obj in objects:
        if obj in utterance:
            action = "GoToObject"
            target = obj
            break

    if action:
        key_values["target"] = target
        if crouch_action(utterance):
            action = "GoAndCrouch"
            key_values["action"] = action
            key_values["unit"] = ""
            return json.dumps(key_values)
            #return [action, target, ""]

        key_values["action"] = action
        key_values["unit"] = ""
        return json.dumps(key_values)
        #return [action, target, ""]

    for direction in directs:
        if direction in utterance:
            action = "ToGoDirection"
            target = direction
            break

    if action:
        distances = set(["one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten"])
        key_values["action"] = action
        key_values["target"] = target
        for dist in distances:
            if dist in utterance:
                unit = dist
                key_values["unit"] = unit
                return json.dumps(key_values)
                #return [action, target, unit]

        key_values["unit"] = ""
        return json.dumps(key_values)
        #return [action, target, ""]

    if crouch_action(utterance):
        action = "Crouch"
        key_values["action"] = action
        key_values["target"] = ""
        key_values["unit"] = ""
        #return [action, "", ""]
    key_values["action"] = action
    key_values["target"] = ""
    key_values["unit"] = ""
    return json.dumps(key_values)
    #return ["", "", ""]

f = open('./test.json')
lines = [line.strip() for line in f.readlines()]
print lambda_handler(' '.join(lines), "")
