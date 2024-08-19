﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using cocult.Comands;
using MessagePack;

namespace cocult
{
    enum Figurs : byte
    {
        Tringle,
        Circle,
        Square,
        Polygon,
        Rectengle
    }

    /// <summary>
    /// класс для работы с приложением
    /// </summary>
    class App
    {
        /// <summary>
        /// список для хранения данных о введенных фигурах 
        /// </summary>
        private ListFigure<Figure> _listEnteredShapes = new ListFigure<Figure>();

        /// <summary>
        /// список для хранения всех сохраненных файлов
        /// </summary>
        private List<string> _paths = new List<string>();

        /// <summary>
        /// лист для хранения команд
        /// </summary>
        private List<IComand> _comands = new List<IComand>();

        public App()
        {
            Reg();
        }

        /// <summary>
        /// метод который запускает программу
        /// </summary>
        public void Run()
        {
            while (true)
            {       
                Console.WriteLine($"\n1 - квадрат, вводим сторону одну" +
                    "\n2 - прямоугольник, вводим два стороны" +
                    "\n3 - Круг, вводим радиус" +
                    "\n4 - треугольник, вводим три стороны" +
                    "\n5 - многоугольник" +
                    "\n6 - вывести сумму всех периметров" +
                    "\n7 - вывести сумму всех площадей" +
                    "\n8 - вывод списка всех фигур ранее введенных" +
                    "\n9 - вывод суммы площадей всех квадратов" +
                    "\n10 - вывод суммы площадей всех кругов" +
                    "\n11 - вывод суммы площадей всех прямоугольников" +
                    "\n12 - вывод суммы площадей всех треугольников" +
                    "\n13 - вывод суммы площадей всех многоугольников" +
                    "\n14 - вывод суммы периметров всех квадратов" +
                    "\n15 - вывод суммы периметров всех кругов" +
                    "\n16 - вывод суммы периметров всех прямоугольников" +
                    "\n17 - вывод суммы периметров всех треугольников" +
                    "\n18 - вывод суммы периметров всех многоугольников" +
                    "\n19 - сохранить текущие фигуры в файл" +
                    "\n20 - вывод сохраненных фигур" +
                    "\n21 - выйти из программы\n");

                Console.WriteLine("Введите номер действия:");
                string? comand = Console.ReadLine();
                string[] words;

                if (comand.Split(" ", 2).Length == 2 && comand != null)
                { 
                    words = comand.Split(" ", 2);
                    SearhComand(words[0], words[1]);
                } 
                else
                {
                    words = comand.Split(" ", 1);
                    SearhComand(words[0], "");
                }
            }
        }
        /// <summary>
        /// метод для поиска команды
        /// </summary>
        /// <param name="comand">команда</param>
        private void SearhComand(string comand, string parametrs)
        {
            foreach (IComand el in _comands)
            {
                if (el.NameComand == comand)
                {
                    el.Execute(parametrs);
                    return;
                }

            }
            Console.Clear();
            Console.WriteLine("Ошибка в записи команды");
        }


        /// <summary>
        /// метод для преобразования строки в параметры для фигур
        /// </summary>
        /// <param name="parametr"></param>
        /// <returns></returns>
        public static List<int> ToParametrs(string parametr)
        {
            string[] a = parametr.Split();
            List<int> parametrs = new List<int>();

            foreach (var c in a)
            {

                if (int.TryParse(c, out int d))
                {
                    parametrs.Add(Convert.ToInt32(c));
                }
            }

            return parametrs;
        }

        /// <summary>
        /// метод для регистрации команд
        /// </summary>
        private void Reg()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            List<Type> types = assembly.GetTypes()
                .Where(t => typeof(IComand).IsAssignableFrom(t) && !t.IsInterface)
                .ToList();
            foreach (Type type in types)
            {
                _comands.Add(Activator.CreateInstance(type, _listEnteredShapes) as IComand);
            }
        }
    }
}
