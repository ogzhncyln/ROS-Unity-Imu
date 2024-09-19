using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Sensor;
using UnitySensors.ROS.Publisher;

public class ImuPublisher : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "/imu/data";
    public float publishFrequency = 50.0f; // Hz
    private ImuSensor imuSensor;
    private float timeSinceLastPublish = 0.0f;

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<ImuMsg>(topicName);

        imuSensor = GetComponent<ImuSensor>();
    }

    void FixedUpdate()
    {
        timeSinceLastPublish += Time.fixedDeltaTime;
        if (timeSinceLastPublish >= 1.0f / publishFrequency)
        {
            PublishImuData();
            timeSinceLastPublish = 0.0f;
        }
    }

    void PublishImuData()
    {
        ImuMsg imuMsg = new ImuMsg();

        imuMsg.header.stamp = new RosMessageTypes.BuiltinInterfaces.TimeMsg((uint)Time.time, (uint)((Time.time - (int)Time.time) * 1e9));
        imuMsg.header.frame_id = "imu_link";

        imuMsg.linear_acceleration.x = imuSensor.linearAcceleration.x;
        imuMsg.linear_acceleration.y = imuSensor.linearAcceleration.z;
        imuMsg.linear_acceleration.z = imuSensor.linearAcceleration.y;

        imuMsg.angular_velocity.x = imuSensor.angularVelocity.x;
        imuMsg.angular_velocity.y = imuSensor.angularVelocity.z;
        imuMsg.angular_velocity.z = imuSensor.angularVelocity.y;

        imuMsg.orientation.x = -imuSensor.orientation.x;
        imuMsg.orientation.y = -imuSensor.orientation.z;
        imuMsg.orientation.z = -imuSensor.orientation.y;
        imuMsg.orientation.w = imuSensor.orientation.w;

        ros.Publish(topicName, imuMsg);
    }
}
