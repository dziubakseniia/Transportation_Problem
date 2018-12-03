using System;

namespace T_task.Models
{
    public class Matrix
    {
        public double[][] CMatrix { get; set; }
        public double[][] TempMatrix { get; set; }

        public int Variables { get; set; }
        public int Restrictions { get; set; }

        public double NorthWest(Matrix matrix)
        {
            TempMatrix = new double[matrix.Restrictions][];

            for (int i = 0; i < matrix.Restrictions; i++)
            {
                matrix.TempMatrix[i] = new double[matrix.Variables];
            }

            for (int i = 0; i < matrix.Restrictions; i++)
            {
                for (int j = 0; j < matrix.Variables; j++)
                {
                    double min = Math.Min(matrix.CMatrix[matrix.CMatrix.Length - 1][j],
                        matrix.CMatrix[i][matrix.CMatrix[i].Length - 1]);

                    matrix.TempMatrix[i][j] = min;

                    matrix.CMatrix[matrix.CMatrix.Length - 1][j] -= min;
                    matrix.CMatrix[i][matrix.CMatrix[i].Length - 1] -= min;
                }
            }

            return Sum(matrix.CMatrix, matrix.TempMatrix);
        }

        public double Sum(double[][] matrix, double[][] tempMatrix)
        {
            double sum = 0;
            for (int i = 0; i < Restrictions; i++)
            {
                for (int j = 0; j < Variables; j++)
                {
                    matrix[i][j] *= tempMatrix[i][j];
                    sum += matrix[i][j];
                }
            }

            return sum;
        }
    }
}