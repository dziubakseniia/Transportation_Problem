using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

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
        /*
        public Matrix UVMethod(Matrix matrix)
        {
            double[][] allocatedMatrix = CreateAllocatedMatrix(matrix);

            if (CheckAllocatedElements(allocatedMatrix))
            {
                double?[] uValues = new double?[matrix.Restrictions - 1];
                double?[] vValues = new double?[matrix.Variables - 1];

                bool finished = false;
                double positiveElement = 0;
                //while (!finished)
                {
                    CreateUVArrays(uValues, vValues, matrix);

                    double[][] unallocatedMatrix = CreateUnallocatedMatrix(matrix, allocatedMatrix, uValues, vValues);

                    List<Tuple<int, int>> coordinates = new List<Tuple<int, int>>();

                    int rowIndexOfEnteringElement = 0;
                    int colIndexOfEnteringElement = 0;
                    bool shouldBreak = false;

                    for (int i = 0; i < unallocatedMatrix.Length; i++)
                    {
                        for (int j = 0; j < unallocatedMatrix[i].Length; j++)
                        {
                            if (unallocatedMatrix[i][j] > 0)
                            {
                                positiveElement = unallocatedMatrix[i][j];
                                rowIndexOfEnteringElement = i;
                                colIndexOfEnteringElement = j;
                                shouldBreak = true;
                                break;
                            }
                        }

                        if (shouldBreak)
                        {
                            break;
                        }
                    }

                    Print(allocatedMatrix);
                    coordinates.Add(Tuple.Create(rowIndexOfEnteringElement, colIndexOfEnteringElement));

                    //find col neighbours


                    //find row neighbours
                    for (int j = 0; j < allocatedMatrix[0].Length; j++)
                    {
                        if (allocatedMatrix[rowIndexOfEnteringElement][j] != 0)
                        {
                            coordinates.Add(Tuple.Create(rowIndexOfEnteringElement, j));
                        }
                    }

                    foreach (var coo in coordinates)
                    {
                        Console.WriteLine(coo.Item1 + " " + coo.Item2);
                    }

                    //find path
                    List<Tuple<int, int>> path = new List<Tuple<int, int>>();


                    //coordinates.Add(Tuple.Create(rowIndex, colIndex));

                    //if ((rowIndex - 1) >= 0 && (rowIndex - 1) < unallocatedMatrix.Length &&
                    //    unallocatedMatrix[rowIndex - 1][colIndex] == 0)
                    //{
                    //    coordinates.Add(Tuple.Create(rowIndex - 1, colIndex));
                    //    rowIndex--;
                    //    colIndex++;
                    //}

                    //if ((rowIndex + 1) >= 0 && (rowIndex + 1) < unallocatedMatrix.Length &&
                    //    unallocatedMatrix[rowIndex + 1][colIndex] == 0)
                    //{
                    //    coordinates.Add(Tuple.Create(rowIndex + 1, colIndex));
                    //}

                    //if ((colIndex - 1) >= 0 && (colIndex - 1) < unallocatedMatrix[0].Length &&
                    //    unallocatedMatrix[rowIndex][colIndex - 1] == 0)
                    //{
                    //    coordinates.Add(Tuple.Create(rowIndex, colIndex - 1));
                    //}

                    //if ((colIndex + 1) >= 0 && (colIndex + 1) < unallocatedMatrix[0].Length &&
                    //    unallocatedMatrix[rowIndex][colIndex + 1] == 0)
                    //{
                    //    coordinates.Add(Tuple.Create(rowIndex, colIndex + 1));
                    //}


                    //    int[][] coordinatesArray = new int[coordinates.Count][];

                    //    for (int i = 0; i < coordinatesArray.Length; i++)
                    //    {
                    //        coordinatesArray[i] = new int[2];
                    //    }

                    //    for (int i = 0; i < coordinatesArray.Length; i++)
                    //    {
                    //        coordinatesArray[i][0] = coordinates[i].Item1;
                    //        coordinatesArray[i][1] = coordinates[i].Item2;
                    //    }

                    //    PrintInt(coordinatesArray);
                    //    Console.WriteLine();
                    //    Console.WriteLine();

                    //    List<double> minValues = new List<double>();

                    //    for (int i = 0; i < coordinatesArray.Length - 1; i++)
                    //    {
                    //        if (coordinatesArray[i][1] != coordinatesArray[i + 1][1])
                    //        {
                    //            allocatedMatrix[coordinatesArray[i][0]][coordinatesArray[i][1]] =
                    //                -allocatedMatrix[coordinatesArray[i][0]][coordinatesArray[i][1]];

                    //            allocatedMatrix[coordinatesArray[i + 1][0]][coordinatesArray[i + 1][1]] =
                    //                -allocatedMatrix[coordinatesArray[i + 1][0]][coordinatesArray[i + 1][1]];

                    //            if (allocatedMatrix[coordinatesArray[i][0]][coordinatesArray[i][1]] != 0)
                    //            {
                    //                minValues.Add(allocatedMatrix[coordinatesArray[i][0]][coordinatesArray[i][1]]);
                    //            }
                    //        }
                    //    }

                    //    double minVal = minValues[0];

                    //    for (int i = 0; i < minValues.Count; i++)
                    //    {
                    //        if (minValues[i] > minVal)
                    //        {
                    //            minVal = minValues[i];
                    //        }
                    //    }

                    //    for (int i = 0; i < allocatedMatrix.Length; i++)
                    //    {
                    //        for (int j = 0; j < allocatedMatrix[i].Length; j++)
                    //        {
                    //            for (int k = 0; k < coordinatesArray.Length; k++)
                    //            {
                    //                if (i == coordinatesArray[k][0] && j == coordinatesArray[k][1] &&
                    //                    allocatedMatrix[i][j] >= 0)
                    //                {
                    //                    allocatedMatrix[i][j] = allocatedMatrix[i][j] + Math.Abs(minVal);
                    //                }
                    //                else if (i == coordinatesArray[k][0] && j == coordinatesArray[k][1] &&
                    //                         allocatedMatrix[i][j] < 0)
                    //                {
                    //                    allocatedMatrix[i][j] = Math.Abs(allocatedMatrix[i][j]) - Math.Abs(minVal);
                    //                }
                    //            }
                    //        }
                    //    }

                    //    unallocatedMatrix = CreateUnallocatedMatrix(matrix, allocatedMatrix, uValues, vValues);
                    //    Console.WriteLine("UN");
                    //    Print(unallocatedMatrix);

                    //    for (int i = 0; i < unallocatedMatrix.Length; i++)
                    //    {
                    //        finished = unallocatedMatrix[i].All(x => x <= 0);
                    //    }
                    //}

                    //matrix.TempMatrix = allocatedMatrix;
                }
            }

            return matrix;
        }

        public void FindColNeighbours(double[][] allocatedMatrix, int colIndexOfEnteringElement)
        {
            for (int i = 0; i < allocatedMatrix.Length; i++)
            {
                if (allocatedMatrix[i][colIndexOfEnteringElement] != 0)
                {
                    coordinates.Add(Tuple.Create(i, colIndexOfEnteringElement));
                }
            }
        }
        */

        public Matrix UVMethod(Matrix matrix)
        {
            double?[] uValues = new double?[matrix.Restrictions - 1];
            double?[] vValues = new double?[matrix.Variables - 1];

            CreateUVArrays(uValues, vValues, matrix);

            double[][] allocatedMatrix = CreateAllocatedMatrix(matrix);

            Console.WriteLine("Allocated Matrix");
            Print(allocatedMatrix);
            Console.WriteLine();
            Console.WriteLine();

            double[][] potentials = CreatePotentialsMatrix(matrix, allocatedMatrix, uValues, vValues);

            Console.WriteLine("Potentials:");
            Print(potentials);
            Console.WriteLine();
            Console.WriteLine();

            List<Tuple<int, int>> path = new List<Tuple<int, int>>();
            double min = 0;

            while (!Optimized(potentials))
            {
                var enteringElementIndexes = FindEnteringElementIndexes(potentials);
                path = FindPath(potentials, enteringElementIndexes.Item1, enteringElementIndexes.Item2);

                foreach (var tuple in path)
                {
                    if (allocatedMatrix[tuple.Item1][tuple.Item2] < min)
                    {
                        min = allocatedMatrix[tuple.Item1][tuple.Item2];
                    }
                }

                allocatedMatrix[enteringElementIndexes.Item1][enteringElementIndexes.Item2] = min;

                foreach (var tuple in path)
                {
                        allocatedMatrix[tuple.Item1][tuple.Item2] -= min;
                }

                potentials = CreatePotentialsMatrix(matrix, allocatedMatrix, uValues, vValues);
            }

            return matrix;
        }

        public List<Tuple<int, int>> FindPath(double[][] potentials, int rowIndexEnteringElement, int colIndexEnteringElement)
        {
            List<Tuple<int, int>> path = new List<Tuple<int, int>>();
            List<List<Tuple<int, int>>> wrongPath = new List<List<Tuple<int, int>>>();

            List<Tuple<int, int>> rowIndexes = FindRowNeighbours(potentials, rowIndexEnteringElement);

            List<Tuple<int, int>> colIndexes = FindColNeighboirs(potentials, colIndexEnteringElement);

            Tuple<int, int> nextNode;

            foreach (var index in colIndexes)
            {
                path.Add(index);
                while (true)
                {
                    nextNode = FindNextNode(potentials, path, wrongPath);
                    if (nextNode != null)
                    {
                        path.Add(nextNode);
                        if (rowIndexes.Contains(nextNode) && path.Count % 2 != 0)
                        {
                            return path;
                        }
                    }
                    else if (path.Count == 1)
                    {
                        break;
                    }
                    else
                    {
                        wrongPath.Add(path);
                        List<Tuple<int, int>> tempTuple = new List<Tuple<int, int>>();
                        tempTuple.Add(index);
                        path = tempTuple;
                    }
                }
            }

            return null;
        }

        public Tuple<int, int> FindNextNode(double[][] potentials, List<Tuple<int, int>> path, List<List<Tuple<int, int>>> wrongPath)
        {
            List<Tuple<int, int>> nodes = new List<Tuple<int, int>>();
            if (path.Count % 2 == 0)
            {
                nodes = FindColNeighboirs(potentials, path[path.Count - 1].Item2);
            }
            else
            {
                nodes = FindRowNeighbours(potentials, path[path.Count - 1].Item1);
            }

            foreach (var node in nodes)
            {
                List<Tuple<int, int>> tempNodes = new List<Tuple<int, int>>();
                tempNodes.AddRange(path);
                tempNodes.Add(node);
                if (!path.Contains(node) && !wrongPath.Contains(tempNodes))
                {
                    return node;
                }
            }

            return null;
        }

        public List<Tuple<int, int>> FindRowNeighbours(double[][] potentials, int rowIndexEnteringElement)
        {
            List<Tuple<int, int>> indexes = new List<Tuple<int, int>>();
            for (int j = 0; j < potentials[0].Length; j++)
            {
                if (potentials[rowIndexEnteringElement][j] == 0)
                {
                    indexes.Add(Tuple.Create(rowIndexEnteringElement, j));
                }
            }

            return indexes;
        }

        public List<Tuple<int, int>> FindColNeighboirs(double[][] potentials, int colIndeEnteringElement)
        {
            List<Tuple<int, int>> indexes = new List<Tuple<int, int>>();
            for (int i = 0; i < potentials.Length; i++)
            {
                if (potentials[i][colIndeEnteringElement] == 0)
                {
                    indexes.Add(Tuple.Create(i, colIndeEnteringElement));
                }
            }

            return indexes;
        }

        public bool Optimized(double[][] potentials)
        {
            for (int i = 0; i < potentials.Length; i++)
            {
                for (int j = 0; j < potentials[i].Length; j++)
                {
                    if (potentials[i][j] < 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public Tuple<int, int> FindEnteringElementIndexes(double[][] potentials)
        {
            int rowIndexEnteringElement = 0;
            int colIndexEnteringElement = 0;
            bool shouldBreak = false;

            for (int i = 0; i < potentials.Length; i++)
            {
                for (int j = 0; j < potentials[i].Length; j++)
                {
                    if (potentials[i][j] > 0)
                    {
                        rowIndexEnteringElement = i;
                        colIndexEnteringElement = j;
                        shouldBreak = true;
                        break;
                    }
                }

                if (shouldBreak)
                {
                    break;
                }
            }

            return Tuple.Create(rowIndexEnteringElement, colIndexEnteringElement);
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

        public double[][] CreatePotentialsMatrix(Matrix matrix, double[][] allocatedMatrix, double?[] uValues, double?[] vValues)
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

        public void PrintInt(int[][] matrix)
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