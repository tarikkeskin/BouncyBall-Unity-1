using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cyclone
{
    
    public class Particle 
    {
        //constructor
        public Particle()
        {
            position = new MyVector3();
            velocity = new MyVector3();
            forceAccum = new MyVector3();
            acceleration = new MyVector3();
        }

        /**
         * Holds the inverse of the mass of the particle. It
         * is more useful to hold the inverse mass because
         * integration is simpler, and because in real time
         * simulation it is more useful to have objects with
         * infinite mass (immovable) than zero mass
         * (completely unstable in numerical simulation).
         */
        public double inverseMass { get; set; }

        /**
         * Holds the amount of damping applied to linear
         * motion. Damping is required to remove energy added
         * through numerical instability in the integrator.
         */
        public double damping { get; set; }

        /**
         * Holds the linear position of the particle in
         * world space.
         */
        public MyVector3 position { get; set; }

        /**
         * Holds the linear velocity of the particle in
         * world space.
         */
        public MyVector3 velocity { get; set; }

        /**
         * Holds the accumulated force to be applied at the next
         * simulation iteration only. This value is zeroed at each
         * integration step.
         */
        public MyVector3 forceAccum { get; set; }

        /**
         * Holds the acceleration of the particle.  This value
         * can be used to set acceleration due to gravity (its primary
         * use), or any other constant acceleration.
         */
        public MyVector3 acceleration { get; set; }

        /**
         * Sets both the damping of the particle.
         */
        public void SetDamping(double damping)
        {
            this.damping = damping;
        }

        /**
         * Gets the current damping value.
         */
        public double GetDamping()
        {
            double d = new double();
            d = damping;
            return d;
        }

        /**
         * Sets the mass of the particle;
         */
        public void SetMass(double mass)
        {
            Debug.Assert(mass != 0);
            inverseMass = ((double)1.0)/mass;
        }

        /**
         * Gets the mass of the particle.
         */
        public double GetMass()
        {
            if (inverseMass == 0) 
            {
                return double.MaxValue;
            } 
            else 
            {
                return ((double)1.0)/inverseMass;
            }
        }

        /**
         * Gets the inverse mass of the particle.
         *
         * @return The current inverse mass of the particle.
         */
        public double GetInverseMass()
        {
            return inverseMass;
        }

        /**
         * Sets the inverse mass of the particle.
         */
        public void SetInverseMass(double inverseMass)
        {
            this.inverseMass = inverseMass;
        }


        /**
         * Integrates the particle forward in time by the given amount.
         * This function uses a Newton-Euler integration method, which is a
         * linear approximation to the correct integral. For this reason it
         * may be inaccurate in some cases.
         */
        public void Integrate(double duration)
        {
            // We don't integrate things with zero mass.
            if (inverseMass <= 0.0f) return;

            Debug.Assert(duration > 0.0);

            // Update linear position.
            position.AddScaledVector(this.velocity, duration);

            // Work out the acceleration from the force
            MyVector3 resultingAcc = acceleration;
            resultingAcc.AddScaledVector(forceAccum, inverseMass);

            // Update linear velocity from the acceleration.
            velocity.AddScaledVector(resultingAcc, duration);

            // Impose drag.
            velocity *= System.Math.Pow(damping, duration);

            // Clear the forces.
            ClearAccumulator();
        }
 

        /**
         * Sets the position of the particle.
         *
         * @param position The new position of the particle.
         */
        public void SetPosition(MyVector3 position)
        {
            this.position = position;
        }

        /**
         * Sets the position of the particle by component.
         */
        public void SetPosition(double x,double y,double z)
        {
            position.x = x;
            position.y = y;
            position.z = z;
        }

        /**
         * Fills the given vector with the position of the particle.
         *
         * @param position A pointer to a vector into which to write
         * the position.
         */
        public void GetPosition(MyVector3 position)
        {
            position = this.position;
        }

        /**
         * Gets the position of the particle.
         *
         * @return The position of the particle.
         */
         
        public MyVector3 GetPosition()
        {
            return position;
        }

        /**
         * Fills the given vector with the velocity of the particle.
         */
        public void GetVelocity(MyVector3 velocity)
        {
            velocity = this.velocity;
        }

        /**
         * Gets the velocity of the particle.
         */
        public MyVector3 GetVelocity()
        {
            return velocity;
        }
        /**
         * Sets the velocity of the particle.
         */
        public void SetVelocity(MyVector3 velocity)
        {
            this.velocity = velocity;
        }

        /**
         * Sets the velocity of the particle by component.
         */
        public void SetVelocity(double x, double y, double z)
        {
            velocity.x = x;
            velocity.y = y;
            velocity.z = z;
        }


        /**
         * Fills the given vector with the acceleration of the particle.
         */
        public void GetAcceleration(MyVector3 acceleration)
        {
            acceleration = this.acceleration;
        }

        /**
         * Gets the acceleration of the particle.
         */  
        public MyVector3 GetAcceleration()
        {
            return acceleration;
        }
        

        /**
        * Sets the constant acceleration of the particle.
        */
        public void SetAcceleration(MyVector3 acceleration)
        {
            this.acceleration = acceleration;
        }

        /**
         * Sets the constant acceleration of the particle by component.
         */
        public void SetAcceleration(double x,double y, double z)
        {
            acceleration.x = x;
            acceleration.y = y;
            acceleration.z = z;
        }

        /**
         * Clears the forces applied to the particle. This will be called automatically after each integration step.
         */
        public void ClearAccumulator()
        {
            forceAccum.Clear();
        }

    };
}

