import json
import re
def lambda_handler(event, context):
    utterance = json.loads(event)["request"]["intent"]["slots"]["Utterance"]["value"].strip()

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
        return [action, target, ""]

    for direction in directs:
        if direction in utterance:
            action = "ToGoDirection"
            target = direction
            break

    if action:
        distances = set(["one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten"])
        for dist in distances:
            if dist in utterance:
                unit = dist
                return [action, target, unit]
        return [action, target, ""]

    if 'crouch' in utterance:
        action = "Crouch"
        return [action, "", ""]

    return ["", "", ""]
