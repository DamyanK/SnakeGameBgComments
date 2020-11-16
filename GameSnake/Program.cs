using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Snake
{
    class Program
    {
        //Координати
        struct Coordinates
        {
            public int row;
            public int col;

            public Coordinates(int row, int col)
            {
                this.row = row;
                this.col = col;
            }
        }

        static void Main(string[] args)
        {
        GAME:
            Console.CursorVisible = false;
            Console.Title = "SNAKE";

            //Размери
            Console.WindowHeight = 30;
            Console.WindowWidth = 40;

            //Премахване на слайдери
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;

            Random randomNumberGenerator = new Random();

            //Увеличаване на скоростта - Ще трябва за по-късно
            double speedIncreaser = 100;

            //Добавяне на нова "глава" при нулевият елемент
            Queue<Coordinates> snakeElements = new Queue<Coordinates>();
            for (int i = 0; i <= 4; i++)
            {
                snakeElements.Enqueue(new Coordinates(0, i));
            }

            //Генериране на координати за храната
            Coordinates food;
            do
            {
                food = new Coordinates(
                    randomNumberGenerator.Next(1, Console.WindowHeight - 1),
                    randomNumberGenerator.Next(1, Console.WindowWidth - 1));
            }
            while (snakeElements.Contains(food));

            //Начин да определим посоката
            int right = 0;
            int left = 1;
            int down = 2;
            int up = 3;

            //Възможни посоки, по които да тръгне змията
            Coordinates[] directions =
            {
                new Coordinates(0,1),
                new Coordinates(0,-1),
                new Coordinates(1,0),
                new Coordinates(-1,0)
            };

            //Начална посока
            //По default змията тръгва надясно, заради разположението на конзолата
            int currentDirection = right;

            //Отпечатване на змията
            foreach (Coordinates item in snakeElements)
            {
                Console.SetCursorPosition(item.col, item.row);
                Console.Write("*");
            }

            while (true)
            {
                //Проверка за клавиш: 
                if (Console.KeyAvailable)
                {
                    //Запазваме стойността на клавиша в променлива
                    ConsoleKeyInfo userKeyInput = Console.ReadKey();

                    //Проверяваме кой клавиш е натиснат
                    if (userKeyInput.Key == ConsoleKey.RightArrow)
                    {
                        //Сменяме посоката, само ако не е противоположна на текущата
                        //В противен случай змията ще умре
                        if (currentDirection != left)
                            currentDirection = right;
                    }
                    //Аналогично за другите проверки...
                    else if (userKeyInput.Key == ConsoleKey.LeftArrow)
                    {
                        if (currentDirection != right)
                            currentDirection = left;
                    }
                    else if (userKeyInput.Key == ConsoleKey.DownArrow)
                    {
                        if (currentDirection != up)
                            currentDirection = down;
                    }
                    else if (userKeyInput.Key == ConsoleKey.UpArrow)
                    {
                        if (currentDirection != down)
                            currentDirection = up;
                    }
                }

                //Взимаме текущите координати на змията
                Coordinates snakeCurrentHead = snakeElements.Last();

                //Смятаме координатите на "завъртане"
                Coordinates nextDirection = directions[currentDirection];

                //Смятаме координатите на новата "глава"
                Coordinates snakeNewHead = new Coordinates(
                    snakeCurrentHead.row + nextDirection.row,
                    snakeCurrentHead.col + nextDirection.col);

                //Проверяваме за съвпадение на стари с нови координати
                //т.е. дали сме се самоубили
                if (snakeElements.Contains(snakeNewHead))
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine("GAME OVER!" + "\n" + "Your points are {0}!",
                        (snakeElements.Count - 5) * 100);

                    Console.SetCursorPosition(10, 11);
                    Console.WriteLine("Press [SPACE] for new game...");

                    Console.SetCursorPosition(0, Console.WindowHeight - 1);
                    ConsoleKeyInfo userKeyInput = Console.ReadKey();

                    //Ако потребителят иска рестарт на играта: 
                    if (userKeyInput.Key == ConsoleKey.Spacebar)
                    {
                        Console.ResetColor();
                        Console.Clear();
                        goto GAME;
                    }
                    //Иначе...
                    else
                    {
                        Console.ResetColor();

                        Console.SetCursorPosition(0, Console.WindowHeight - 1);
                        return;
                    }
                }

                //Проверяваме дали змията е изяла храната
                if (snakeNewHead.row == food.row && snakeNewHead.col == food.col)
                {
                    Console.SetCursorPosition(food.col, food.row);
                    Console.Write(" ");

                    do
                    {
                        food = new Coordinates(
                            randomNumberGenerator.Next(1, Console.WindowHeight - 1),
                            randomNumberGenerator.Next(1, Console.WindowWidth - 1));
                    }
                    while (snakeElements.Contains(food));
                }
                else
                {
                    //Изтриваме последният елемент, заради auto-incremention на змията
                    Coordinates lastElement = snakeElements.Dequeue();
                    Console.SetCursorPosition(lastElement.col, lastElement.row);
                    Console.Write(" ");
                }

                //Бъг фиксер за новата глава на змията
                if (snakeNewHead.row < 0)
                    snakeNewHead.row = Console.WindowHeight - 2;
                else if (snakeNewHead.row >= Console.WindowHeight - 1)
                    snakeNewHead.row = 0;
                else if (snakeNewHead.col < 0)
                    snakeNewHead.col = Console.WindowWidth - 2;
                else if (snakeNewHead.col >= Console.WindowWidth - 1)
                    snakeNewHead.col = 0;

                //Промяна на старата глава, към тяло
                Console.SetCursorPosition(snakeCurrentHead.col, snakeCurrentHead.row);
                Console.WriteLine("*");

                //Фиксиране на главата
                snakeElements.Enqueue(snakeNewHead);
                Console.SetCursorPosition(snakeNewHead.col, snakeNewHead.row);
                if (currentDirection == right)
                    Console.Write("*");
                else if (currentDirection == left)
                    Console.Write("*");
                else if (currentDirection == down)
                    Console.Write("*");
                else if (currentDirection == up)
                    Console.Write("*");

                //Принтиране на храната
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(food.col, food.row);
                Console.Write("o");
                Console.ResetColor();

                //Забавяне
                speedIncreaser = speedIncreaser - 0.025;
                Thread.Sleep((int)speedIncreaser);
            }
        }
    }
}
