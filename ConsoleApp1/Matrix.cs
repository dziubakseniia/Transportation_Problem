using System;
using System.Linq;

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
            CreateTempMatrix(matrix);

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
            CreateTempMatrix(matrix);

            double min = matrix.CMatrix[0][0];

            double[][] tArray = new double[matrix.Restrictions][];

            for (int i = 0; i < matrix.Restrictions; i++)
            {
                tArray[i] = new double[matrix.Variables];
            }

            CopyCMatrixToTArray(tArray, matrix);

            while (!IsOptimal(tArray))
            {
                int minRowIndex = 0;
                int minColIndex = 0;

                var minRowColIndexes = FindFirstNot0Value(tArray, min, minRowIndex, minColIndex);

                min = minRowColIndexes.Item1;
                minRowIndex = minRowColIndexes.Item2;
                minColIndex = minRowColIndexes.Item3;

                minRowColIndexes = FindMinNot0Value(tArray, min, minRowIndex, minColIndex);

                min = minRowColIndexes.Item1;
                minRowIndex = minRowColIndexes.Item2;
                minColIndex = minRowColIndexes.Item3;

                double minVal = Math.Min(tArray[matrix.CMatrix.Length - 1][minColIndex],
                    tArray[minRowIndex][matrix.CMatrix[minRowIndex].Length - 1]);

                matrix.TempMatrix[minRowIndex][minColIndex] = minVal;

                tArray[matrix.CMatrix.Length - 1][minColIndex] -= minVal;
                tArray[minRowIndex][matrix.CMatrix[minRowIndex].Length - 1] -= minVal;

                MakeCol0(tArray, matrix, minColIndex);
                MakeRow0(tArray, matrix, minRowIndex);
            }

            return matrix;
        }

        public void CreateTempMatrix(Matrix matrix)
        {
            TempMatrix = new double[matrix.Restrictions][];

            for (int i = 0; i < matrix.Restrictions; i++)
            {
                matrix.TempMatrix[i] = new double[matrix.Variables];
            }
        }

        public void CopyCMatrixToTArray(double[][] tArray, Matrix matrix)
        {
            for (int i = 0; i < matrix.Restrictions; i++)
            {
                for (int j = 0; j < matrix.Variables; j++)
                {
                    tArray[i][j] = matrix.CMatrix[i][j];
                }
            }
        }

        public Tuple<double, int, int> FindFirstNot0Value(double[][] tArray, double min, int minRowIndex, int minColIndex)
        {
            for (int i = 0; i < tArray.Length; i++)
            {
                for (int j = 0; j < tArray[i].Length; j++)
                {
                    if (tArray[i][j] != 0)
                    {
                        min = tArray[i][j];
                        minRowIndex = i;
                        minColIndex = j;
                        break;
                    }
                }
            }

            return Tuple.Create(min, minRowIndex, minColIndex);
        }

        public Tuple<double, int, int> FindMinNot0Value(double[][] tArray, double min, int minRowIndex, int minColIndex)
        {
            for (int i = 0; i < tArray.Length; i++)
            {
                for (int j = 0; j < tArray[i].Length; j++)
                {
                    if (tArray[i][j] < min && tArray[i][j] != 0)
                    {
                        min = tArray[i][j];
                        minRowIndex = i;
                        minColIndex = j;
                    }
                }
            }

            return Tuple.Create(min, minRowIndex, minColIndex);
        }

        public void MakeCol0(double[][] tArray, Matrix matrix, int minColIndex)
        {
            if (tArray[matrix.CMatrix.Length - 1][minColIndex] == 0)
            {
                int k = 0;
                while (k < matrix.Restrictions)
                {
                    tArray[k][minColIndex] = 0;
                    k++;
                }
            }
        }

        public void MakeRow0(double[][] tArray, Matrix matrix, int minRowIndex)
        {
            if (tArray[minRowIndex][matrix.CMatrix[minRowIndex].Length - 1] == 0)
            {
                int k = 0;
                while (k < matrix.Variables)
                {
                    tArray[minRowIndex][k] = 0;
                    k++;
                }
            }
        }

        public bool IsOptimal(double[][] tArray)
        {
            double[] expectedArray = new double[tArray[0].Length];
            if (tArray[tArray.Length - 1].SequenceEqual(expectedArray))
            {
                return true;
            }

            return false;
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
                    Console.Write($"{matrix[i][j],5}");
                }
                Console.WriteLine();
            }
        }
    }
}