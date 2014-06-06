using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Shapes;
using CompassPlus.Models;


namespace CompassPlus.Views
{
    // TODO: review

    public partial class CompassRoseView : UserControl, INotifyPropertyChanged
    {
         private const double Radius = 64D;
        private const double Size = Radius * 2;

        private const double Points = 360;
        private const double Epsilon = 1E-15D;

        private static readonly Dictionary<double, string> EigthPoints = new Dictionary<double, string>
            {
                {0, "N"}, // TODO: Localise
                {45, "NE"},
                {90, "E"},
                {135, "SE"},
                {180, "S"},
                {225, "SW"},
                {270, "W"},
                {315, "NW"},
            };

        public CompassRoseView()
        {
            InitializeComponent();

            this.PropertyChanged += CompassRoseView_PropertyChanged;
        }

        #region Event handlers

        private void CompassRoseView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CompassRose" && this.CompassRose != null)
            {
                BuildCompass(this.CompassRose);
            }
        } 

        #endregion

        #region Private methods

        private void BuildCompass(CompassRose compassRose)
        {
            CardinalDirectionCanvas.Children.Clear();

            switch (compassRose.Name)
            {
                case CompassRose.Rose8Name:
                    {
                        BuildEightPointRose(compassRose);
                        break;
                    }
                case CompassRose.Rose360Name:
                    {
                        Build360PointRose(compassRose);
                        break;
                    }
                case CompassRose.Rose6000Name:
                    {
                        Build6000MilstRose(compassRose);
                        break;
                    }
                case CompassRose.Rose6400Name:
                    {
                        Build6400MilstRose(compassRose);
                        break;
                    }
            }

            RenderOuterElipse(CardinalDirectionCanvas);
            RenderInnerElipse(CardinalDirectionCanvas);
        } 

        #endregion

        #region CompassPlus methods

        private void BuildEightPointRose(CompassRose compassRose)
        {
            // Render letters

            double degree = Points / compassRose.Points;

            for (int i = 0; i < compassRose.Points; i++)
            {
                var text = EigthPoints[i * degree];
                RenderText(CardinalDirectionCanvas, i, degree, text);
            }

            // Render markers

            const int points = 56;
            const double markerDegree = Points / points;

            for (int i = 0; i < points; i++)
            {
                this.RenderMarker(CardinalDirectionCanvas, i, markerDegree, 7);
            }
        }

        private void Build360PointRose(CompassRose compassRose)
        {
            // Render letters

            double degree = Points / compassRose.Points;

            for (int i = 0; i < compassRose.Points; i++)
            {
                string text = (i * degree).ToString(CultureInfo.InvariantCulture);
                RenderText(CardinalDirectionCanvas, i, degree, text);
            }

            // Render markers

            const int points = 180;
            const double markerDegree = Points / points;

            for (int i = 0; i < points; i++)
            {
                this.RenderMarker(CardinalDirectionCanvas, i, markerDegree, 5);
            }
        }

        private void Build6000MilstRose(CompassRose compassRose)
        {
            // Render letters

            double degree = Points / compassRose.Points;

            for (int i = 0; i < compassRose.Points; i++)
            {
                var text = (i * 5).ToString(CultureInfo.InvariantCulture);
                RenderText(CardinalDirectionCanvas, i, degree, text);
            }

            // Render markers

            const int mils60 = 60;
            const double markerDegree = Points / mils60;

            for (int i = 0; i < mils60; i++)
            {
                RenderMarker(CardinalDirectionCanvas, i, markerDegree, 5);
            }
        }

        private void Build6400MilstRose(CompassRose compassRose)
        {
            // Render letters

            double degrees = Points / compassRose.Points;

            for (int i = 0; i < compassRose.Points; i++)
            {
                var text = (i * (64 / 16)).ToString(CultureInfo.InvariantCulture);
                RenderText(CardinalDirectionCanvas, i, degrees, text);
            }

            // Render points

            const int mils64 = 64;
            const double pointDegree = Points / mils64;

            for (int i = 0; i < mils64; i++)
            {
                RenderMarker(CardinalDirectionCanvas, i, pointDegree, 4);
            }
        }

        private void RenderText(Canvas canvas, int i, double degrees, string text)
        {
            var textBlock = GetTextBlock();
            textBlock.Text = text;
            canvas.Children.Add(textBlock);

            // Make it negative
            double x = (textBlock.ActualWidth / 2) * -1;

            var tbTransformGroup = new TransformGroup();
            textBlock.RenderTransform = tbTransformGroup;

            var tbTranslateTransform = new TranslateTransform
            {
                X = Radius + x,
                Y = .5
            };
            tbTransformGroup.Children.Add(tbTranslateTransform);

            var tbRotateTransform = new RotateTransform();
            tbRotateTransform.Angle = i * degrees;
            tbRotateTransform.CenterX = Radius;
            tbRotateTransform.CenterY = Radius;
            tbTransformGroup.Children.Add(tbRotateTransform);
        }

        private void RenderMarker(Canvas canvas, int i, double degree, double bigMarker)
        {
            var rectangle = GetMarkerRectangle();
            canvas.Children.Add(rectangle);

            var reTransformGroup = new TransformGroup();
            rectangle.RenderTransform = reTransformGroup;

            // Render long line

            if (i % bigMarker < Epsilon)
            {
                rectangle.Height = rectangle.ActualHeight + 2D;
                rectangle.Width = rectangle.ActualWidth + .13D;
            }

            // Position

            var reTranslateTransform = new TranslateTransform
            {
                X = Radius + (rectangle.ActualWidth / 2) * -1,
                Y = 14.5D
            };

            reTransformGroup.Children.Add(reTranslateTransform);

            // Rotate 

            var reRotateTransform = new RotateTransform();
            reRotateTransform.Angle = i * degree;
            reRotateTransform.CenterX = Radius;
            reRotateTransform.CenterY = Radius;
            reTransformGroup.Children.Add(reRotateTransform);
        }

        private TextBlock GetTextBlock()
        {
            var text = new TextBlock
            {
                Style = TextStyle,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = Brush
            };
            return text;
        }

        private Rectangle GetMarkerRectangle()
        {
            var rectangle = new Rectangle();
            rectangle.HorizontalAlignment = HorizontalAlignment.Center;
            rectangle.Style = MarkerStyle;
            rectangle.Fill = Brush;
            return rectangle;
        }

        private void RenderOuterElipse(Canvas canvas)
        {
            var elipse = new Ellipse();
            elipse.Height = Size;
            elipse.Width = Size;
            elipse.Style = EllipseStyle;
            elipse.Fill = new SolidColorBrush(Colors.Transparent);
            elipse.StrokeThickness = 1;
            elipse.Stroke = Brush;

            canvas.Children.Add(elipse);
        }

        private void RenderInnerElipse(Canvas canvas)
        {
            const double ratio = 28;
            const double size = Size - ratio; // 100
            const double position = ratio / 2;

            var elipse = new Ellipse();
            elipse.Height = size;
            elipse.Width = size;
            elipse.Style = EllipseStyle;
            elipse.Fill = new SolidColorBrush(Colors.Transparent);
            elipse.StrokeThickness = .5;
            elipse.Stroke = Brush;

            var translateTransform2 = new TranslateTransform
            {
                X = position,
                Y = position
            };

            var transformGroup = new TransformGroup();
            transformGroup.Children.Add(translateTransform2);

            elipse.RenderTransform = transformGroup;

            canvas.Children.Add(elipse);
        }

        #endregion

        public static readonly DependencyProperty CompassRoseProperty = DependencyProperty.Register(
          "CompassRose", typeof(CompassRose), typeof(CompassRoseView), new PropertyMetadata(null, OnCompassRoseChanged));

        public static readonly DependencyProperty EllipseStyleProperty = DependencyProperty.Register(
         "EllipseStyle", typeof(Style), typeof(CompassRoseView), null);

        public static readonly DependencyProperty MarkerStyleProperty = DependencyProperty.Register(
            "MarkerStyle", typeof(Style), typeof(CompassRoseView), null);

        public static readonly DependencyProperty TextStyleProperty = DependencyProperty.Register(
            "TextStyle", typeof(Style), typeof(CompassRoseView), null);

        public static readonly DependencyProperty BrushProperty = DependencyProperty.Register(
           "Brush", typeof(Brush), typeof(CompassRoseView), new PropertyMetadata(null, OnCompassRoseChanged));

        private static void OnCompassRoseChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var myUserControl = (CompassRoseView)dependencyObject;
            myUserControl.OnPropertyChanged("CompassRose");
            myUserControl.OnCaptionPropertyChanged(e);
        }

        private void OnCaptionPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            // TODO Remove if not used
            // Raise property here
        }

        public CompassRose CompassRose
        {
            get { return (CompassRose)GetValue(CompassRoseProperty); }
            set { SetValue(CompassRoseProperty, value); }
        }

        public Style EllipseStyle
        {
            get { return (Style)GetValue(EllipseStyleProperty); }
            set { SetValue(EllipseStyleProperty, value); }
        }

        public Style MarkerStyle
        {
            get { return (Style)GetValue(MarkerStyleProperty); }
            set { SetValue(MarkerStyleProperty, value); }
        }

        public Style TextStyle
        {
            get { return (Style)GetValue(TextStyleProperty); }
            set { SetValue(TextStyleProperty, value); }
        }

        public Brush Brush
        {
            get { return (Brush)GetValue(BrushProperty); }
            set { SetValue(BrushProperty, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
