﻿using UnityEngine;

public class VehiclePositionResetter : MonoBehaviour, Ros.IRosClient
{
    Ros.Bridge Bridge;

    public GpsDevice GpsDevice;
    public string ResetTopic = "/simulator/reset";

    public void OnRosBridgeAvailable(Ros.Bridge bridge)
    {
        Bridge = bridge;
        Bridge.AddPublisher(this);
    }

    public void OnRosConnected()
    {
        Bridge.Subscribe<Ros.Vector3>(ResetTopic, msg =>
        {
            var position = GpsDevice.GetPosition(msg.x, msg.y);

            int mask = 1 << LayerMask.NameToLayer("Ground And Road");
            RaycastHit hit;
            if (Physics.Raycast(position + new Vector3(0, 100, 0), new Vector3(0, -1, 0), out hit, Mathf.Infinity, mask))
            {
                position = hit.point;
                position.y += 0.01f;
            }
            else
            {
                position.y += 20.0f;
            }
            var angle = (float)msg.z * Mathf.Rad2Deg - GpsDevice.Angle;
            var rotation = Quaternion.AngleAxis(angle, Vector3.up);
            // reset position, rotation, velocity and angular velocity
            GpsDevice.Robot.GetComponent<VehicleInputController>().controller.ResetSavedPosition(position, rotation);

        });
    }
}
