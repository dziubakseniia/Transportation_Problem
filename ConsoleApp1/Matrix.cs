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
        public double[][] Lines { get; set; }

        private int numLines;
        private int[] rows;
        private double[] occupiedCols;
        private List<Tuple<int, int>> indexes = new List<Tuple<int, int>>();

        #region TransportationProblem     
        /*
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
            double?[] uValues = new double?[matrix.Restrictions - 1];
            double?[] vValues = new double?[matrix.Variables - 1];

            CreateUVArrays(uValues, vValues, matrix);

            double[][] allocatedMatrix = CreateAllocatedMatrix(matrix);

            double[][] potentials = CreatePotentialsMatrix(matrix, allocatedMatrix, uValues, vValues);

            List<Tuple<int, int>> path = new List<Tuple<int, int>>();
            double min = double.MaxValue;

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

            while (uValues.Contains(null) && vValues.Contains(null))
            {
                for (int i = 0; i < matrix.Restrictions - 1; i++)
                {
                    for (int j = 0; j < matrix.Variables - 1; j++)
                    {
                        if (allocatedMatrix[i][j] == 0)
                        {
                            if (vValues[j] == null && uValues[i] != null)
                            {
                                vValues[j] = unallocatedMatrix[i][j] + uValues[i];
                            }

                            if (uValues[i] == null && vValues[j] != null)
                            {
                                uValues[i] = vValues[j] - unallocatedMatrix[i][j];
                            }
                        }
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
        */
        #endregion

        #region AssignmentProblem

        public double[][] HungarianMethod()
        {
            CreateTempMatrix(this);
            rows = new int[Restrictions];
            occupiedCols = new double[Restrictions];

            TempMatrix = SubtractMinFromRow();
            TempMatrix = SubtractMinFromCol();

            CrossZeros();
            while (numLines < Restrictions)
            {
                createAdditionalZeros();
                CrossZeros();
            }

            IsOptimal();
            return TempMatrix;
        }

        private double[][] SubtractMinFromRow()
        {
            List<double> minList = FindMinInRow();
            for (int i = 0; i < Restrictions; i++)
            {
                for (int j = 0; j < Variables; j++)
                {
                    TempMatrix[i][j] -= minList[i];
                }
            }

            return CMatrix;
        }

        private double[][] SubtractMinFromCol()
        {
            List<double> minList = FindMinInColumn();
            for (int j = 0; j < Variables; j++)
            {
                for (int i = 0; i < Restrictions; i++)
                {
                    CMatrix[i][j] -= minList[j];
                }
            }

            return CMatrix;
        }

        private List<double> FindMinInRow()
        {
            List<double> minList = new List<double>();
            for (int i = 0; i < Restrictions; i++)
            {
                var min = CMatrix[i][0];
                for (int j = 0; j < Variables; j++)
                {
                    if (CMatrix[i][j] < min)
                    {
                        min = CMatrix[i][j];
                    }
                }
                minList.Add(min);
            }

            return minList;
        }

        private List<double> FindMinInColumn()
        {
            List<double> minList = new List<double>();
            for (int j = 0; j < Variables; j++)
            {
                var min = CMatrix[0][j];
                for (int i = 0; i < Restrictions; i++)
                {
                    if (CMatrix[i][j] < min)
                    {
                        min = CMatrix[i][j];
                    }
                }
                minList.Add(min);
            }

            return minList;
        }

        private void CrossZeros()
        {
            numLines = 0;
            Lines= new double[Restrictions][];
            for (int i = 0; i < Restrictions; i++)
            {
                Lines[i] = new double[Variables];
            }

            for (int i = 0; i < Restrictions; i++)
            {
                for (int j = 0; j < Variables; j++)
                {
                    if (TempMatrix[i][j] == 0)
                    {
                        crossedNeighbours(i, j, MaxVerticalHorizontal(i, j));
                        indexes.Add(new Tuple<int, int>(i, j));
                    }
                }
            }

            Console.WriteLine("Indexes:");
            foreach (var index in indexes)
            {
                Console.WriteLine(index.Item1 + " " + index.Item2);
            }
        }

        private int MaxVerticalHorizontal(int row, int col)
        {
            int result = 0;
            for (int i = 0; i < Restrictions; i++)
            {
                if(TempMatrix[i][col] == 0)
                {
                    result++;
                }

                if (TempMatrix[row][i] == 0)
                {
                    result--;
                }
            }

            return result;
        }

        private void crossedNeighbours(int row, int col, int maxVerticalHorizontal)
        {
            if (Lines[row][col] == 2)
            {
                return;
            }

            if (maxVerticalHorizontal > 0 && Lines[row][col] == 1)
            {
                return;
            }

            if (maxVerticalHorizontal <= 0 && Lines[row][col] == -1)
            {
                return;
            }

            for (int i = 0; i < Restrictions; i++)
            {
                if (maxVerticalHorizontal > 0)
                {
                    Lines[i][col] = Lines[i][col] == -1 || Lines[i][col] == 2 ? 2 : 1;
                }
                else
                {
                    Lines[row][i] = Lines[row][i] == 1 || Lines[row][i] == 2 ? 2 : -1;
                }
            }

            numLines++;
        }

        public void createAdditionalZeros()
        {
            double minUncoveredValue = 0;

            for (int i = 0; i < Restrictions; i++)
            {
                for (int j = 0; j < Variables; j++)
                {
                    if (Lines[i][j] == 0 && (TempMatrix[i][j] < minUncoveredValue || minUncoveredValue == 0))
                    {
                        minUncoveredValue = TempMatrix[i][j];
                    }
                }
            }

            for (int i = 0; i < Restrictions; i++)
            {
                for (int j = 0; j < Variables; j++)
                {
                    if (Lines[i][j] == 0)
                    {
                        TempMatrix[i][j] -= minUncoveredValue;
                    }
                    else if (Lines[i][j] == 2)
                    {
                        TempMatrix[i][j] += minUncoveredValue;
                    }
                }
            }
        }

        private bool IsOptimal(int row)
        {
            if (row == rows.Length)
            {
                return true;
            }

            for (int col = 0; col < Variables; col++)
            {
                if (TempMatrix[row][col] == 0 && occupiedCols[col] == 0)
                {
                    rows[row] = col;
                    occupiedCols[col] = 1;
                    if (IsOptimal(row + 1))
                    {
                        return true;
                    }

                    occupiedCols[col] = 0;
                }
            }

            return false;
        }

        public bool IsOptimal()
        {
            return IsOptimal(0);
        }

        public double GetTotal()
        {
            double total = 0;
            for (int i = 0; i < Restrictions; i++)
            {
                total += CMatrix[i][rows[i]];
            }

            return total;
        }

        public void CreateTempMatrix(Matrix matrix)
        {
            TempMatrix = new double[matrix.Restrictions][];

            for (int i = 0; i < matrix.Restrictions; i++)
            {
                matrix.TempMatrix[i] = new double[matrix.Variables];
            }
        }

        #endregion
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