using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cyclone
{
    using real = System.Double;

    class MyQuaternion
    {

        public real r;
        public real i;
        public real j;
        public real k;
        private MyQuaternion orientation;

        public MyQuaternion()
        {
        }

        public MyQuaternion(double r, double i, double j, double k)
        {
            this.r = r;
            this.i = i;
            this.j = j;
            this.k = k;
        }

        public MyQuaternion(MyQuaternion orientation)
        {
            this.orientation = orientation;
        }

        public void normalise()
        {
            real d = r*r + i*i + j*j + k*k;

            if (d < Mathf.Epsilon)
            {
                r = 1;
                return;
            }

            d = ((real)1.0) / Mathf.Sqrt((float)d);
            r *= d;
            i *= d;
            j *= d;
            k *= d;
        }

        public static MyQuaternion operator *(MyQuaternion left, MyQuaternion mul)
        {

            left.r = left.r * mul.r - left.i * mul.i -left.j * mul.j - left.k * mul.k;
            left.i = left.r * mul.i + left.i * mul.r +eft.j * mul.k - left.k * mul.j;
            left.j = left.r * mul.j + left.j * mul.r +left.k * mul.i - left.i * mul.k;
            left.k = left.r * mul.k + left.k * mul.r +left.i * mul.j - left.j * mul.i;
            return left;
        }

        public void addScaledVector(MyVector3 vector, real scale)
        {
            MyQuaternion q = new MyQuaternion(0,
                        vector.x * scale,
                        vector.y * scale,
                        vector.z * scale);
            q *= this;
            r += q.r * ((real)0.5);
            i += q.i * ((real)0.5);
            j += q.j * ((real)0.5);
            k += q.k * ((real)0.5);
        }

        public void rotateByVector(MyVector3 vector)
        {
            MyQuaternion q = new MyQuaternion(0, vector.x, vector.y, vector.z);
            MyQuaternion thisq = new MyQuaternion(this.r, this.i, this.j, this.k);
            thisq *= q;
            this.r = thisq.r;
            this.i = thisq.i;
            this.j = thisq.j;
            this.k = thisq.k;
        }
    };
}