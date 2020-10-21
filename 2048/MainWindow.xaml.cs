using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _2048
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() {
            InitializeComponent();
            // initialize a single 2 into the field then draw it out !!!
            StartGame();
        }

        // set for later
        public Random r = new Random();
        public int[,] tileValues = new int[4,4];
        public int score = 0;
        private void Window_KeyUp(object sender, KeyEventArgs e) {
            // Main game cycle
            if (!GameOver()) {
                switch (e.Key) {
                    case Key.Up: MergeUp(); break;
                    case Key.Down: MergeDown(); break;
                    case Key.Left: MergeLeft(); break;      // ??? add restart key (R)
                    case Key.Right: MergeRight(); break;
                    default: break;
                }
                this.Title = "WPF 2048 - Score: "+score;
            }
            else {
                MessageBox.Show("Game over! No more possible moves!");
                StartGame();
            }
        }
        private void StartGame() {
            tileValues = new int[4,4];
            score = 0;
            SummonTile();
            SummonTile();
            DrawTiles();
        }

        #region pushing
        private void MergeDown() {
            bool merges = Push("down",3);
            for(int row=2; row>=0; row--) {
                for(int col=0; col<4; col++) {
                    int neighb = tileValues[row+1,col];
                    int current = tileValues[row,col];
                    if(neighb == current) {
                        tileValues[row+1,col] += current;
                        tileValues[row,col] = 0;
                        _ = Push("down",1);
                        merges = true;
                        score += neighb + current;
                    }
                }
            }
            // only count as a step if tiles could be merged
            if(merges) {SummonTile();}
            DrawTiles();
        }

        private void MergeUp() {
            bool merges = Push("up",3);
            for(int row=3; row>0; row--) {
                for(int col=0; col<4; col++) {
                    int neighb = tileValues[row-1,col];
                    int current = tileValues[row,col];
                    if(neighb == current) {
                        tileValues[row-1,col] += current;
                        tileValues[row,col] = 0;
                        _ = Push("up",1);
                        merges = true;
                        score += neighb + current;
                    }
                }
            }
            // only count as a step if tiles could be merged
            if (merges) {SummonTile();}
            DrawTiles();
        }

        private void MergeLeft() {
            bool merges = Push("left",3);
            for(int row=0; row<4; row++) {
                for(int col=3; col>0; col--) {
                    int neighb = tileValues[row,col-1];
                    int current = tileValues[row,col];
                    if(neighb == current) {
                        tileValues[row,col-1] += current;
                        tileValues[row,col] = 0;
                        _ = Push("left",1);
                        merges = true;
                        score += neighb + current;
                    }
                }
            }
            // only count as a step if tiles could be merged
            if (merges) {SummonTile();}
            DrawTiles();
        }

        private void MergeRight() {
            bool merges = Push("right",3);
            for (int row=0; row<4; row++) {
                for (int col=2; col>=0; col--) {
                    int neighb = tileValues[row,col+1];
                    int current = tileValues[row,col];
                    if(neighb == current) {
                        tileValues[row,col+1] += current;
                        tileValues[row,col] = 0;
                        _ = Push("right",1);
                        merges = true;
                        score += neighb + current;
                    }
                }
            }
            // only count as a step if tiles could be merged
            if (merges) {SummonTile();}
            DrawTiles();
        }
        #endregion

        private bool GameOver() {
            bool emptyTileAvailable = tileValues.Cast<int>().Count(s => s.Equals(0)) > 0; // check if there's at least 1 empty tile
            // POSSIBLE MERGES:
            int merges = 0;
            for (int row=0; row<4; row++) {
                for (int col=0; col<4; col++) {
                    int value = tileValues[row,col];
                    // 1. column neighbours check (if row index is on the edge, only test neighbouring tiles inside the bounds, else both directions)
                    switch (row) {
                        case 0: merges += tileValues[row+1,col]==value ? 1 : 0; break;
                        case 3: merges += tileValues[row-1,col]==value ? 1 : 0; break;
                        default:
                            merges += tileValues[row+1,col]==value ? 1 : 0;
                            merges += tileValues[row-1,col]==value ? 1 : 0;
                            break;
                    }
                    // 2. row neighbours check (if column index is on the edge, only test neighbouring tiles inside the bounds, else both directions)
                    switch (col) {
                        case 0: merges += tileValues[row,col+1]==value ? 1 : 0; break;
                        case 3: merges += tileValues[row,col-1]==value ? 1 : 0; break;
                        default:
                            merges += tileValues[row,col+1]==value ? 1 : 0;
                            merges += tileValues[row,col-1]==value ? 1 : 0;
                            break;
                    }
                }
            }
            // game over = true IF no possible merges left and no empty tiles available
            return merges==0 && !emptyTileAvailable;
        }
        private void SummonTile() {
            // summon if possible, else do nothing
            if(tileValues.Cast<int>().Count(s => s.Equals(0)) > 0) {
                // generate possible indexes (tiles with 0)
                List<int[]> possibleIndex = new List<int[]>();
                for (int a=0; a<4; a++) {
                    for (int b=0; b<4; b++) {
                        if(tileValues[a,b] == 0) {
                            possibleIndex.Add(new int[]{a,b});
                        }
                    }
                }
                int[] newIndex = possibleIndex[r.Next(0,possibleIndex.Count())]; // random select one tile index from list
                tileValues[newIndex[0],newIndex[1]] = 2; // set the correct tile 2 according to the generated index
            }
        }
        private bool Push(string direction, int count) {
            bool merges = false;
            for(int a=0; a<count; a++) {
                switch(direction) {
                    case "up":
                        for (int row=1; row<4; row++) {
                            for (int col=0; col<4; col++) {
                                if (tileValues[row-1,col] == 0) {
                                    tileValues[row-1,col] += tileValues[row,col];
                                    tileValues[row,col] = 0;
                                    merges = true;
                                }
                            }
                        }
                        break;
                    case "down":
                        for (int row=0; row<3; row++) {
                            for (int col=0; col<4; col++) {
                                if (tileValues[row+1,col] == 0) {
                                    tileValues[row+1,col] += tileValues[row,col];
                                    tileValues[row,col] = 0;
                                    merges = true;
                                }
                            }
                        }
                        break;
                    case "left":
                        for (int row=0; row<4; row++) {
                            for (int col=1; col<4; col++) {
                                if (tileValues[row,col-1] == 0) {
                                    tileValues[row,col-1] += tileValues[row,col];
                                    tileValues[row,col] = 0;
                                    merges = true;
                                }
                            }
                        }
                        break;
                    case "right":
                        for (int row=0; row<4; row++) {
                            for (int col=0; col<3; col++) {
                                if (tileValues[row,col+1] == 0) {
                                    tileValues[row,col+1] += tileValues[row,col];
                                    tileValues[row,col] = 0;
                                    merges = true;
                                }
                            }
                        }
                        break;
                }
            }
            return merges;
        }
        private void DrawTiles() {
            // ugly junk for styling and setting content of the GUI tiles
            // row 0
            row0col0_lab.Content = tileValues[0,0]==0 ? "" : tileValues[0,0].ToString();
            row0col0_wrap.Background = GetTileColor(tileValues[0,0]);
            row0col1_lab.Content = tileValues[0,1]==0 ? "" : tileValues[0,1].ToString();
            row0col1_wrap.Background = GetTileColor(tileValues[0,1]);
            row0col2_lab.Content = tileValues[0,2]==0 ? "" : tileValues[0,2].ToString();
            row0col2_wrap.Background = GetTileColor(tileValues[0,2]);
            row0col3_lab.Content = tileValues[0,3]==0 ? "" : tileValues[0,3].ToString();
            row0col3_wrap.Background = GetTileColor(tileValues[0,3]);
            // row 1
            row1col0_lab.Content = tileValues[1,0]==0 ? "" : tileValues[1,0].ToString();
            row1col0_wrap.Background = GetTileColor(tileValues[1,0]);
            row1col1_lab.Content = tileValues[1,1]==0 ? "" : tileValues[1,1].ToString();
            row1col1_wrap.Background = GetTileColor(tileValues[1,1]);
            row1col2_lab.Content = tileValues[1,2]==0 ? "" : tileValues[1,2].ToString();
            row1col2_wrap.Background = GetTileColor(tileValues[1,2]);
            row1col3_lab.Content = tileValues[1,3]==0 ? "" : tileValues[1,3].ToString();
            row1col3_wrap.Background = GetTileColor(tileValues[1,3]);
            // row 2
            row2col0_lab.Content = tileValues[2,0]==0 ? "" : tileValues[2,0].ToString();
            row2col0_wrap.Background = GetTileColor(tileValues[2,0]);
            row2col1_lab.Content = tileValues[2,1]==0 ? "" : tileValues[2,1].ToString();
            row2col1_wrap.Background = GetTileColor(tileValues[2,1]);
            row2col2_lab.Content = tileValues[2,2]==0 ? "" : tileValues[2,2].ToString();
            row2col2_wrap.Background = GetTileColor(tileValues[2,2]);
            row2col3_lab.Content = tileValues[2,3]==0 ? "" : tileValues[2,3].ToString();
            row2col3_wrap.Background = GetTileColor(tileValues[2,3]);
            // row 3
            row3col0_lab.Content = tileValues[3,0]==0 ? "" : tileValues[3,0].ToString();
            row3col0_wrap.Background = GetTileColor(tileValues[3,0]);
            row3col1_lab.Content = tileValues[3,1]==0 ? "" : tileValues[3,1].ToString();
            row3col1_wrap.Background = GetTileColor(tileValues[3,1]);
            row3col2_lab.Content = tileValues[3,2]==0 ? "" : tileValues[3,2].ToString();
            row3col2_wrap.Background = GetTileColor(tileValues[3,2]);
            row3col3_lab.Content = tileValues[3,3]==0 ? "" : tileValues[3,3].ToString();
            row3col3_wrap.Background = GetTileColor(tileValues[3,3]);
        }

        private SolidColorBrush GetTileColor(int value) {
            // specify colors for numbers
            switch (value) {
                case 2: return new SolidColorBrush(Color.FromArgb(255, 255, 252, 235));
                case 4: return new SolidColorBrush(Color.FromArgb(255, 255, 246, 196));
                case 8: return new SolidColorBrush(Color.FromArgb(255, 255, 238, 143));
                case 16: return new SolidColorBrush(Color.FromArgb(255, 255, 229, 84));
                case 32: return new SolidColorBrush(Color.FromArgb(255, 255, 217, 3));
                case 64: return new SolidColorBrush(Color.FromArgb(255, 255, 163, 3));
                case 128: return new SolidColorBrush(Color.FromArgb(255, 255, 112, 3));
                case 256: return new SolidColorBrush(Color.FromArgb(255, 255, 45, 3));
                case 512: return new SolidColorBrush(Color.FromArgb(255, 255, 3, 79));
                case 1024: return new SolidColorBrush(Color.FromArgb(255, 255, 3, 217));
                case 2048: return new SolidColorBrush(Color.FromArgb(255, 171, 3, 255));
                case 4096: return new SolidColorBrush(Color.FromArgb(255, 95, 3, 255));
                case 8192: return new SolidColorBrush(Color.FromArgb(255, 3, 7, 255));
                case 16384: return new SolidColorBrush(Color.FromArgb(255, 3, 133, 255));
                case 32768: return new SolidColorBrush(Color.FromArgb(255, 3, 200, 255));
                case 65536: return new SolidColorBrush(Color.FromArgb(255, 3, 255, 247));
                default: return new SolidColorBrush(Color.FromArgb(255, 112, 97, 69));
            }
        }
    }
}
