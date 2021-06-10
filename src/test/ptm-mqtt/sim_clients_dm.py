import json
import paho.mqtt.client as mqtt

# The callback for when the client receives a CONNACK response from the server.
def on_connect(client, userdata, flags, rc):
    print("Connected with result code "+str(rc))

# The callback for when a PUBLISH message is received from the server.
def on_message(client, userdata, msg):
    print(msg.topic+" "+str(msg.payload))

    topicParts = msg.topic.split("/")
    deviceId = topicParts[1]
    methodName = topicParts[3]

    payloadJson = json.loads(msg.payload)
    requestId = payloadJson["RequestId"]

    response = {
        "RequestId": requestId,
        "Data": {
            "value1": 123,
            "value2": "FooBar"
        }
    }
    
    topic = msg.topic.replace("request", "response")
    client.publish(topic, json.dumps(response))


client = mqtt.Client()
client.on_connect = on_connect
client.on_message = on_message

client.connect("127.0.0.1", 1883, 60)

client.subscribe("device/+/directmethod/+/request", 0)

# Blocking call that processes network traffic, dispatches callbacks and
# handles reconnecting.
# Other loop*() functions are available that give a threaded interface and a
# manual interface.
client.loop_forever()