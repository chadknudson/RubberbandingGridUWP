using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.Foundation;
using Windows.UI;
using System.Collections.Generic;

namespace RubberBandingGridUWP
{
    public sealed partial class MainPage : Page
    {
        private List<Line> _gridLines = new List<Line>();
        private Rectangle _boundingBox = null;
        private Point _startPoint;

        // Configurable grid size.
        public int GridRows { get; set; } = 5;
        public int GridColumns { get; set; } = 5;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _startPoint = e.GetCurrentPoint(Canvas).Position;
        }

        private void Canvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.IsInContact)
            {
                var currentPoint = e.GetCurrentPoint(Canvas).Position;

                var width = currentPoint.X - _startPoint.X;
                var height = currentPoint.Y - _startPoint.Y;

                ClearGridLinesAndBoundingBox();
                DrawGridInRectangle(_startPoint, width, height);
                DrawBoundingBox(_startPoint, width, height);
            }
        }

        private void Canvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            _gridLines.Clear();
        }

        private void ClearGridLinesAndBoundingBox()
        {
            foreach (var line in _gridLines)
            {
                Canvas.Children.Remove(line);
            }
            _gridLines.Clear();

            if (_boundingBox != null)
            {
                Canvas.Children.Remove(_boundingBox);
                _boundingBox = null;
            }
        }

        private void DrawBoundingBox(Point startPoint, double width, double height)
        {
            _boundingBox = new Rectangle
            {
                Stroke = new SolidColorBrush(Colors.Red),
                StrokeThickness = 2,
                Width = width,
                Height = height
            };

            _boundingBox.SetValue(Canvas.LeftProperty, startPoint.X);
            _boundingBox.SetValue(Canvas.TopProperty, startPoint.Y);

            Canvas.Children.Add(_boundingBox);
        }

        private void DrawGridInRectangle(Point startPoint, double width, double height)
        {
            double columnSpacing = width / GridColumns;
            double rowSpacing = height / GridRows;

            for (var i = startPoint.X + columnSpacing; i < startPoint.X + width; i += columnSpacing)
            {
                var line = new Line
                {
                    X1 = i,
                    Y1 = startPoint.Y,
                    X2 = i,
                    Y2 = startPoint.Y + height,
                    Stroke = new SolidColorBrush(Colors.Blue),
                    StrokeThickness = 1
                };
                Canvas.Children.Add(line);
                _gridLines.Add(line);
            }

            for (var i = startPoint.Y + rowSpacing; i < startPoint.Y + height; i += rowSpacing)
            {
                var line = new Line
                {
                    X1 = startPoint.X,
                    Y1 = i,
                    X2 = startPoint.X + width,
                    Y2 = i,
                    Stroke = new SolidColorBrush(Colors.Blue),
                    StrokeThickness = 1
                };
                Canvas.Children.Add(line);
                _gridLines.Add(line);
            }
        }
        

        private void SetGridSize_Clicked(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(RowsInput.Text, out int rows))
            {
                GridRows = rows;
            }
            if (int.TryParse(ColumnsInput.Text, out int columns))
            {
                GridColumns = columns;
            }
        }

        private void ClearCanvas_Clicked(object sender, RoutedEventArgs e)
        {
            ClearGridLinesAndBoundingBox();
            Canvas.Children.Clear();
        }
    }
}
