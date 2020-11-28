using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Win32;

namespace _2048_csharp_cli {
    class Program {
        static void Main(string[] args) {
            ConsoleKey key;
            Table table = new Table();
            do {
                Console.Clear();
                table.Draw();
                key = Console.ReadKey().Key;
                switch(key) {
                    case ConsoleKey.R: table = new Table(); break;
                    case ConsoleKey.LeftArrow: table.MergeLeft(); break;
                    case ConsoleKey.RightArrow: table.MergeRight(); break;
                    case ConsoleKey.UpArrow: table.MergeUp(); break;
                    case ConsoleKey.DownArrow: table.MergeDown(); break;
                    case ConsoleKey.L: table.Load(); break;
                    case ConsoleKey.S: table.Save(); break;
                    default:  break;
                }
            }
            while(key != ConsoleKey.Escape);
        }
    }
}
