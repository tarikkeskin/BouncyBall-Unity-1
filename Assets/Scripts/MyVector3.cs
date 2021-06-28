using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cyclone
{
    
    /**
    * Holds a vector in 3 dimensions. Four data members are allocated
    * to ensure alignment in an array.
    */
    public class MyVector2 
    {
        /** Holds the value along the x axis. */
        public double x;

        /** Holds the value along the y axis. */
        public double y;



        /** The default constructor creates a zero vector. */
        public MyVector2()
        {
            x = 0;
            y = 0;
        }

        /**
         * The explicit constructor creates a vector with the given
         * components.
         */
        public MyVector2(double x_1, double y_1, double z_1)
        {
            x = x_1;
            y = y_1;
        }

        //constructor with Vector
        public MyVector2(MyVector2 v)
        {

            x = v.x;
            y = v.y;
        }


         /** Flips all the components of the vector. */
        public void Invert()
        {
            x = -x;
            y = -y;
        }

         /** Gets the magnitude of this vector. */
        public double Magnitude()
        {
            return System.Math.Sqrt(x*x + y*y);
        }

        /** Gets the squared magnitude of this vector. */
        public double SquareMagnitude()
        {
            return x*x+y*y;
        }

        /** Return a normilese vector into a vector of unit length. */
        public void Normalise()
        {
            double temp = Magnitude();
            if (temp > 0)
            {
                temp = ((double)1) / temp;
                this.x *= temp;
                this.y *= temp;
            }
        }

        /** Return  a vector of unit length. */
        public MyVector2 returnNormalized()
        {
            MyVector2 normalized = new MyVector2(this);
            double temp = Magnitude();
            if (temp > 0)
            {
                temp = ((double)1) / temp;
                normalized.x *= temp;
                normalized.y *= temp;
            }
            return normalized;
        }

        /*
        *
        *
        *                **********  OPERATORS  *************************
        *
        *
        */ 

          /**
        * Returns the value of the given vector multiply to this.
        */
        public static MyVector2 operator *(MyVector2 vector_1, double value)
        {
            vector_1.x *= value;
            vector_1.y *= value;
            return vector_1;
        }
        /**
        * Returns the value of the given vector multiply to this.
        */
        public static MyVector2 operator *(double value, MyVector2 vector_1)
        {
            vector_1.x *= value;
            vector_1.y *= value;
            return vector_1;
        }
        /**
         * Calculates and returns the scalar product of this two vector.
         */
        public static double operator *(MyVector2 vector_1, MyVector2 vector) 
        {
            return vector_1.x* vector.x + vector_1.y * vector.y;
        }
    
  /**
        * Returns the value of the given vector added to this.
        */
        public static MyVector2 operator +( MyVector2 vector_1, MyVector2 vector_2)
        {
            vector_1.x += vector_2.x;
            vector_1.y += vector_2.y;
            return vector_1;
        }


        /**
        * Returns the value of the given vector subtracted to this.
        */
        public static MyVector2 operator -(MyVector2 vector_1, MyVector2 vector_2)
        {
            vector_1.x -= vector_2.x;
            vector_1.y -= vector_2.y;
            return vector_1;
        }

        /**
         * Calculates and returns the vector product
         */
        public static MyVector2 operator %(MyVector2 vector_1,  MyVector2 vector)
        {
            vector_1.x = vector_1.y * vector.z - vector_1.z * vector.y;
            vector_1.y = vector_1.z * vector.x - vector_1.x * vector.z;
            return vector_1;
        }

        /** Checks if the two vectors have identical or not. */
        public static bool operator ==(MyVector2 vector_1 ,MyVector2 other) 
        {
            return vector_1.x == other.x &&
                vector_1.y == other.y ;
        }
        /** Checks if the two vectors have non-identical components. */
        public static bool operator !=(MyVector2 vector_1, MyVector2 other)
        {
            return !(vector_1 == other);
        }

        /**
        * Calculates and return vector product of this vector with the given value
        */
        public static MyVector2 operator /(MyVector2 vector_1, double value)
        {
            if (value == 0)
            {
                Debug.LogError("Divison by zero Error");
            }
            value = 1 / value;
            vector_1.x *= value;
            vector_1.y *= value;

            return vector_1;
        }
         /**
         * Checks the vector is less than the other or not 
         */
        public static bool operator <(MyVector2 vector_1, MyVector2 other) 
        {
            return vector_1.x < other.x && vector_1.y <other.y;
        }

        /**
         * Checks the vector is more than the other or not .
         */
        public static bool operator >(MyVector2 vector_1, MyVector2 other)
        {
            return vector_1.x > other.x && vector_1.y > other.y;
        }

        /*
        *
        *
        *                **********  OPERATORS END *************************
        *
        *
        */ 

          /**
         * Adds the given vector to this, scaled by the given amount.
         */
        public void AddScaledVector(MyVector2 vector, double scale)
        {
            x += vector.x* scale;
            y += vector.y* scale;

        }

         /**
         * Calculates and returns the scalar product
         */
        public double ScalarProduct( MyVector2 vector) 
        {
            return x* vector.x + y* vector.y;
        }

        /**
         * Calculates and returns a component product with giving vector
         */
        public MyVector2 ComponentProduct(MyVector2 vector)
        {
            return new MyVector2(x* vector.x, y* vector.y);
        }

        /**
         * Performs a component product with the given vector 
         */
        public void ComponentProductUpdate( MyVector2 vector)
        {
            x *= vector.x;
            y *= vector.y;
        }
        
        /** Zero all the components of the vector. */
        public void Clear()
        {
            x = y = 0;
        }

       
    }
}