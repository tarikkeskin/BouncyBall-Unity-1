using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cyclone
{
    using real = System.Double;

    class RigidBody
    {
        public static readonly real sleepEpsilon = (real)0.3;

        static void _calculateTransformMatrix(Matrix4 transform,MyVector3 pos,MyQuaternion ori)
        {
            transform.data[0] = 1 - 2 * ori.j * ori.j -
                2 * ori.k * ori.k;
            transform.data[1] = 2 * ori.i * ori.j -
                2 * ori.r * ori.k;
            transform.data[2] = 2 * ori.i * ori.k +
                2 * ori.r * ori.j;
            transform.data[3] = pos.x;

            transform.data[4] = 2 * ori.i * ori.j +
                2 * ori.r * ori.k;
            transform.data[5] = 1 - 2 * ori.i * ori.i -
                2 * ori.k * ori.k;
            transform.data[6] = 2 * ori.j * ori.k -
                2 * ori.r * ori.i;
            transform.data[7] = pos.y;

            transform.data[8] = 2 * ori.i * ori.k -
                2 * ori.r * ori.j;
            transform.data[9] = 2 * ori.j * ori.k +
                2 * ori.r * ori.i;
            transform.data[10] = 1 - 2 * ori.i * ori.i -
                2 * ori.j * ori.j;
            transform.data[11] = pos.z;
        }

        protected real inverseMass;
        protected Matrix3 inverseInertiaTensor;
        protected real linearDamping;

        /**
         * Holds the amount of damping applied to angular
         * motion.  Damping is required to remove energy added
         * through numerical instability in the integrator.
         */
        protected real angularDamping;

        /**
         * Holds the linear position of the rigid body in
         * world space.
         */
        protected MyVector3 position;

        /**
         * Holds the angular orientation of the rigid body in
         * world space.
         */
        protected MyQuaternion orientation;
        protected MyVector3 velocity;
        protected MyVector3 rotation;
        protected Matrix3 inverseInertiaTensorWorld;
        protected real motion;
        protected bool isAwake;
        protected bool canSleep;
        protected Matrix4 transformMatrix;
        protected MyVector3 forceAccum;
        protected MyVector3 torqueAccum;
        protected MyVector3 acceleration;
        protected MyVector3 lastFrameAcceleration;

        public RigidBody()
        {
            this.inverseMass = new real();
            this.inverseInertiaTensor = new Matrix3();
            this.linearDamping = new real();
            this.angularDamping = new real();
            this.position = new MyVector3();
            this.orientation = new MyQuaternion();
            this.velocity = new MyVector3();
            this.rotation = new MyVector3();
            this.inverseInertiaTensorWorld = new Matrix3();
            this.motion = new real();
            this.isAwake = new bool();
            this.canSleep = new bool();
            this.transformMatrix = new Matrix4();
            this.forceAccum = new MyVector3();
            this.torqueAccum = new MyVector3();
            this.acceleration = new MyVector3();
            this.lastFrameAcceleration = new MyVector3();
        }

        public void calculateDerivedData()
        {
            orientation.normalise();

            _calculateTransformMatrix(transformMatrix, position, orientation);
        }

        /**
         * Integrates the rigid body forward in time by the given amount.
         * This function uses a Newton-Euler integration method
         */
        public void integrate(real duration)
        {
            if (!isAwake) return;

            lastFrameAcceleration = acceleration;
            lastFrameAcceleration.AddScaledVector(forceAccum, inverseMass);
            MyVector3 angularAcceleration =inverseInertiaTensorWorld.transform(torqueAccum);
            velocity.AddScaledVector(lastFrameAcceleration, duration);
            rotation.AddScaledVector(angularAcceleration, duration);
            velocity *= Mathf.Pow((float)linearDamping, (float)duration);
            rotation *= Mathf.Pow((float)angularDamping, (float)duration);
            position.AddScaledVector(velocity, duration);
            orientation.addScaledVector(rotation, duration);
            calculateDerivedData();
            clearAccumulators();

            if (canSleep)
            {
                real currentMotion = velocity.ScalarProduct(velocity) +
                    rotation.ScalarProduct(rotation);

                real bias = Mathf.Pow(0.5f, (float)duration);
                motion = bias * motion + (1 - bias) * currentMotion;

                if (motion < sleepEpsilon) setAwake(false);
                else if (motion > 10 * sleepEpsilon) motion = 10 * sleepEpsilon;
            }
        }

        public void setMass(real mass)
        {
            Debug.Assert(mass != 0);
            this.inverseMass = ((real)1.0) / mass;
        }

        public real getMass()
        {
            if (this.inverseMass == 0)
            {
                return real.MaxValue;
            }
            else
            {
                return ((real)1.0) / inverseMass;
            }
        }

        public void setInverseMass(real inverseMass)
        {
            this.inverseMass = inverseMass;
        }

        public real getInverseMass()
        {
            return inverseMass;
        }

        public bool hasFiniteMass()
        {
            return inverseMass >= 0.0f;
        }

        public void setInertiaTensor(Matrix3 inertiaTensor)
        {
            inverseInertiaTensor.setInverse(inertiaTensor);
        }

        public void getInertiaTensor(Matrix3 inertiaTensor)
        {
            inertiaTensor.setInverse(inverseInertiaTensor);
        }

        public Matrix3 getInertiaTensor()
        {
            Matrix3 it = new Matrix3();
            getInertiaTensor(it);
            return new Matrix3(it);
        }

        public void getInertiaTensorWorld(Matrix3 inertiaTensor)
        {
            inertiaTensor.setInverse(inverseInertiaTensorWorld);
        }

        public Matrix3 getInertiaTensorWorld()
        {
            Matrix3 it = new Matrix3();
            getInertiaTensorWorld(it);
            return new Matrix3(it);
        }

        /**
         * Sets the inverse intertia tensor for the rigid body.
         **/
        public void setInverseInertiaTensor(Matrix3 inverseInertiaTensor)
        {

            this.inverseInertiaTensor = inverseInertiaTensor;
        }

        /**
         * Copies the current inverse inertia tensor of the rigid body
         * into the given matrix.
         */
        public void getInverseInertiaTensor(Matrix3 inverseInertiaTensor)
        {
            inverseInertiaTensor = this.inverseInertiaTensor;
        }

        public Matrix3 getInverseInertiaTensor()
        {
            return new Matrix3(inverseInertiaTensor);
        }

        public void getInverseInertiaTensorWorld(Matrix3 inverseInertiaTensor)
        {
            inverseInertiaTensor = inverseInertiaTensorWorld;
        }

        public Matrix3 getInverseInertiaTensorWorld()
        {
            return new Matrix3(inverseInertiaTensorWorld);
        }

        public void setDamping(real linearDamping, real angularDamping)
        {
            this.linearDamping = linearDamping;
            this.angularDamping = angularDamping;
        }

        /**
         * Sets the linear damping for the rigid body.
        linearDamping The speed that velocity is shed from
         * the rigid body.
         */
        public void setLinearDamping(real linearDamping)
        {
            this.linearDamping = linearDamping;
        }

        public real getLinearDamping()
        {
            return linearDamping;
        }

        /**
         * Sets the angular damping for the rigid body.
         * angularDamping The speed that rotation is shed from
         * the rigid body.
         */
        public void setAngularDamping(real angularDamping)
        {
            this.angularDamping = angularDamping;
        }

        public real getAngularDamping()
        {
            return angularDamping;
        }

 
        public void setPosition(MyVector3 position)
        {
            this.position = position;
        }

        public void setPosition(real x, real y, real z)
        {
            position.x = x;
            position.y = y;
            position.z = z;
        }

        public void getPosition(MyVector3 position)
        {
            position = this.position;
        }

        public MyVector3 getPosition()
        {
            return new MyVector3(position);
        }

        public void setOrientation(MyQuaternion orientation)
        {
            this.orientation = orientation;
            this.orientation.normalise();
        }

        public void setOrientation(real r, real i, real j, real k)
        {
            orientation.r = r;
            orientation.i = i;
            orientation.j = j;
            orientation.k = k;
            orientation.normalise();
        }

        public void getOrientation(MyQuaternion orientation)
        {
            orientation = this.orientation;
        }

        public MyQuaternion getOrientation()
        {
            return new MyQuaternion( orientation);
        }

        public void getOrientation(Matrix3 matrix)
        {
            getOrientation(matrix.data);
        }

        public void getOrientation(real[] matrix)
        {
            matrix[0] = transformMatrix.data[0];
            matrix[1] = transformMatrix.data[1];
            matrix[2] = transformMatrix.data[2];

            matrix[3] = transformMatrix.data[4];
            matrix[4] = transformMatrix.data[5];
            matrix[5] = transformMatrix.data[6];

            matrix[6] = transformMatrix.data[8];
            matrix[7] = transformMatrix.data[9];
            matrix[8] = transformMatrix.data[10];
        }

        public void getTransform(Matrix4 transform)
        {
            transform = new Matrix4(transformMatrix.data);
        }

        public void getTransform(real[] matrix)
        {
            matrix = transformMatrix.data;
            matrix[12] = matrix[13] = matrix[14] = 0;
            matrix[15] = 1;
        }

        public Matrix4 getTransform()
        {
            return new Matrix4(transformMatrix);
        }

        public MyVector3 getPointInLocalSpace(MyVector3 point)
        {
            return new MyVector3( transformMatrix.transformInverse(point));
        }

        public MyVector3 getPointInWorldSpace(MyVector3 point)
        {
            return new MyVector3( transformMatrix.transform(point));
        }

        public MyVector3 getDirectionInLocalSpace(MyVector3 direction)
        {
            return new MyVector3(transformMatrix.transformInverseDirection(direction));
        }

        public MyVector3 getDirectionInWorldSpace(MyVector3 direction)
        {
            return new MyVector3( transformMatrix.transformDirection(direction));
        }

        public void setVelocity(MyVector3 velocity)
        {
            this.velocity = velocity;
        }

        public void setVelocity(real x, real y, real z)
        {
            velocity.x = x;
            velocity.y = y;
            velocity.z = z;
        }

        public void getVelocity(MyVector3 velocity)
        {
            velocity = this.velocity;
        }

        public MyVector3 getVelocity()
        {
            return new MyVector3( velocity);
        }

        public void addVelocity(MyVector3 deltaVelocity)
        {
            this.velocity += deltaVelocity;
        }


        public void setRotation(MyVector3 rotation)
        {
            this.rotation = rotation;
        }

        public void setRotation(real x, real y, real z)
        {
            rotation.x = x;
            rotation.y = y;
            rotation.z = z;
        }

        public void getRotation(MyVector3 rotation)
        {
            rotation = this.rotation;
        }

        public MyVector3 getRotation()
        {
            return new MyVector3(rotation);
        }

        public void addRotation(MyVector3 deltaRotation)
        {
            rotation += deltaRotation;
        }

        public bool getAwake()
        {
            return isAwake;
        }
        public void setAwake(bool awake = true)
        {
            if (awake){
                isAwake = true;
                motion = sleepEpsilon * 2.0f;
                }
            else{
                isAwake = false;
                velocity.Clear();
                rotation.Clear();
            }
        }


        public bool getCanSleep()
        {
            return canSleep;
        }

        public void setCanSleep(bool canSleep = true)
        {

            this.canSleep = canSleep;

            if (!canSleep && !isAwake) setAwake();
        }

        public void getLastFrameAcceleration(MyVector3 linearAcceleration)
        {
            linearAcceleration = this.lastFrameAcceleration;
        }

        public MyVector3 getLastFrameAcceleration()
        {
            return new MyVector3(lastFrameAcceleration);
        }

        public void clearAccumulators()
        {

            forceAccum.Clear();
            torqueAccum.Clear();
        }

        public void addForce(MyVector3 force)
        {
            forceAccum += force;
            isAwake = true;
        }

        public void addForceAtPoint(MyVector3 force, MyVector3 point)
        {

            MyVector3 pt = point;
            pt -= position;

            forceAccum += force;
            torqueAccum += pt % force;

            isAwake = true;
        }

        public void addForceAtBodyPoint(MyVector3 force, MyVector3 point)
        {


            // Convert to coordinates relative to center of mass.
            MyVector3 pt = getPointInWorldSpace(point);
            addForceAtPoint(force, pt);
        }


        public void addTorque(MyVector3 torque)
        {
            torqueAccum += torque;
            isAwake = true;
        }


        public void setAcceleration(MyVector3 acceleration)
        {
            this.acceleration = acceleration;
        }

        public void setAcceleration(real x, real y, real z)
        {
            acceleration.x = x;
            acceleration.y = y;
            acceleration.z = z;
        }

   
        public void getAcceleration(MyVector3 acceleration)
        {
            acceleration = this.acceleration;
        }

        public MyVector3 getAcceleration()
        {
            return new MyVector3(acceleration);
        }


    };
}