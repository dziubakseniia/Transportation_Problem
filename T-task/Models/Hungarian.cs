using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T_task.Models
{
    public class Hungarian
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

        public double MinHungarianMethod()
        {
            double[][] tArray = new double[Restrictions][];
            for (int i = 0; i < Restrictions; i++)
            {
                tArray[i] = new double[Variables];
            }

            CreateTempMatrix(this);
            CopyCMatrixToTArray(tArray, this);
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
            Create1_sTempMatrix();

            return Sum(tArray);
        }

        public double MaxHungarianMethod()
        {
            double[][] tArray = new double[Restrictions][];
            for (int i = 0; i < Restrictions; i++)
            {
                tArray[i] = new double[Variables];
            }

            CreateTempMatrix(this);
            CopyCMatrixToTArray(tArray, this);
            rows = new int[Restrictions];
            occupiedCols = new double[Restrictions];

            TempMatrix = SubtractMaxFromRow();
            TempMatrix = SubtractMinFromCol();

            CrossZeros();
            while (numLines < Restrictions)
            {
                createAdditionalZeros();
                CrossZeros();
            }

            IsOptimal();
            Create1_sTempMatrix();

            return Sum(tArray);
        }

        private void Create1_sTempMatrix()
        {
            for (int i = 0; i < Restrictions; i++)
            {
                for (int j = 0; j < Variables; j++)
                {
                    TempMatrix[i][j] = 0;
                }
            }
            foreach (var index in indexes)
            {
                TempMatrix[index.Item1][index.Item2] = 1;
            }
        }

        private double[][] SubtractMaxFromRow()
        {
            List<double> maxList = FindMaxInRow();
            for (int i = 0; i < Restrictions; i++)
            {
                for (int j = 0; j < Variables; j++)
                {
                    CMatrix[i][j] *= -1;
                }
            }

            for (int i = 0; i < Restrictions; i++)
            {
                for (int j = 0; j < Variables; j++)
                {
                    CMatrix[i][j] -= maxList[i];
                }
            }

            return CMatrix;
        }

        private double[][] SubtractMinFromRow()
        {
            List<double> minList = FindMinInRow();
            for (int i = 0; i < Restrictions; i++)
            {
                for (int j = 0; j < Variables; j++)
                {
                    CMatrix[i][j] -= minList[i];
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

        private List<double> FindMaxInRow()
        {
            List<double> maxList = new List<double>();
            for (int i = 0; i < Restrictions; i++)
            {
                var max = CMatrix[i][0];
                for (int j = 0; j < Variables; j++)
                {
                    if (CMatrix[i][j] > max)
                    {
                        max = CMatrix[i][j];
                    }
                }
                maxList.Add(max);
            }

            return maxList;
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
            Lines = new double[Restrictions][];
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
                    }
                }
            }
        }

        private int MaxVerticalHorizontal(int row, int col)
        {
            int result = 0;
            for (int i = 0; i < Restrictions; i++)
            {
                if (TempMatrix[i][col] == 0)
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
            indexes.Clear();
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
                    indexes.Add(new Tuple<int, int>(row, col));
                    if (IsOptimal(row + 1))
                    {
                        return true;
                    }

                    occupiedCols[col] = 0;
                    indexes.Remove(new Tuple<int, int>(row, col));
                }
            }

            return false;
        }

        public bool IsOptimal()
        {
            return IsOptimal(0);
        }

        public void CreateTempMatrix(Hungarian matrix)
        {
            TempMatrix = new double[matrix.Restrictions][];

            for (int i = 0; i < matrix.Restrictions; i++)
            {
                matrix.TempMatrix[i] = new double[matrix.Variables];
            }
        }

        public void CopyCMatrixToTArray(double[][] tArray, Hungarian matrix)
        {
            for (int i = 0; i < matrix.Restrictions; i++)
            {
                for (int j = 0; j < matrix.Variables; j++)
                {
                    tArray[i][j] = matrix.CMatrix[i][j];
                }
            }
        }

        public double Sum(double[][] tArray)
        {
            double sum = 0;
            for (int i = 0; i < Restrictions; i++)
            {
                for (int j = 0; j < Variables; j++)
                {
                    if (TempMatrix[i][j] == 1)
                    {
                        sum += tArray[i][j];
                    }
                }
            }

            return sum;
        }
    }
}