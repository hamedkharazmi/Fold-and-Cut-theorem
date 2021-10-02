using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fold_paper
{
    class Program
    {
        static void Main(string[] args)
        {
            int row, column;
            Console.WriteLine("Enter row:");
            row = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Enter column:");
            column = Int32.Parse(Console.ReadLine());
            string order;
            order = Console.ReadLine();
            var paper = new string[row, column];
            for (int i = 0; i < row; i++)
                for (int j = 0; j < column; j++)
                    paper[i, j] = Convert.ToString((char)('A' + i * column + j));
            string foldOrder = "";
            FindFoldOrder(paper, order, ref foldOrder);
            Console.WriteLine(foldOrder.Remove(0,1));
            Console.ReadKey();
        }
        static bool FindFoldOrder(string[,] paper, string order, ref string foldOrder)
        {
            for (int i = 1; i < paper.GetLength(0); i++)
            {
                if (HorizontalCheckFold(paper, i, order))
                {
                    if (paper.GetLength(0) == 2 && paper.GetLength(1) == 1)
                    {
                        foldOrder += " H1U";
                        return true;
                    }
                    var str = foldOrder + $" H{i}L";
                    var temp = FoldHorizontal(paper, i, true);
                    if (FindFoldOrder(temp, order, ref str))
                    {
                        foldOrder = str;
                        return true;
                    }
                    str = foldOrder + $" H{i}U";
                    temp = FoldHorizontal(paper, i, false);
                    if (FindFoldOrder(temp, order, ref str))
                    {
                        foldOrder = str;
                        return true;
                    }
                }
            }
            for (int j = 1; j < paper.GetLength(1); j++)
            {
                if (VerticalCheckFold(paper, j, order))
                {
                    if (paper.GetLength(0) == 1 && paper.GetLength(1) == 2)
                    {
                        foldOrder += " V1R";
                        return true;
                    }
                    var str = foldOrder + $" V{j}R";
                    var temp = FoldVertical(paper, j, true);
                    if (FindFoldOrder(temp, order, ref str))
                    {
                        foldOrder = str;
                        return true;
                    }
                    str = foldOrder + $" V{j}L";
                    temp = FoldVertical(paper, j, false);
                    if (FindFoldOrder(temp, order, ref str))
                    {
                        foldOrder = str;
                        return true;
                    }
                }
            }
            return false;
        }
        static bool HorizontalCheckFold(string[,] paper, int row, string order)
        {
            bool flag = true;
            for (int iter = 0; row - iter != 0 && row + iter != paper.GetLength(0); iter++)
            {
                for (int j = 0; j < paper.GetLength(1); j++)
                {
                    if (!IsNeighbour(paper[row - iter - 1, j][0], paper[row + iter, j][0], order))
                    {
                        flag = false;
                        goto line2;
                    }
                }
            }
        line2:
            return flag;
        }
        static bool VerticalCheckFold(string[,] paper, int column, string order)
        {
            bool flag = true;
            for (int iter = 0; column - iter != 0 && column + iter != paper.GetLength(1); iter++)
            {
                for (int i = 0; i < paper.GetLength(0); i++)
                {
                    if (!IsNeighbour(paper[i, column - iter - 1][0], paper[i, column + iter][0], order))
                    {
                        flag = false;
                        goto line3;
                    }
                }
            }
        line3:
            return flag;
        }
        static string[,] FoldVertical(string[,] paper, int column, bool right)
        {
            int clmn = (paper.GetLength(1) / 2 < column) ? column : paper.GetLength(1) - column;
            var res = new string[paper.GetLength(0), clmn];
            if (right)
            {
                for (int iter = 0; column - iter != 0 && column + iter != paper.GetLength(1); iter++)
                {
                    for (int i = 0; i < paper.GetLength(0); i++)
                    {
                        var chL = paper[i, column - iter - 1];
                        var chR = paper[i, column + iter];
                        var temp = chR[chR.Length - 1].ToString() + chL[chL.Length - 1];
                        res[i, res.GetLength(1) - iter - 1] = temp;
                    }
                }
                for (int i = 0; i < paper.GetLength(0); i++)
                    for (int j = 0; j < 2 * res.GetLength(1) - paper.GetLength(1); j++)
                        if (paper.GetLength(1) / 2 < column)
                        {
                            res[i, j] = paper[i, j];
                        }
                        else
                        {
                            res[i, j] = string.Join("", paper[i, paper.GetLength(1) - j - 1].Reverse());
                        }
            }
            else
            {
                for (int iter = 0; column - iter != 0 && column + iter != paper.GetLength(1); iter++)
                {
                    for (int i = 0; i < paper.GetLength(0); i++)
                    {
                        var chL = paper[i, column - iter - 1];
                        var chR = paper[i, column + iter];
                        var temp = chL[chL.Length - 1].ToString() + chR[chR.Length - 1];
                        res[i, iter] = temp;
                    }
                }
                for (int i = 0; i < paper.GetLength(0); i++)
                    for (int j = 2 * (paper.GetLength(1) - res.GetLength(1)); j < paper.GetLength(1); j++)
                        if (paper.GetLength(1) / 2 < column)
                        {
                            res[i, j - column] = string.Join("", paper[i, paper.GetLength(1) - 1 - j].Reverse());
                        }
                        else
                        {
                            res[i, j - column] = paper[i, j];
                        }
            }
            return res;
        }
        static string[,] FoldHorizontal(string[,] paper, int row, bool low)
        {
            int rw = (paper.GetLength(0) / 2 < row) ? row : paper.GetLength(0) - row;
            var res = new string[rw, paper.GetLength(1)];
            if (low)
            {
                for (int iter = 0; row - iter != 0 && row + iter != paper.GetLength(0); iter++)
                {
                    for (int j = 0; j < paper.GetLength(1); j++)
                    {
                        var chU = paper[row - iter - 1, j];
                        var chL = paper[row + iter, j];
                        var temp = chL[chL.Length - 1].ToString() + chU[chU.Length - 1];
                        res[res.GetLength(0) - iter - 1, j] = temp;
                    }
                }
                for (int i = 0; i < 2 * res.GetLength(0) - paper.GetLength(0); i++)
                    for (int j = 0; j < paper.GetLength(1); j++)
                        if (paper.GetLength(0) / 2 < row)
                        {
                            res[i, j] = paper[i, j];
                        }
                        else
                        {
                            res[i, j] = string.Join("", paper[paper.GetLength(0) - i - 1, j].Reverse());
                        }
            }
            else
            {
                for (int iter = 0; row - iter != 0 && row + iter != paper.GetLength(0); iter++)
                {
                    for (int j = 0; j < paper.GetLength(1); j++)
                    {
                        var chU = paper[row - iter - 1, j];
                        var chL = paper[row + iter, j];
                        var temp = chU[chU.Length - 1].ToString() + chL[chL.Length - 1];
                        res[iter, j] = temp;
                    }
                }
                for (int i = 2 * (paper.GetLength(0) - res.GetLength(0)); i < paper.GetLength(0); i++)
                    for (int j = 0; j < paper.GetLength(1); j++)
                        if ((paper.GetLength(0) / 2 < row))
                        {
                            res[i - row, j] = string.Join("", paper[paper.GetLength(0) - 1 - i, j].Reverse());
                        }
                        else
                        {
                            res[i - row, j] = paper[i, j];
                        }
            }
            return res;
        }
        static bool IsNeighbour(char ch1, char ch2, string order)
        {

            for (int i = 0; i < order.Length; i++)
            {
                if (ch1 == order[i])
                {
                    if (i != 0) if (order[i - 1] == ch2) return true;
                    if (i != order.Length - 1) if (order[i + 1] == ch2) return true;
                    return false;
                }
            }
            return false;
        }
    }
}
