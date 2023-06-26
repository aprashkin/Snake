using System.Drawing;

namespace Snake
{
    internal class Program
    {
        public enum Direction { Stop, Up, Down, Left, Right };
        private static Size _size;
        private static Point _head;
        private static Point _fruit;
        private static bool _gameOver;
        private static List<Point> _tail;
        private static Direction _direction;
        private static readonly Random _random = new Random();
        private static int _speed = 3;

        private static void Main()
        {
            Console.Title = "Snake";
            bool gaming = true;
            while (gaming)
            {
                ShowMainMenu();

                var userInput = Console.ReadKey(true).Key;

                switch (userInput)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        Setup();
                        RunGame();
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        ShowRules();
                        break;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        ShowSettings();
                        break;
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        gaming = false;
                        break;
                }
            }
            Environment.Exit(0);
        }



        private static void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Главное меню ===");
            Console.WriteLine("1 - Играть");
            Console.WriteLine("2 - Правила");
            Console.WriteLine("3 - Настройки");
            Console.WriteLine("4 - Выход");
        }

        private static void ShowSettings()
        {
            Console.Clear();
            Console.WriteLine("=== Настройки ===");
            Console.WriteLine("Текущая скорость: " + _speed);
            Console.WriteLine("Выберите новую скорость (1-5):");

            var userInput = Console.ReadKey(true).Key;
            if (userInput >= ConsoleKey.D1 && userInput <= ConsoleKey.D5)
            {
                _speed = userInput - ConsoleKey.D0;
            }
            else if (userInput >= ConsoleKey.NumPad1 && userInput <= ConsoleKey.NumPad5)
            {
                _speed = userInput - ConsoleKey.NumPad0;
            }
        }

        private static void ShowRules()
        {
            Console.Clear();
            Console.WriteLine("=== Правила игры ===");
            Console.WriteLine("Кушайте еду, избегайте столкновения с хвостом");
            Console.WriteLine("Нажмите любую клавишу, чтобы вернуться в главное меню.");
            Console.ReadKey(true);
        }

        private static void RunGame()
        {
            while (_gameOver == false)
            {
                Draw();
                Input();
                Logic();
                Thread.Sleep(200 - _speed * 30);
            }
            End();
        }

        private static void Setup()
        {
            _gameOver = false;
            _size = new Size(40, 20);
            _tail = new List<Point>();
            _direction = Direction.Stop;
            _head = RandomPoint();
            _fruit = RandomPoint();

            Console.Clear();
            Console.CursorVisible = false;
            Console.Title = "Snake Game";

            for (var i = 0; i < _size.Height; i++)
            {
                if (i == 0 || i == _size.Height - 1)
                {
                    Console.SetCursorPosition(0, i);
                    Console.Write($"+{new string('-', _size.Width - 2)}+");
                }
                else
                {
                    Console.SetCursorPosition(0, i);
                    Console.Write($"|{new string(' ', _size.Width - 2)}|");
                }
            }
        }

        private static void Draw()
        {
            for (var i = 1; i < _size.Height - 1; i++)
            {
                Console.SetCursorPosition(1, i);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(new string(' ', _size.Width - 2));
            }

            Console.SetCursorPosition(_head.X, _head.Y);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("0");
            Console.ResetColor();

            foreach (var point in _tail)
            {
                Console.SetCursorPosition(point.X, point.Y);
                Console.Write("o");
            }

            Console.SetCursorPosition(_fruit.X, _fruit.Y);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("*");
            Console.ResetColor();

            Console.SetCursorPosition(_size.Width + 3, 5);
            Console.Write($"Score: {_tail.Count}");
        }

        private static void Input()
        {
            if (!Console.KeyAvailable) { return; }

            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.W || key == ConsoleKey.UpArrow || key == ConsoleKey.NumPad8)
            {
                if (_direction != Direction.Down) { _direction = Direction.Up; }
            }
            else if (key == ConsoleKey.S || key == ConsoleKey.DownArrow || key == ConsoleKey.NumPad2)
            {
                if (_direction != Direction.Up) { _direction = Direction.Down; }
            }
            else if (key == ConsoleKey.A || key == ConsoleKey.LeftArrow || key == ConsoleKey.NumPad4)
            {
                if (_direction != Direction.Right) { _direction = Direction.Left; }
            }
            else if (key == ConsoleKey.D || key == ConsoleKey.RightArrow || key == ConsoleKey.NumPad6)
            {
                if (_direction != Direction.Left) { _direction = Direction.Right; }
            }
            else if (key == ConsoleKey.Escape) { _direction = Direction.Stop; }
        }

        private static void Logic()
        {
            if (_direction == Direction.Stop) { return; }

            if (_tail.Contains(_head))
            {
                _gameOver = true;
                _direction = Direction.Stop;
                return;
            }

            _tail.Add(new Point(_head.X, _head.Y));

            if (_head == _fruit) _fruit = RandomPoint();
            else
            {
                if (_tail.Any())
                {
                    _tail.Remove(_tail.First());
                }
            }

            if (_direction == Direction.Up)
            {
                if (_head.Y - 1 < 1) { _head.Y = _size.Height - 2; }
                else { _head.Y--; }
            }
            else if (_direction == Direction.Down)
            {
                if (_head.Y + 1 > _size.Height - 2) { _head.Y = 1; }
                else { _head.Y++; }
            }
            else if (_direction == Direction.Left)
            {
                if (_head.X - 1 < 1) { _head.X = _size.Width - 2; }
                else { _head.X--; }
            }
            else if (_direction == Direction.Right)
            {
                if (_head.X + 1 > _size.Width - 2) { _head.X = 1; }
                else { _head.X++; }
            }
        }

        private static void End()
        {
            Console.SetCursorPosition(_size.Width + 3, 3);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("GAME OVER");

            Console.SetCursorPosition(_size.Width + 3, 4);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write("Spacebar to play again");

            Console.ResetColor();

            while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) ;
        }

        public static Point RandomPoint()
        {
            var x = _random.Next(1, _size.Width - 1);
            var y = _random.Next(1, _size.Height - 1);
            var point = new Point(x, y);

            if (_tail.Contains(point) || _head == point) { return RandomPoint(); }
            else { return point; }
        }
    }
}