using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fulleren1
{
    class Coords
    {
        double x, y, z;
        public Coords()
        {
            x = 0;
            y = 0;
            z = 0;
        }
        public Coords(double x, double y)
        {
            this.x = x;
            this.y = y;
            z = 0;
        }
        public static Coords operator +(Coords c1, Coords c2) =>
        new Coords(c1.x + c2.x, c1.y + c2.y);
        public static Coords operator -(Coords c1, Coords c2) =>
        new Coords(c1.x - c2.x, c1.y - c2.y);
        public static Coords operator *(double d, Coords c) =>
        new Coords(d * c.x, d * c.y);
        public static Coords operator *(Coords c, double d) =>
        new Coords(d * c.x, d * c.y);
        public bool belongs(Rectangle S)
        {
            if (this.x > 0 && this.x < S.a && this.y > 0 && this.y < S.b)
                return true;
            else
                return false;
        }
        public override String ToString()
        {
            int p = 2;
            //Console.WriteLine(Math.Round(x, p) + "\t" + Math.Round(y, p) + "\t0\tC");
            return Math.Round(x, p) + "\t" + Math.Round(y, p) + "\t0\tC";
        }
    }
    class Rectangle
    {
        public double a, b;
        public Rectangle(double a, double b)
        {
            this.a = a;
            this.b = b;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            double a, b;
            Console.Write("a = ");
            a = Double.Parse(Console.ReadLine());
            Console.WriteLine("Вы ввели a = " + a);
            Console.Write("b = ");
            b = Double.Parse(Console.ReadLine());
            Console.WriteLine("Вы ввели b = " + b);
            Rectangle S = new Rectangle(a, b);

            double d = 1.42; //Расстояние между атомами
            double h = d * Math.Sqrt(3); //Высота



            Coords center0 = new Coords(d, h / 2);

    

            Coords center00 = new Coords(d, h / 2);

            Coords V1 = new Coords(d / 2, h / 2); //Вектор вверх и вправо от центра до ближашего атома
            Coords V2 = new Coords(-d / 2, h / 2); //Вектор вверх и влево от центра до ближашего атома
            Coords V3 = new Coords(d, 0); //Вектор вправо от центра до ближашего атома
            Coords Vtop = new Coords(0, h);
            Coords Vright = new Coords(3 * d / 2, 0);
            Coords Vt = new Coords(h / 2, 0);

            Coords Vr1 = new Coords(3 * d / 2, -h / 2); //Смещение в центр, который справа внизу от текущего
            Coords Vr2 = new Coords(3 * d / 2, h / 2); //Смещение в центр, который справа вверху от текущего

            List<Coords> list = new List<Coords>();
            List<Coords> column;
            List<List<Coords>> structure = new List<List<Coords>>();

            bool flag = center0.belongs(S);

            if(!flag)
            {
                Console.WriteLine("Введите другие значения a или b");
                Console.ReadKey();
                return;
            }

            bool even = false;//четное
            Coords newCenter;
            
            while (flag)
            {
                column = new List<Coords>();
                column.Add(center0);
                newCenter = center0 + Vtop;
                while (newCenter.belongs(S))
                {
                    column.Add(newCenter);
                    newCenter += Vtop;
                }
                structure.Add(column);
                even = (even ? false : true);
                center0 += (even ? Vr2 : Vr1);
                flag = center0.belongs(S);
            }
            foreach(List<Coords> l in structure)
            {
                foreach (Coords c in l)
                    Console.Write("1 ");
                Console.WriteLine();
            }
            /*
            foreach (List<Coords> l in structure)
            {
                foreach (Coords c in l)
                    c.show();
                Console.WriteLine();
            }
            */
            even = false;
            center0 = new Coords(d, h / 2);
            for (int i = 0; i < structure.Count; i++)
            {
                for (int j = 0; j < structure[i].Count; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        list.Add(structure[i][j] - V1);
                        list.Add(structure[i][j] - V2);
                        list.Add(structure[i][j] - V3);
                        list.Add(structure[i][j] + V3);
                        list.Add(structure[i][j] + V2);
                        list.Add(structure[i][j] + V1);
                    }
                    else
                    {
                        if (i == 0)
                        {
                            list.Add(structure[i][j] + V3);
                            list.Add(structure[i][j] - V3);
                            list.Add(structure[i][j] + V2);
                            list.Add(structure[i][j] + V1);
                        }
                        else
                        {
                            if (even)
                            {
                                list.Add(structure[i][j] + V3);
                                list.Add(structure[i][j] + V1);
                                if (j == 0)
                                {
                                    list.Add(structure[i][j] - V2);
                                }
                                if (!(j < structure[i - 1].Count - 1))
                                {
                                    list.Add(structure[i][j] + V2);
                                }
                            }
                            else
                            {
                                list.Add(structure[i][j] + V3);
                                list.Add(structure[i][j] + V1);


                                if (j == 0)
                                {
                                    list.Add(structure[i][j] - V1);
                                    list.Add(structure[i][j] - V2);
                                }
                                if (!(j < structure[i - 1].Count))
                                {
                                    list.Add(structure[i][j] + V2);
                                }
                            }
                        }
                    }
                }
                even = (even ? false : true);
            }
            Console.WriteLine("x" + '\t' + "y" + '\t' + "z" + '\t' + "atom");

            foreach (Coords item in list)
            {
                Console.WriteLine(item);
            }


            using (StreamWriter sw = new StreamWriter("out.txt"))
            {
                foreach (Coords item in list)
                    sw.WriteLine(item);
            }
            Console.ReadKey();
        }
    }
}
