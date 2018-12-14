using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main()
        {
            #region TransportationProblem
            /*Matrix matrix = new Matrix();
            matrix.Restrictions = 4;
            matrix.Variables = 5;
            matrix.CMatrix = new double[matrix.Restrictions][];
            for (int i = 0; i < matrix.Restrictions; i++)
            {
                matrix.CMatrix[i] = new double[matrix.Variables];
            }

            matrix.CMatrix[0][0] = 4;
            matrix.CMatrix[0][1] = 4;
            matrix.CMatrix[0][2] = 2;
            matrix.CMatrix[0][3] = 5;
            matrix.CMatrix[0][4] = 150;

            matrix.CMatrix[1][0] = 5;
            matrix.CMatrix[1][1] = 3;
            matrix.CMatrix[1][2] = 1;
            matrix.CMatrix[1][3] = 2;
            matrix.CMatrix[1][4] = 60;

            matrix.CMatrix[2][0] = 2;
            matrix.CMatrix[2][1] = 1;
            matrix.CMatrix[2][2] = 4;
            matrix.CMatrix[2][3] = 2;
            matrix.CMatrix[2][4] = 80;

            matrix.CMatrix[3][0] = 110;
            matrix.CMatrix[3][1] = 40;
            matrix.CMatrix[3][2] = 60;
            matrix.CMatrix[3][3] = 80;
            matrix.CMatrix[3][4] = 0;

            matrix.LeastCost(matrix);
            matrix.UVMethod(matrix);

            matrix.Print(matrix.CMatrix);
            Console.WriteLine();
            Console.WriteLine();
            matrix.Print(matrix.TempMatrix);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(matrix.Sum(matrix.CMatrix, matrix.TempMatrix));


            matrix.Variables = Convert.ToInt32(4);
            matrix.Restrictions = Convert.ToInt32(3);

            matrix.CMatrix = new double[matrix.Restrictions + 1][];

            for (int i = 0; i < matrix.Restrictions + 1; i++)
            {
                matrix.CMatrix[i] = new double[matrix.Variables + 1];
            }

            for (int i = 0; i < matrix.Restrictions; i++)
            {
                for (int j = 0; j < matrix.Variables; j++)
                {
                    matrix.CMatrix[i][j] = 1;
                }
            }

            Console.WriteLine(matrix.CMatrix.Length);
            Console.WriteLine(matrix.CMatrix[0].Length);*/
            #endregion

            #region AssignmentProblem

            Matrix matrix = new Matrix();

            matrix.Restrictions = 5;
            matrix.Variables = 5;

            matrix.CMatrix = new double[matrix.Restrictions][];

            for (int i = 0; i < matrix.Restrictions; i++)
            {
                matrix.CMatrix[i] = new double[matrix.Variables];
            }

            matrix.CMatrix[0][0] = 2;
            matrix.CMatrix[0][1] = 4;
            matrix.CMatrix[0][2] = 1;
            matrix.CMatrix[0][3] = 3;
            matrix.CMatrix[0][4] = 3;

            matrix.CMatrix[1][0] = 1;
            matrix.CMatrix[1][1] = 5;
            matrix.CMatrix[1][2] = 4;
            matrix.CMatrix[1][3] = 1;
            matrix.CMatrix[1][4] = 2;

            matrix.CMatrix[2][0] = 3;
            matrix.CMatrix[2][1] = 5;
            matrix.CMatrix[2][2] = 2;
            matrix.CMatrix[2][3] = 2;
            matrix.CMatrix[2][4] = 4;

            matrix.CMatrix[3][0] = 1;
            matrix.CMatrix[3][1] = 4;
            matrix.CMatrix[3][2] = 3;
            matrix.CMatrix[3][3] = 1;
            matrix.CMatrix[3][4] = 4;

            matrix.CMatrix[4][0] = 3;
            matrix.CMatrix[4][1] = 2;
            matrix.CMatrix[4][2] = 5;
            matrix.CMatrix[4][3] = 3;
            matrix.CMatrix[4][4] = 5;

            //Console.WriteLine(matrix.MinHungarianMethod());

            //matrix.Print(matrix.CMatrix);
            Console.WriteLine(new string('-', 50));

            Console.WriteLine(matrix.MaxHungarianMethod());
            matrix.Print(matrix.CMatrix);

            #endregion
        }
    }
}
