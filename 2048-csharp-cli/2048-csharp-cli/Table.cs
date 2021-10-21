using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Win32;

namespace _2048_csharp_cli {
    public class Table {
        public int[,] Values { get; set; }
        public int Score { get; set; }
        public Table() {
            if (!Directory.Exists("saves")) { Directory.CreateDirectory("saves"); }
            int rowCount = 0;
            int colCount = 0;
            bool valid = false;
            while(rowCount<4 || rowCount>8 || !valid) {
                Console.Clear();
                Console.Write("Please set the size of the play area!\nHeight (4-8): ");
                valid = int.TryParse(Console.ReadLine(), out rowCount);
            }
            valid = false;
            while (colCount<4 || colCount>8 || !valid) {
                Console.Clear();
                Console.Write("Please set the size of the play area!\nWidth (4-8): ");
                valid = int.TryParse(Console.ReadLine(), out colCount);
            }
            Console.Clear();
            Values = new int[rowCount,colCount];
            Score = 0;
            Summon(2);
        }

        public int MaxRow => Values.GetUpperBound(0);
        public int MaxCol => Values.GetUpperBound(1);
        public int MaxValueLength => Values.Cast<int>().Max().ToString().Length;

        public void Draw() {
            Console.Title = $"2048 - Score: {Score}";
            for(int row=0; row <= MaxRow; row++) {
                for(int col=0; col <= MaxCol; col++) {
                    Console.ForegroundColor = Color(Values[row,col]);
                    Console.Write($" {Values[row,col].ToString().PadRight(MaxValueLength)} ");
                }
                Console.Write("\n\n");
            }
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("CONTROLS:\n- Push: Arrow keys\n- New game: R\n- Load: L\n- Save: S\n- Exit: Esc");
        }
        public void Summon(int quantity) {
            Random r = new Random();
            for(int a=0; a<quantity; a++) {
                if (Values.Cast<int>().Count(s => s.Equals(0)) > 0) {
                    List<int[]> possibleIndexes = new List<int[]>();
                    for (int row=0; row <= MaxRow; row++)  {
                        for (int col=0; col <= MaxCol; col++) {
                            if (Values[row,col] == 0) {
                                possibleIndexes.Add(new int[] { row, col });
                            }
                        }
                    }
                    int[] newIndex = possibleIndexes[r.Next(0, possibleIndexes.Count())];
                    Values[newIndex[0], newIndex[1]] = 2;
                }
            }
        }

        #region merge
        public void MergeDown() {
            bool merges = Push("down",3);
            for(int row=MaxRow-1; row>=0; row--) {
                for(int col=0; col<=MaxCol; col++) {
                    int neighb = Values[row+1,col];
                    int current = Values[row,col];
                    if(neighb == current) {
                        Values[row+1,col] += current;
                        Values[row,col] = 0;
                        _ = Push("down",1);
                        merges = true;
                        Score += neighb + current;
                    }
                }
            }
            if(merges) {Summon(1);}
        }

        public void MergeUp() {
            bool merges = Push("up",3);
            for(int row=MaxRow; row>0; row--) {
                for(int col=0; col<=MaxCol; col++) {
                    int neighb = Values[row-1,col];
                    int current = Values[row,col];
                    if(neighb == current) {
                        Values[row-1,col] += current;
                        Values[row,col] = 0;
                        _ = Push("up",1);
                        merges = true;
                        Score += neighb + current;
                    }
                }
            }
            if (merges) {Summon(1);}
        }

        public void MergeLeft() {
            bool merges = Push("left",3);
            for(int row=0; row<=MaxRow; row++) {
                for(int col=MaxCol; col>0; col--) {
                    int neighb = Values[row,col-1];
                    int current = Values[row,col];
                    if(neighb == current) {
                        Values[row,col-1] += current;
                        Values[row,col] = 0;
                        _ = Push("left",1);
                        merges = true;
                        Score += neighb + current;
                    }
                }
            }
            if (merges) {Summon(1);}
        }

        public void MergeRight() {
            bool merges = Push("right",3);
            for (int row=0; row<=MaxRow; row++) {
                for (int col=MaxCol-1; col>=0; col--) {
                    int neighb = Values[row,col+1];
                    int current = Values[row,col];
                    if(neighb == current) {
                        Values[row,col+1] += current;
                        Values[row,col] = 0;
                        _ = Push("right",1);
                        merges = true;
                        Score += neighb + current;
                    }
                }
            }
            if (merges) {Summon(1);}
        }
        #endregion

        public bool GameOver() {
            bool emptyTileAvailable = Values.Cast<int>().Count(s => s.Equals(0)) > 0; // check if there's at least 1 empty tile
            int merges = 0;
            for (int row=0; row<=MaxRow; row++) {
                for (int col=0; col<=MaxCol; col++) {
                    int value = Values[row,col];
                    // 1. column neighbours check (if row index is on the edge, only test neighbouring tiles inside the bounds, else both directions)
                    switch (row) {
                        case 0: merges += Values[row+1,col]==value ? 1 : 0; break;
                        case 3: merges += Values[row-1,col]==value ? 1 : 0; break;
                        default:
                            merges += Values[row+1,col]==value ? 1 : 0;
                            merges += Values[row-1,col]==value ? 1 : 0;
                            break;
                    }
                    // 2. row neighbours check (if column index is on the edge, only test neighbouring tiles inside the bounds, else both directions)
                    switch (col) {
                        case 0: merges += Values[row,col+1]==value ? 1 : 0; break;
                        case 3: merges += Values[row,col-1]==value ? 1 : 0; break;
                        default:
                            merges += Values[row,col+1]==value ? 1 : 0;
                            merges += Values[row,col-1]==value ? 1 : 0;
                            break;
                    }
                }
            }
            return merges==0 && !emptyTileAvailable; // game over = true IF no possible merges left and no empty tiles available
        }
        public bool Push(string direction, int count) {
            bool merges = false;
            for(int a=0; a<count; a++) {
                for (int row=(direction=="up" ? 1 : 0);   row<=MaxRow-(direction=="down" ? 1 : 0);   row++) {
                    for (int col=(direction=="left" ? 1 : 0);   col<=MaxCol-(direction=="right" ? 1 : 0);   col++) {
                        int[] neighbour = new int[2];
                        switch(direction) {
                            case "up":    neighbour = new int[2] { row-1,col }; break;
                            case "down":  neighbour = new int[2] { row+1,col }; break;
                            case "left":  neighbour = new int[2] { row,col-1 }; break;
                            case "right": neighbour = new int[2] { row,col+1 }; break;
                        }
                        if(Values[neighbour[0],neighbour[1]] == 0) {
                            Values[neighbour[0],neighbour[1]] += Values[row,col];
                            Values[row,col] = 0;
                            merges = true;
                        }
                    }
                }
            }
            return merges;
        }
        public ConsoleColor Color(int value) {
            switch(value) {
                case 0:     return ConsoleColor.Black;
                case 2:     return ConsoleColor.Yellow;
                case 4:     return ConsoleColor.DarkYellow;
                case 8:     return ConsoleColor.DarkRed;
                case 16:    return ConsoleColor.Red;
                case 32:    return ConsoleColor.Magenta;
                case 64:    return ConsoleColor.DarkMagenta;
                case 128:   return ConsoleColor.DarkCyan;
                case 256:   return ConsoleColor.Cyan;
                case 512:   return ConsoleColor.Blue;
                case 1024:  return ConsoleColor.DarkBlue;
                case 2048:  return ConsoleColor.DarkGreen;
                case 4096:  return ConsoleColor.Green;
                case 8192:  return ConsoleColor.Gray;
                case 16384: return ConsoleColor.DarkGray;
                default:    return ConsoleColor.White;
            }
        }
        public void Save() {
            // Pick game
            Console.Clear();
            Console.ForegroundColor = Color(-1);
            Console.Write("Save your progress (leave empty to cancel):\nName: ");
            string fileName = Console.ReadLine();
            Console.Clear();
            // Save
            if (fileName != "")  {
                using(StreamWriter wr = new StreamWriter($@"saves\{fileName}.2s")) {
                    wr.WriteLine(Score);
                    for(int row = 0; row <= MaxRow; row++) {
                        List<int> rowValues = new List<int>();      // working row by row, collect their values joined together with ";" and write them out into a file
                        for(int col = 0; col <= MaxCol; col++) {
                            rowValues.Add(Values[row,col]);
                        }
                        wr.WriteLine(string.Join(";",rowValues.ToArray()));
                    }
                }
                Console.Write($"Game successfully saved as [{fileName}.2s].\nPress any key to continue...");
                Console.ReadKey();
            }
            else {
                Console.Write($"Something went wrong.\nPress any key to continue...");
                Console.ReadKey();
            }

        }
        public void Load() {
            // Pick an existing save name
            Console.Clear();
            Console.ForegroundColor = Color(-1);
            foreach(string existingSave in Directory.GetFiles(@"saves").Where(s => s.EndsWith(".2s"))) {
                Console.Write($"{existingSave.Substring(6).Replace(".2s","")}\n");
            }
            Console.Write("\nLoad your progress (leave empty to cancel):\nName: ");
            string fileName = Console.ReadLine();
            List<string> file = File.Exists($@"saves\{fileName}.2s") ? File.ReadAllLines($@"saves\{fileName}.2s").ToList() : new List<string>();
            // Load
            if (fileName != "" && string.Join("",file).Length > 0 && file.Count()-1 >= 4 && file.Count()-1 <= 8) {
                int openedScore = 0;
                bool valid = int.TryParse(file[0], out openedScore);
                file.RemoveAt(0);
                int[,] ValuesOpened = new int[file.Count(),file.First().Split(';').Count()];

                for(byte row = 0; row <= ValuesOpened.GetUpperBound(0); row++) {
                    for (byte col = 0; col <= ValuesOpened.GetUpperBound(1) && col < file[row].Split(';').Count(); col++) {
                        double currentValue = 0;
                        valid = double.TryParse(file[row].Split(';')[col], out currentValue) && valid;   // try to parse each read item, and if it's possible, 
                        while (currentValue > 2) {                                                       // check if it's a possible game number by halving with 2 until it reaches 2.
                            currentValue /= 2;
                        }
                        valid = (currentValue == 2 || currentValue == 0) && valid;                       // setting the valid boolean to true only if it was true before,
                        ValuesOpened[row, col] = valid ? int.Parse(file[row].Split(';')[col]) : 0;       // to avoid exploits, and set it to false at any incorrect data
                    }
                }

                Values = valid ? ValuesOpened : Values;               // set the tiles to the new one if the opened file is valid, else keep the current one
                Score = valid ? openedScore : Score;                  // set the score to the new one -||-
            }
        }
    }
}
