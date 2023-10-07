using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;

namespace DialogueBuilderWpf.ui
{
    /// <summary>
    /// Builds nodeEditor nodes.
    /// </summary>
    internal class NodeEditorNodeBuilder
    {
        const int _BoxHeight = 24;
        const int _BoxWidth = 40;
        const int _LineThickness = 2;
        readonly Brush _LineColor = new SolidColorBrush(Colors.BurlyWood);
        readonly Brush _BoxBgrColor = new SolidColorBrush(Colors.BurlyWood);


        public TextBlock BuildBlock(string UiId)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = UiId;
            textBlock.Name = UiId;
            textBlock.Width = _BoxWidth;
            textBlock.Height = _BoxHeight;
            textBlock.Background = _BoxBgrColor;
            textBlock.Padding = new Thickness(4, 0, 0, 0);
            return textBlock;
        }

        public Line BuildLineToChild(System.Drawing.Point position, System.Drawing.Point childPosition)
        {
            Line line = new Line();
            line.X1 = position.X + _BoxWidth;
            line.Y1 = position.Y + _BoxHeight / 2;
            line.X2 = childPosition.X;
            line.Y2 = childPosition.Y + _BoxHeight / 2;
            line.Stroke = _LineColor;
            line.StrokeThickness = _LineThickness;
            return line;
        }
    }
}
