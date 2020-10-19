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
            
        }

        // set for later
        public Random r = new Random();
        public int[,] tileValues = new int[4,4];
        public TileBrushes colors = new TileBrushes();
        private void Window_KeyUp(object sender, KeyEventArgs e) {
            // Main game cycle
            while (!GameOver()) {
                switch (e.Key) {
                    case Key.Up: MoveUp(); break;
                    case Key.Down: MoveDown(); break;
                    case Key.Left: MoveLeft(); break;      // ??? add restart key (R)
                    case Key.Right: MoveRight(); break;
                    default: break;
                }
                SummonTile();
                DrawTiles();
            }
            MessageBox.Show("Game over! No more possible moves!");
            tileValues = new int[4,4];
        }
        private bool GameOver() {
            // check for 2 same tiles next to each other OR check if there's at least 1 empty tile !!!
            // Check neighbouring tiles with same value: (tile1index - tile2index-- > if 1 || -1, neighbours) < -- do horizontally and vertically too (tile[one direction,other direction])
            bool isGameOver = false;
            // maybe Linq group tiles by their values, then if there's 2 or more of any, check if they are neighbours
            return isGameOver;
        }
        private void SummonTile() {
            // summon if possible, else do nothing (game over has already been tested, it doesn't matter) <-- possibility: there's at least 1 possible merging step, but 0 empty tiles
            bool summonPossible = tileValues.Cast<int>().Count(s => s.Equals(0)) > 0 ? true : false;
            if(summonPossible) {
                // collect possible titleValue indexes (Linq select every 0 value one) into List
                // random select one from list
            }
        }
        private void DrawTiles() {
            for(int a=0; a<4; a++) {
                for(int b = 0; b < 4; b++) {
                    // set number of the tile
                    // switch for tile color, from the tile value
                }
            }
        }
        private void Window_KeyDown(object sender, KeyEventArgs e) { } // leftover code, maybe useful later
        //   cell0row0col.Background = colors.c2;   (example)
        //   cell0row0colLBL.Content = "4";         (example)
        private void MoveUp() {
            
        }
        private void MoveDown() {
            
        }
        private void MoveLeft() {

        }
        private void MoveRight() {

        }
    }
}
