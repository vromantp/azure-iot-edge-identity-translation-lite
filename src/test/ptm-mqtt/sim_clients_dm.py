import json
import random
import paho.mqtt.client as mqtt

def on_connect(client, userdata, flags, rc):
    print("Connected with result code " + str(rc))

def on_directmethod_requestmsg(client, userdata, msg):
    print(msg.topic + " " + str(msg.payload))

    payloadJson = json.loads(msg.payload)
    requestId = payloadJson["RequestId"]

    response = {
        "RequestId": requestId,
        "Data": {
            "value1": random.randrange(0,100),
            "value2": random.random()
        }
    }
    
    topic = msg.topic.replace("request", "response")
    client.publish(topic, json.dumps(response))


client = mqtt.Client()
client.on_connect = on_connect
client.on_message = on_directmethod_requestmsg

client.connect("127.0.0.1", 1883, 60)
client.subscribe("device/+/directmethod/+/request", 0)

# Blocking call that processes network traffic, dispatches callbacks and
# handles reconnecting.
# Other loop*() functions are available that give a threaded interface and a
# manual interface.
client.loop_forever()