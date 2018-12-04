using System;
using System.Collections.Generic;
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

        public Matrix UVMethod(Matrix matrix)
        {
            double[][] allocatedMatrix = CreateAllocatedMatrix(matrix);

            if (CheckAllocatedElements(allocatedMatrix))
            {
                double?[] uValues = new double?[matrix.Restrictions - 1];
                double?[] vValues = new double?[matrix.Variables - 1];

                CreateUVArrays(uValues, vValues, matrix);

                Console.WriteLine("V_Values:");
                PrintOneDArray(vValues);

                Console.WriteLine("U_Values");
                PrintOneDArray(uValues);

                double[][] unallocatedMatrix = CreateUnallocatedMatrix(matrix, allocatedMatrix, uValues, vValues);

                List<Tuple<int, int>> coordinates = new List<Tuple<int, int>>();

                for (int i = 0; i < unallocatedMatrix.Length; i++)
                {
                    for (int j = 0; j < unallocatedMatrix[i].Length; j++)
                    {
                        if (unallocatedMatrix[i][j] > 0)
                        {
                            if ((i - 1) >= 0 && (i - 1) < unallocatedMatrix.Length && unallocatedMatrix[i - 1][j] == 0)
                            {
                                coordinates.Add(Tuple.Create(i - 1, j));
                            }
                            else if ((i + 1) >= 0 && (i + 1) < unallocatedMatrix.Length && unallocatedMatrix[i + 1][j] == 0)
                            {
                                coordinates.Add(Tuple.Create(i + 1, j));
                            }
                        }
                    }
                }

                foreach (var coordinate in coordinates)
                {
                    Console.WriteLine(coordinate.Item1 + " " + coordinate.Item2);
                }

                matrix.CMatrix = unallocatedMatrix;
            }

            return matrix;
        }

        public double[][] CreateAllocatedMatrix(Matrix matrix)
        {
            double[][] allocatedMatrix = new double[matrix.Restrictions][];

            for (int i = 0; i < matrix.Restrictions; i++)
            {
                allocatedMatrix[i] = new double[matrix.Variables];
            }

            for (int i = 0; i < matrix.Restrictions; i++)
            {
                for (int j = 0; j < matrix.Variables; j++)
                {
                    allocatedMatrix[i][j] = matrix.TempMatrix[i][j];
                }
            }

            return allocatedMatrix;
        }

        public double[][] CreateUnallocatedMatrix(Matrix matrix, double[][] allocatedMatrix, double?[] uValues, double?[] vValues)
        {
            double[][] unallocatedMatrix = new double[matrix.Restrictions - 1][];

            for (int i = 0; i < matrix.Restrictions - 1; i++)
            {
                unallocatedMatrix[i] = new double[matrix.Variables - 1];
            }

            for (int i = 0; i < matrix.Restrictions - 1; i++)
            {
                for (int j = 0; j < matrix.Variables - 1; j++)
                {
                    if (allocatedMatrix[i][j] == 0)
                    {
                        unallocatedMatrix[i][j] = (double)uValues[i] + (double)vValues[j] - matrix.CMatrix[i][j];
                    }
                }
            }

            return unallocatedMatrix;
        }

        public void CreateUVArrays(double?[] uValues, double?[] vValues, Matrix matrix)
        {
            for (int i = 0; i < uValues.Length; i++)
            {
                for (int j = 0; j < vValues.Length; j++)
                {
                    if (matrix.TempMatrix[i][j] != 0)
                    {
                        uValues[0] = 0;
                        break;
                    }
                }
            }

            for (int i = 0; i < uValues.Length; i++)
            {
                for (int j = 0; j < vValues.Length; j++)
                {
                    if (matrix.TempMatrix[i][j] != 0)
                    {
                        if (uValues[i] != null && vValues[j] == null)
                        {
                            vValues[j] = matrix.CMatrix[i][j] - uValues[i];
                        }
                        else if (uValues[i] == null && vValues[j] != null)
                        {
                            uValues[i] = matrix.CMatrix[i][j] - vValues[j];
                        }
                    }
                }
            }
        }

        public bool CheckAllocatedElements(double[][] allocatedMatrix)
        {
            int numbers = 0;
            for (int i = 0; i < allocatedMatrix.Length; i++)
            {
                for (int j = 0; j < allocatedMatrix[i].Length; j++)
                {
                    if (allocatedMatrix[i][j] != 0)
                    {
                        numbers++;
                    }
                }
            }

            if (numbers == ((allocatedMatrix.Length - 1) + (allocatedMatrix[0].Length - 1) - 1))
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

        public void PrintOneDArray(double?[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write($"{array[i],5}");
            }

            Console.WriteLine();
            Console.WriteLine();
        }
    }
}