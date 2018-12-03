using System;

namespace ConsoleApp1
{
    public class Matrix
    {
        public double[][] CMatrix { get; set; }
        public double[][] TempMatrix { get; set; }

        public int Variables { get; set; }
        public int Restrictions { get; set; }

        public Matrix NorthWest(Matrix matrix)
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

            return matrix;
        }

        public Matrix LeastCost(Matrix matrix)
        {
            TempMatrix = new double[matrix.Restrictions][];

            for (int i = 0; i < matrix.Restrictions; i++)
            {
                matrix.TempMatrix[i] = new double[matrix.Variables];
            }

            double min = matrix.CMatrix[0][0];
            double[][] tArray = new double[matrix.Restrictions][];

            tArray = CMatrix;

            for (int i = 0; i < matrix.Restrictions; i++)
            {
                for (int j = 0; j < matrix.Variables; j++)
                {
                    if (min >= tArray[i][j])
                    {
                        double minVal = Math.Min(tArray[matrix.CMatrix.Length - 1][j],
                            tArray[i][matrix.CMatrix[i].Length - 1]);

                        matrix.TempMatrix[i][j] = minVal;

                        tArray[matrix.CMatrix.Length - 1][j] -= minVal;
                        tArray[i][matrix.CMatrix[i].Length - 1] -= minVal;

                        if (tArray[matrix.CMatrix.Length - 1][j] == 0)
                        {
                            int k = 0;
                            while (k < matrix.Restrictions)
                            {
                                tArray[i][k] = 0;
                                k++;
                            }
                        }

                        if (tArray[i][matrix.CMatrix[i].Length - 1] == 0)
                        {
                            int k = 0;
                            while (k < matrix.Variables)
                            {
                                tArray[i][k] = 0;
                                k++;
                            }
                        }
                    }
                }
            }

            Console.WriteLine("matrix.CMatrix:");
            Print(matrix.CMatrix);
            Console.WriteLine();
            Console.WriteLine();

            return matrix;
        }

        public double Sum(double[][] matrix, double[][] tempMatrix)
        {
            double sum = 0;
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    matrix[i][j] *= tempMatrix[i][j];
                    sum += matrix[i][j];
                }
            }

            return sum;
        }

        public void Print(double[][] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    Console.Write(String.Format("{0,5}", matrix[i][j]));
                }
                Console.WriteLine();
            }
        }
    }
}