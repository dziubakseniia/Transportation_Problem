using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main()
        {
            Matrix matrix = new Matrix();
            matrix.Restrictions = 5;
            matrix.Variables = 5;
            matrix.CMatrix = new double[matrix.Restrictions][];
            for (int i = 0; i < matrix.Restrictions; i++)
            {
                matrix.CMatrix[i] = new double[matrix.Variables];
            }

            matrix.CMatrix[0][0] = 1;
            matrix.CMatrix[0][1] = 2;
            matrix.CMatrix[0][2] = 9;
            matrix.CMatrix[0][3] = 7;
            matrix.CMatrix[0][4] = 60;

            matrix.CMatrix[1][0] = 3;
            matrix.CMatrix[1][1] = 4;
            matrix.CMatrix[1][2] = 1;
            matrix.CMatrix[1][3] = 5;
            matrix.CMatrix[1][4] = 55;

            matrix.CMatrix[2][0] = 6;
            matrix.CMatrix[2][1] = 4;
            matrix.CMatrix[2][2] = 8;
            matrix.CMatrix[2][3] = 3;
            matrix.CMatrix[2][4] = 40;

            matrix.CMatrix[3][0] = 2;
            matrix.CMatrix[3][1] = 3;
            matrix.CMatrix[3][2] = 3;
            matrix.CMatrix[3][3] = 1;
            matrix.CMatrix[3][4] = 35;

            matrix.CMatrix[4][0] = 70;
            matrix.CMatrix[4][1] = 5;
            matrix.CMatrix[4][2] = 45;
            matrix.CMatrix[4][3] = 70;
            matrix.CMatrix[4][4] = 0;

            matrix.LeastCost(matrix);
            matrix.UVMethod(matrix);

            matrix.Print(matrix.CMatrix);
            Console.WriteLine();
            Console.WriteLine();
            matrix.Print(matrix.TempMatrix);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(matrix.Sum(matrix.CMatrix, matrix.TempMatrix));
        }
    }
}
