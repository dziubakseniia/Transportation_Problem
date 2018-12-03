using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main()
        {
            Matrix matrix = new Matrix();
            matrix.Restrictions = 4;
            matrix.Variables = 5;
            matrix.CMatrix = new double[matrix.Restrictions][];
            for (int i = 0; i < matrix.Restrictions; i++)
            {
                matrix.CMatrix[i] = new double[matrix.Variables];
            }

            matrix.CMatrix[0][0] = 3;
            matrix.CMatrix[0][1] = 1;
            matrix.CMatrix[0][2] = 7;
            matrix.CMatrix[0][3] = 4;
            matrix.CMatrix[0][4] = 300;

            matrix.CMatrix[1][0] = 2;
            matrix.CMatrix[1][1] = 6;
            matrix.CMatrix[1][2] = 5;
            matrix.CMatrix[1][3] = 9;
            matrix.CMatrix[1][4] = 400;

            matrix.CMatrix[2][0] = 8;
            matrix.CMatrix[2][1] = 3;
            matrix.CMatrix[2][2] = 3;
            matrix.CMatrix[2][3] = 2;
            matrix.CMatrix[2][4] = 500;

            matrix.CMatrix[3][0] = 250;
            matrix.CMatrix[3][1] = 350;
            matrix.CMatrix[3][2] = 400;
            matrix.CMatrix[3][3] = 200;
            matrix.CMatrix[3][4] = 0;

            matrix.LeastCost(matrix);

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
