using G.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace G.Test.Models
{
    public class PlayerModel : IEntityModel
    {
        public PlayerModel(Player data)
        {
            Data = data;
            _body = new Rectangle()
            {
                Height = 10,
                Width = 10,
                Fill = new SolidColorBrush(Colors.Aqua)
            };
            _direction = new Line()
            {
                X1 = Data.Position.Location.X,
                Y1 = Data.Position.Location.Y,
                X2 = Data.Position.Location.X + Data.Position.Direction.X * 20,
                Y2 = Data.Position.Location.Y + Data.Position.Direction.Y * 20,
                Stroke = Brushes.Black,
                StrokeThickness = 3
            };
            currentCanvas = null;
        }
        public Entity Data { get; set; } 
        Rectangle _body;
        Line _direction;
        Canvas currentCanvas;

        public void Draw(Canvas canvas)
        {
            if (currentCanvas != null && currentCanvas != canvas)
            {
                try
                {
                    if (currentCanvas.Children.Contains(_body))
                    {
                        currentCanvas.Children.Remove(_body);
                    }
                    if (currentCanvas.Children.Contains(_direction))
                    {
                        currentCanvas.Children.Remove(_direction);
                    }
                }
                catch (Exception)
                {
                }
            }
            Canvas.SetLeft(_body, Data.Position.Location.X - 5);
            Canvas.SetTop(_body, Data.Position.Location.Y - 5);
            _direction.X1 = Data.Position.Location.X;
            _direction.Y1 = Data.Position.Location.Y;
            _direction.X2 = Data.Position.Location.X + Data.Position.Direction.X * 20;
            _direction.Y2 = Data.Position.Location.Y + Data.Position.Direction.Y * 20;
        
            if (!canvas.Children.Contains(_body))
            {
                canvas.Children.Add(_body);
            }
            if (!canvas.Children.Contains(_direction))
            {
                canvas.Children.Add(_direction);
            }
            currentCanvas = canvas;
        }

        public void Dispose()
        {
            if (currentCanvas != null)
            {
                try
                {
                    currentCanvas.Children.Remove(_body);
                    currentCanvas.Children.Remove(_direction);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
