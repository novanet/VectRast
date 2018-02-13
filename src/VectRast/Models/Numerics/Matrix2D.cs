using System;

namespace VectRast.Models.Numerics
{
    public class Matrix2D
    {
        public double[,] elements;
        public Matrix2D()
        {
            elements = new double[3, 3];
        }
        public static Matrix2D identityM()
        {
            Matrix2D matrix = new Matrix2D();
            for (int i = 0; i < 3; i++)
                matrix.elements[i, i] = 1;
            return matrix;
        }
        public static Matrix2D translationM(double x, double y)
        {
            Matrix2D matrix = Matrix2D.identityM();
            matrix.elements[0, 2] = x;
            matrix.elements[1, 2] = y;
            return matrix;
        }
        public static Matrix2D scaleM(double x, double y)
        {
            Matrix2D matrix = new Matrix2D();
            matrix.elements[0, 0] = x;
            matrix.elements[1, 1] = y;
            matrix.elements[2, 2] = 1;
            return matrix;
        }
        public static Matrix2D rotationM(double x)
        {
            Matrix2D matrix = Matrix2D.identityM();
            matrix.elements[0, 0] = Math.Cos(x * Math.PI / 180);
            matrix.elements[1, 1] = matrix.elements[0, 0];
            matrix.elements[1, 0] = Math.Sin(x * Math.PI / 180);
            matrix.elements[0, 1] = -matrix.elements[1, 0];
            return matrix;
        }
        public static Matrix2D operator *(Matrix2D m1, Matrix2D m2)
        {
            Matrix2D matrix = new Matrix2D();
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < 3; k++)
                        sum += m1.elements[k, j] * m2.elements[i, k];
                    matrix.elements[i, j] = sum;
                }
            return matrix;
        }
        public static DoubleVector2 operator *(DoubleVector2 v, Matrix2D m)
        {
            DoubleVector2 vector = new DoubleVector2();
            vector.x = v.x * m.elements[0, 0] + v.y * m.elements[0, 1] + m.elements[0, 2];
            vector.y = v.x * m.elements[1, 0] + v.y * m.elements[1, 1] + m.elements[1, 2];
            return vector;
        }
    }
}