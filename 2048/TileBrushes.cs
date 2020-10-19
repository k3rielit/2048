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

namespace _2048 {
    public class TileBrushes {
        public SolidColorBrush c2 => new SolidColorBrush(Color.FromArgb(255, 255, 252, 235));
        public SolidColorBrush c4 => new SolidColorBrush(Color.FromArgb(255, 255, 246, 196));
        public SolidColorBrush c8 => new SolidColorBrush(Color.FromArgb(255, 255, 238, 143));
        public SolidColorBrush c16 => new SolidColorBrush(Color.FromArgb(255, 255, 229, 84));
        public SolidColorBrush c32 => new SolidColorBrush(Color.FromArgb(255, 255, 217, 3));
        public SolidColorBrush c64 => new SolidColorBrush(Color.FromArgb(255, 255, 163, 3));
        public SolidColorBrush c128 => new SolidColorBrush(Color.FromArgb(255, 255, 112, 3));
        public SolidColorBrush c256 => new SolidColorBrush(Color.FromArgb(255, 255, 45, 3));
        public SolidColorBrush c512 => new SolidColorBrush(Color.FromArgb(255, 255, 3, 79));
        public SolidColorBrush c1024 => new SolidColorBrush(Color.FromArgb(255, 255, 3, 217));
        public SolidColorBrush c2048 => new SolidColorBrush(Color.FromArgb(255, 171, 3, 255));
        public SolidColorBrush c4096 => new SolidColorBrush(Color.FromArgb(255, 95, 3, 255));
        public SolidColorBrush c8192 => new SolidColorBrush(Color.FromArgb(255, 3, 7, 255));
        public SolidColorBrush c16384 => new SolidColorBrush(Color.FromArgb(255, 3, 133, 255));
        public SolidColorBrush c32768 => new SolidColorBrush(Color.FromArgb(255, 3, 200, 255));
        public SolidColorBrush c65536 => new SolidColorBrush(Color.FromArgb(255, 3, 255, 247));
        public SolidColorBrush def => new SolidColorBrush(Color.FromArgb(255, 240, 240, 240));
    }
}
