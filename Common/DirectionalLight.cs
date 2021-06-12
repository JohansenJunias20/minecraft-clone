using OpenTK.Graphics.ES30;
using OpenTK.Mathematics;
using System;

namespace LearnOpenTK.Common
{
    // This is the camera class as it could be set up after the tutorials on the website
    // It is important to note there are a few ways you could have set up this camera, for example
    // you could have also managed the player input inside the camera class, and a lot of the properties could have
    // been made into functions.

    // TL;DR: This is just one of many ways in which we could have set up the camera
    // Check out the web version if you don't know why we are doing a specific thing or want to know more about the code
    public class DirectionalLight
    {
        // Those vectors are directions pointing outwards from the camera to define how it rotated
        public Vector3 _front = -Vector3.UnitZ;

        private Vector3 _up = Vector3.UnitY;

        private Vector3 _right = Vector3.UnitX;

        // Rotation around the X axis (radians)
        private float _pitch;

        // Rotation around the Y axis (radians)
        //private float _yaw = 0f; // Without this you would be started rotated 90 degrees right
        private float _yaw = -MathHelper.PiOver2; // Without this you would be started rotated 90 degrees right

        // The field of view of the camera (radians)

        public DirectionalLight(Vector3 position)
        {
            Position = position;
        }

        // The position of the camera
        public Vector3 Position { get; set; }

        // This is simply the aspect ratio of the viewport, used for the projection matrix
        public float AspectRatio { private get; set; }

        public Vector3 Front => _front;
        public Vector4 orthoSize { get; set; }

        public Vector3 Up => _up;

        public Vector3 Right => _right;


        // We convert from degrees to radians as soon as the property is set to improve performance
        public float Pitch
        {
            get => MathHelper.RadiansToDegrees(_pitch);
            set
            {
                // We clamp the pitch value between -89 and 89 to prevent the camera from going upside down, and a bunch
                // of weird "bugs" when you are using euler angles for rotation.
                // If you want to read more about this you can try researching a topic called gimbal lock
                //var angle = MathHelper.Clamp(value, -89f, 89f);
                _pitch = MathHelper.DegreesToRadians(value);
                UpdateVectors();
            }
        }
        public float nearVal;
        public float farVal;
        // We convert from degrees to radians as soon as the property is set to improve performance
        public float Yaw
        {
            get => MathHelper.RadiansToDegrees(_yaw);
            set
            {
                _yaw = MathHelper.DegreesToRadians(value);
                UpdateVectors();
            }
        }

        public Vector3 GetDirection()
        {
            return _front;
        }


        // Get the view matrix using the amazing LookAt function described more in depth on the web tutorials
        public Matrix4 getSpaceMatrix()
        {
            return Matrix4.LookAt(Position, Position + _front, _up) * Matrix4.CreateOrthographicOffCenter(orthoSize.X, orthoSize.Y, orthoSize.Z, orthoSize.W, nearVal, farVal);
        }

        // Get the projection matrix using the same method we have used up until this point

        // This function is going to update the direction vertices using some of the math learned in the web tutorials
        private void UpdateVectors()
        {
            // First the front matrix is calculated using some basic trigonometry
            _front.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
            _front.Y = MathF.Sin(_pitch);
            _front.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);
            // We need to make sure the vectors are all normalized, as otherwise we would get some funky results
            _front = Vector3.Normalize(_front);
            // Calculate both the right and the up vector using cross product
            // Note that we are calculating the right from the global up, this behaviour might
            // not be what you need for all cameras so keep this in mind if you do not want a FPS camera
            _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
            _up = Vector3.Normalize(Vector3.Cross(_right, _front));
            //Console.WriteLine($"front : {_front.X}, {_front.Y}, {_front.Z} up : {_up.X},{_up.Y},{_up.Z} position : {Position.X},{Position.Y},{Position.Z}");
        }
    }
}