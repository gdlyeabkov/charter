using MaterialDesignThemes.Wpf;
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

using System.Speech.Synthesis;

namespace Charter
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public Line line;
        public bool isChartSelected = false;
        public Line measureLeft;
        public Line measureRight;
        public double lineWidth = 0;
        public double lineHeight = 0;
        public Point measureLeftStartPosition;
        public Point measureRightStartPosition;
        public bool isAppInit = false;
        public string lineType = "solid";
        public string circleDrawMode = "diameter";
        public Ellipse circle;
        public double lastCircleCoord = 0;
        public double initialCircleCoordX = 0;
        public double initialCircleCoordY = 0;
        public double lastLineWidthCoord = 0;
        public SpeechSynthesizer debugger;
        public double lineThickness = 1;
        public double lineDelta = 0;
        bool isStartCornerStep = true;
        public PolyBezierSegment corner;
        public Path curve;

        public MainWindow()
        {
            InitializeComponent();

            Initialize();

        }

        public void Initialize()
        {
            measureLeftStartPosition = new Point();
            measureRightStartPosition = new Point();
            debugger = new SpeechSynthesizer();
        }

        private void WorkspaceHoverHandler(object sender, MouseEventArgs e)
        {
            WorkspaceHover(sender, e);
            ChartPenMove(e);
        }

        public void WorkspaceHover(object sender, MouseEventArgs e)
        {
            DockPanel workspace = ((DockPanel)(sender));
            Point currentPosition = e.GetPosition(workspace);
            double coordX = currentPosition.X;
            double coordY = currentPosition.Y;
            Canvas.SetLeft(horizontalRuller, coordX);
            Canvas.SetTop(verticalRuller, coordY);
        }

        private void ChartPenDownHandler(object sender, MouseButtonEventArgs e)
        {
            int selectedToolIndex = toolbar.SelectedIndex;
            bool isLineSelectedTool = selectedToolIndex == 0;
            bool isHorizontalMeasureSelectedTool = selectedToolIndex == 1;
            bool isVerticalMeasureSelectedTool = selectedToolIndex == 2;
            bool isHorizontalDirectionSelectedTool = selectedToolIndex == 3;
            bool isVerticalDirectionSelectedTool = selectedToolIndex == 4;
            bool isCircleSelectedTool = selectedToolIndex == 5;
            if (isLineSelectedTool)
            {
                line = new Line();
                Point currentPosition = e.GetPosition(chart);
                double coordX = currentPosition.X;
                double coordY = currentPosition.Y;
                line.X1 = coordX;
                line.Y1 = coordY;
                line.X2 = coordX;
                line.Y2 = coordY;
                line.Stroke = System.Windows.Media.Brushes.LightGray;
                line.StrokeThickness = lineThickness;
                chart.Children.Add(line);
            }
            else if (isHorizontalMeasureSelectedTool)
            {
                line = new Line();
                Point currentPosition = e.GetPosition(chart);
                double coordX = currentPosition.X;
                double coordY = currentPosition.Y;

                bool isChartNotSelected = !isChartSelected;
                if (isChartNotSelected)
                {
                    line.X1 = coordX;
                    line.Y1 = coordY;
                    line.X2 = coordX;
                    line.Y2 = coordY;
                }

                line.StrokeDashArray = new DoubleCollection(
                    new double[] {
                        10
                    }
                );
                line.Stroke = System.Windows.Media.Brushes.LightGray;
                chart.Children.Add(line);

                measureLeftStartPosition = new Point(coordX, coordY);

            }
            else if (isVerticalMeasureSelectedTool)
            {
                line = new Line();
                Point currentPosition = e.GetPosition(chart);
                double coordX = currentPosition.X;
                double coordY = currentPosition.Y;
                bool isChartNotSelected = !isChartSelected;
                if (isChartNotSelected)
                {
                    line.X1 = coordX;
                    line.Y1 = coordY;
                    line.X2 = coordX;
                    line.Y2 = coordY;
                }
                line.StrokeDashArray = new DoubleCollection(
                    new double[] {
                        10
                    }
                );
                line.Stroke = System.Windows.Media.Brushes.LightGray;
                chart.Children.Add(line);

                measureLeftStartPosition = new Point(coordX, coordY);

            }
            else if (isHorizontalDirectionSelectedTool)
            {
                line = new Line();
                Point currentPosition = e.GetPosition(chart);
                double coordY = currentPosition.Y;
                line.Y1 = coordY;
                line.X1 = 0;
                line.Y2 = coordY;
                line.X2 = 5000;
                line.Stroke = System.Windows.Media.Brushes.LightGray;
                chart.Children.Add(line);
            }
            else if (isVerticalDirectionSelectedTool)
            {
                line = new Line();
                Point currentPosition = e.GetPosition(chart);
                double coordX = currentPosition.X;
                double coordY = currentPosition.Y;
                line.X1 = coordX;
                line.Y1 = 0;
                line.X2 = coordX;
                line.Y2 = 5000;
                line.Stroke = System.Windows.Media.Brushes.LightGray;
                chart.Children.Add(line);
            }
            else if (isCircleSelectedTool)
            {
                Point currentPosition = e.GetPosition(chart);
                double coordX = currentPosition.X;
                double coordY = currentPosition.Y;
                circle = new Ellipse();
                Canvas.SetLeft(circle, coordX);
                Canvas.SetTop(circle, coordY);
                circle.Width = 5;
                circle.Height = 5;
                if (circleDrawMode == "radius")
                {
                    Canvas.SetLeft(circle, coordX);
                    Canvas.SetTop(circle, coordY);
                    circle.Width = 0;
                    circle.Height = 0;
                }
                circle.Stroke = System.Windows.Media.Brushes.Black;
                circle.StrokeThickness = lineThickness;
                int lineTypeIndex = lineTypeBox.SelectedIndex;
                if (lineTypeIndex == 1)
                {
                    circle.Stroke = System.Windows.Media.Brushes.LightGray;
                    circle.StrokeDashArray = new DoubleCollection(
                        new double[] {
                            10
                        }
                    );
                }
                else if (lineTypeIndex == 2)
                {
                    circle.Stroke = System.Windows.Media.Brushes.AliceBlue;
                    circle.StrokeDashArray = new DoubleCollection(
                        new double[] {
                            2
                        }
                    );
                }
                chart.Children.Add(circle);
                lastCircleCoord = coordX;
                initialCircleCoordX = coordX;
                initialCircleCoordY = coordY;
            }
        }

        private void ChartPenMove(MouseEventArgs e)
        {
            int selectedToolIndex = toolbar.SelectedIndex;
            bool isLineSelectedTool = selectedToolIndex == 0;
            bool isHorizontalMeasureSelectedTool = selectedToolIndex == 1;
            bool isVerticalMeasureSelectedTool = selectedToolIndex == 2;
            bool isHorizontalDirectionSelectedTool = selectedToolIndex == 3;
            bool isVerticalDirectionSelectedTool = selectedToolIndex == 4;
            bool isCircleSelectedTool = selectedToolIndex == 5;
            MouseButtonState mouseLeftBtnState = e.LeftButton;
            MouseButtonState mouseBtnPressed = MouseButtonState.Pressed;
            bool isMouseBtnPressed = mouseLeftBtnState == mouseBtnPressed;
            bool isLineExists = line != null;
            bool isCircleExists = circle != null;
            bool isCircleMove = isCircleSelectedTool && isCircleExists;
            if (isLineSelectedTool)
            {
                if (isLineExists)
                {
                    if (isMouseBtnPressed)
                    {
                        Point currentPosition = e.GetPosition(chart);
                        double coordX = currentPosition.X;
                        double coordY = currentPosition.Y;
                        var shiftModifier = Keyboard.Modifiers & ModifierKeys.Shift;
                        bool isShiftModifierEnabled = shiftModifier > 0;
                        if (isShiftModifierEnabled)
                        {
                            if (lineDelta != coordX)
                            {
                                line.X2 = coordX;
                                line.Y2 = line.Y1;
                            }
                            else
                            {
                                line.X2 = line.X1;
                                line.Y2 = coordY;
                            }
                        }
                        else
                        {
                            line.X2 = coordX;
                            line.Y2 = coordY;
                        }
                        lineDelta = coordX;
                    }
                }
            }
            else if (isHorizontalMeasureSelectedTool)
            {
                if (isLineExists)
                {
                    bool isMeasureLeftExists = measureLeft != null;
                    bool isMeasureRightExists = measureRight != null;
                    bool isMeasuresExists = isMeasureLeftExists && isMeasureRightExists;
                    bool isDrawMeasureLine = isMeasuresExists && isChartSelected;
                    if (!isChartSelected)
                    {
                        if (isMouseBtnPressed)
                        {
                            Point currentPosition = e.GetPosition(chart);
                            double coordX = currentPosition.X;
                            double coordY = currentPosition.Y;
                            line.X2 = coordX;
                            line.Y2 = measureLeftStartPosition.Y;
                        }
                    }
                    else if (isDrawMeasureLine)
                    {
                        Point currentPosition = e.GetPosition(chart);
                        double coordX = currentPosition.X;
                        double coordY = currentPosition.Y;
                        measureLeft.X2 = line.X1;
                        measureLeft.Y2 = coordY;
                        measureRight.Y2 = coordY;

                        line.Y2 = coordY;
                        line.Y1 = coordY;

                    }
                }
            }
            else if (isVerticalMeasureSelectedTool)
            {
                if (isLineExists)
                {
                    bool isMeasureLeftExists = measureLeft != null;
                    bool isMeasureRightExists = measureRight != null;
                    bool isMeasuresExists = isMeasureLeftExists && isMeasureRightExists;
                    bool isDrawMeasureLine = isMeasuresExists;
                    if (!isChartSelected)
                    {
                        if (isMouseBtnPressed)
                        {
                            Point currentPosition = e.GetPosition(chart);
                            double coordX = currentPosition.X;
                            double coordY = currentPosition.Y;
                            line.X2 = measureLeftStartPosition.X;
                            line.Y2 = coordY;
                        }
                    }
                    else if (isDrawMeasureLine)
                    {
                        Point currentPosition = e.GetPosition(chart);
                        double coordX = currentPosition.X;
                        line.X2 = coordX;
                        line.X1 = coordX;
                        measureLeft.Y2 = line.Y1;
                        measureLeft.X2 = coordX;
                        measureRight.X2 = coordX;
                    }
                }
            }
            else if (isHorizontalDirectionSelectedTool)
            {
                if (isLineExists)
                {
                    if (isMouseBtnPressed)
                    {
                        Point currentPosition = e.GetPosition(chart);
                        double coordY = currentPosition.Y;
                        line.Y1 = coordY;
                        line.Y2 = coordY;
                    }
                }
            }
            else if (isVerticalDirectionSelectedTool)
            {
                if (isMouseBtnPressed)
                {
                    Point currentPosition = e.GetPosition(chart);
                    double coordX = currentPosition.X;
                    line.X1 = coordX;
                    line.X2 = coordX;
                }
            }
            else if (isCircleMove)
            {
                if (isMouseBtnPressed)
                {
                    Point currentPosition = e.GetPosition(chart);
                    double coordX = currentPosition.X;
                    double coordY = currentPosition.Y;
                    double ratio = 1;
                    bool isCoordLT = lastCircleCoord < coordX;
                    bool isCoordGT = lastCircleCoord > coordX;
                    if (isCoordLT)
                    {
                        ratio = -0.5;
                    }
                    else if (isCoordGT)
                    {
                        ratio = 0.5;
                    }
                    try
                    {
                        circle.Width += ratio;
                        circle.Height += ratio;
                    }
                    catch (Exception)
                    {
                        // не могу изменить на отрицательный размер
                    }
                    bool isCircleDrawModeRadius = circleDrawMode == "radius";
                    if (isCircleDrawModeRadius)
                    {
                        double halfWidth = circle.Width / 2;
                        double left = initialCircleCoordX - halfWidth;
                        double halfHeight = circle.Height / 2;
                        double top = initialCircleCoordY - halfHeight;
                        Canvas.SetLeft(circle, left);
                        Canvas.SetTop(circle, top);
                    }
                    lastCircleCoord = coordX;
                }
            }
        }

        private void ChartPenUpHandler(object sender, MouseButtonEventArgs e)
        {
            int selectedToolIndex = toolbar.SelectedIndex;
            bool isLineSelectedTool = selectedToolIndex == 0;
            bool isHorizontalMeasureSelectedTool = selectedToolIndex == 1;
            bool isVerticalMeasureSelectedTool = selectedToolIndex == 2;
            bool isHorizontalDirectionSelectedTool = selectedToolIndex == 3;
            bool isVerticalDirectionSelectedTool = selectedToolIndex == 4;
            bool isCornerSelectedTool = selectedToolIndex == 6;
            bool isLineExists = line != null;
            if (isLineSelectedTool)
            {
                if (isLineExists)
                {
                    int lineTypeIndex = lineTypeBox.SelectedIndex;
                    line.Stroke = System.Windows.Media.Brushes.Black;
                    if (lineTypeIndex == 1)
                    {
                        line.Stroke = System.Windows.Media.Brushes.LightGray;
                        line.StrokeDashArray = new DoubleCollection(
                            new double[] {
                                    10
                            }
                        );
                    }
                    else if (lineTypeIndex == 2)
                    {
                        line.Stroke = System.Windows.Media.Brushes.AliceBlue;
                        line.StrokeDashArray = new DoubleCollection(
                            new double[] {
                                    2
                            }
                        );
                    }
                }
            }
            else if (isHorizontalMeasureSelectedTool)
            {
                if (isLineExists)
                {
                    isChartSelected = !isChartSelected;
                    if (isChartSelected)
                    {
                        measureLeft = new Line();
                        Point currentPosition = e.GetPosition(chart);
                        double coordX = currentPosition.X;
                        double coordY = currentPosition.Y;
                        measureLeft.X1 = measureLeftStartPosition.X;
                        measureLeft.Y1 = measureLeftStartPosition.Y;
                        measureLeft.X2 = coordX;
                        measureLeft.Y2 = coordY + 0;
                        measureLeft.StrokeDashArray = new DoubleCollection(
                            new double[] {
                                10
                            }
                        );
                        measureLeft.Stroke = System.Windows.Media.Brushes.LightGray;
                        chart.Children.Add(measureLeft);
                        measureRight = new Line();
                        currentPosition = e.GetPosition(chart);
                        coordX = currentPosition.X;
                        coordY = currentPosition.Y;
                        measureRight.X1 = coordX;
                        measureRight.Y1 = measureLeft.Y1;
                        measureRight.X2 = coordX;
                        measureRight.Y2 = coordY;
                        measureRight.StrokeDashArray = new DoubleCollection(
                            new double[] {
                                10
                            }
                        );
                        measureRight.Stroke = System.Windows.Media.Brushes.LightGray;
                        chart.Children.Add(measureRight);
                        measureLeftStartPosition = new Point(line.X1, line.Y1);
                        measureRightStartPosition = currentPosition;
                    }
                    else
                    {
                        TextBlock measureLabel = new TextBlock();
                        chart.Children.Add(measureLabel);
                        double leftMeasureCoordX2 = measureLeft.X2;
                        double rightMeasureCoordX2 = measureRight.X2;
                        double measure = rightMeasureCoordX2 - leftMeasureCoordX2;
                        double measureCenter = measure / 2;
                        double coordX = measureCenter + leftMeasureCoordX2;
                        Canvas.SetLeft(measureLabel, coordX);
                        double verticalOffset = 20;
                        double pointCoordY = measureLeft.Y2;
                        double coordY = pointCoordY - verticalOffset;
                        Canvas.SetTop(measureLabel, coordY);
                        bool isMeasureNegative = measure < 0;
                        if (isMeasureNegative)
                        {
                            measure *= -1;
                        }
                        int selectedMeasureIndex = measureBox.SelectedIndex;
                        string selectedMeasure = "мм";
                        if (selectedMeasureIndex == 1)
                        {
                            measure /= 100;
                            selectedMeasure = "cм";
                        }
                        else if (selectedMeasureIndex == 2)
                        {
                            measure /= 1000;
                            selectedMeasure = "дм";
                        }
                        string rawRoundedMeasure = measure.ToString("0.00");
                        string measureLabelContent = rawRoundedMeasure + " " + selectedMeasure;
                        measureLabel.Text = measureLabelContent;
                    }
                }
            }
            else if (isVerticalMeasureSelectedTool)
            {
                if (isLineExists)
                {
                    isChartSelected = !isChartSelected;
                    if (isChartSelected)
                    {
                        measureLeft = new Line();
                        Point currentPosition = e.GetPosition(chart);
                        double coordX = currentPosition.X;
                        double coordY = currentPosition.Y;

                        measureLeft.X1 = measureLeftStartPosition.X;
                        measureLeft.Y1 = measureLeftStartPosition.Y;
                        measureLeft.X2 = coordX;

                        measureLeft.Y2 = coordY + 0;
                        measureLeft.StrokeDashArray = new DoubleCollection(
                            new double[] {
                                10
                            }
                        );
                        measureLeft.Stroke = System.Windows.Media.Brushes.LightGray;
                        chart.Children.Add(measureLeft);
                        measureRight = new Line();
                        currentPosition = e.GetPosition(chart);
                        coordX = currentPosition.X;
                        coordY = currentPosition.Y;
                        measureRight.X1 = measureLeftStartPosition.X;
                        measureRight.Y1 = coordY;
                        measureRight.X2 = coordX;
                        measureRight.Y2 = coordY;
                        measureRight.StrokeDashArray = new DoubleCollection(
                            new double[] {
                                10
                            }
                        );
                        measureRight.Stroke = System.Windows.Media.Brushes.LightGray;
                        chart.Children.Add(measureRight);
                        measureLeftStartPosition = new Point(line.X1, line.Y1);
                        measureRightStartPosition = currentPosition;
                        lineWidth = coordX - line.X1;
                        lineHeight = coordY - line.Y1;
                    }
                    else
                    {
                        TextBlock measureLabel = new TextBlock();
                        chart.Children.Add(measureLabel);
                        double leftMeasureCoordY2 = measureLeft.Y2;
                        double rightMeasureCoordY2 = measureRight.Y2;
                        double measure = rightMeasureCoordY2 - leftMeasureCoordY2;
                        double measureCenter = measure / 2;
                        double coordY = measureCenter + leftMeasureCoordY2;
                        Canvas.SetTop(measureLabel, coordY);
                        double horizontalOffset = 75;
                        double pointCoordX = measureLeft.X2;
                        double coordX = pointCoordX - horizontalOffset;
                        Canvas.SetLeft(measureLabel, coordX);
                        bool isMeasureNegative = measure < 0;
                        if (isMeasureNegative)
                        {
                            measure *= -1;
                        }
                        int selectedMeasureIndex = measureBox.SelectedIndex;
                        string selectedMeasure = "мм";
                        if (selectedMeasureIndex == 1)
                        {
                            measure /= 100;
                            selectedMeasure = "cм";
                        }
                        else if (selectedMeasureIndex == 2)
                        {
                            measure /= 1000;
                            selectedMeasure = "дм";
                        }
                        string rawRoundedMeasure = measure.ToString("0.00");
                        string measureLabelContent = rawRoundedMeasure + " " + selectedMeasure;
                        measureLabel.Text = measureLabelContent;
                    }
                }
            }
            else if (isHorizontalDirectionSelectedTool)
            {
                if (isLineExists)
                {
                    int lineTypeIndex = lineTypeBox.SelectedIndex;
                    line.Stroke = System.Windows.Media.Brushes.LightGray;
                    line.StrokeDashArray = new DoubleCollection(
                        new double[] {
                            10
                        }
                    );
                }
            }
            else if (isVerticalDirectionSelectedTool)
            {
                if (isLineExists)
                {
                    int lineTypeIndex = lineTypeBox.SelectedIndex;
                    line.Stroke = System.Windows.Media.Brushes.LightGray;
                    line.StrokeDashArray = new DoubleCollection(
                        new double[] {
                            10
                        }
                    );
                }
            }
            else if (isCornerSelectedTool)
            {
                Point currentPosition = e.GetPosition(chart);
                double coordX = currentPosition.X;
                double coordY = currentPosition.Y;
                if (isStartCornerStep)
                {
                    System.Windows.Shapes.Path penCurve = new System.Windows.Shapes.Path();
                    Brush foreGroundColor = Brushes.Black;
                    penCurve.Stroke = foreGroundColor;
                    double brushSizePts = 1;
                    penCurve.StrokeThickness = brushSizePts;
                    PathGeometry pathGeometry = new PathGeometry();
                    PathFigureCollection pathFigureCollection = new PathFigureCollection();
                    PathFigure pathFigure = new PathFigure();
                    PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();
                    pathFigure.Segments = pathSegmentCollection;
                    pathFigure.StartPoint = new Point(coordX, coordY);
                    pathFigureCollection.Add(pathFigure);
                    pathGeometry.Figures = pathFigureCollection;
                    penCurve.Data = pathGeometry;
                    corner = new PolyBezierSegment();
                    corner.Points.Add(new Point(coordX, coordY));
                    corner.IsSmoothJoin = true;
                    PathSegment pathSegment = corner;
                    pathSegmentCollection.Add(pathSegment);
                    penCurve.StrokeDashArray = new DoubleCollection(
                        new double[] {
                            10
                        }
                    );
                    penCurve.Stroke = System.Windows.Media.Brushes.LightGray;
                    chart.Children.Add(penCurve);
                }
                else
                {
                    Point currentPoint = new Point(coordX, coordY);
                    double diffX = corner.Points[0].X - currentPoint.X;
                    double diffY = corner.Points[0].Y - currentPoint.Y;
                    double middlePointX = coordX + diffX / 2;
                    double middlePointY = coordY - diffY / 2;
                    corner.Points.Add(new Point(middlePointX, middlePointY));
                    corner.Points.Add(currentPoint);

                    double xDiff = currentPoint.X - corner.Points[0].X;
                    double yDiff = currentPoint.Y - corner.Points[0].Y;
                    double measure = Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI;

                    TextBlock measureLabel = new TextBlock();
                    chart.Children.Add(measureLabel);
                    Canvas.SetLeft(measureLabel, middlePointX);
                    double verticalOffset = 20;
                    Canvas.SetTop(measureLabel, middlePointY - verticalOffset);
                    bool isMeasureNegative = measure < 0;
                    if (isMeasureNegative)
                    {
                        measure *= -1;
                    }
                    string selectedMeasure = "°";
                    string rawRoundedMeasure = measure.ToString("0.00");
                    string measureLabelContent = rawRoundedMeasure + " " + selectedMeasure;
                    measureLabel.Text = measureLabelContent;

                }
                isStartCornerStep = !isStartCornerStep;
            }
        }

        private void UpdatePaperSizeHandler(object sender, SelectionChangedEventArgs e)
        {
            int selectedPaperSizeIndex = paperSize.SelectedIndex;
            UpdatePaperSize(selectedPaperSizeIndex);
        }

        public void UpdatePaperSize(int index)
        {
            if (isAppInit)
            {
                if (index == 0)
                {
                    chart.RenderSize = new Size(1000, 1000);
                }
                else if (index == 1)
                {
                    chart.RenderSize = new Size(2000, 2000);
                }
                else if (index == 2)
                {
                    chart.RenderSize = new Size(3000, 3000);
                }
            }
            else
            {
                isAppInit = true;
            }
        }

        private void ToggleCircleDrawModeToRadiusHandler(object sender, RoutedEventArgs e)
        {
            ToggleCircleDrawModeToRadius();
        }

        public void ToggleCircleDrawModeToRadius()
        {
            circleDrawMode = "radius";
            circleDrawModeIcon.Kind = PackIconKind.Radius;
        }

        private void ToggleCircleDrawModeToDiameterHandler(object sender, RoutedEventArgs e)
        {
            ToggleCircleDrawModeToDiameter();
        }

        public void ToggleCircleDrawModeToDiameter()
        {
            circleDrawMode = "diameter";
            circleDrawModeIcon.Kind = PackIconKind.Diameter;
        }

        private void UpdateLineWidthHandler(object sender, MouseEventArgs e)
        {
            UpdateLineWidth(e);
        }

        public void UpdateLineWidth(MouseEventArgs e)
        {
            MouseButtonState mousePressed = MouseButtonState.Pressed;
            MouseButtonState leftMouseButtonState = e.LeftButton;
            bool isMousePressed = leftMouseButtonState == mousePressed;
            if (isMousePressed)
            {
                double lineWidthControlWidth = lineWidthControl.Width;
                int rawLineWidthControlWidth = ((int)(lineWidthControlWidth));
                Point currentPosition = e.GetPosition(lineWidthWrap);
                double coordX = currentPosition.X;
                int ratio = 0;
                if (lastLineWidthCoord > coordX)
                {
                   ratio = -1;
                }
                else if (lastLineWidthCoord < coordX)
                {
                    ratio = 1;
                }
                lineWidthControl.Width += ratio;
                lineWidthControl.Height += ratio;
                lastLineWidthCoord = coordX;
                lineThickness = lineWidthControl.Width / 10;
                Canvas.SetLeft(lineWidthControl, lineWidthWrap.Width / 2 - lineWidthControl.Width / 2);
                Canvas.SetTop(lineWidthControl, lineWidthWrap.Height / 2 - lineWidthControl.Height / 2);

            }
        }

        private void GlobalHotKeyHandler(object sender, KeyEventArgs e)
        {
            Key currentKey = e.Key;
            var shiftModifier = Keyboard.Modifiers & ModifierKeys.Shift;
            bool isShiftModifierEnabled = shiftModifier > 0;
            if (isShiftModifierEnabled)
            {

            }
        }

    }
}
