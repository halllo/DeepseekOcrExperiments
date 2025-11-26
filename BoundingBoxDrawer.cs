using SkiaSharp;

namespace DeepseekOcrExperiments;

public static class BoundingBoxDrawer
{
    extension(SKCanvas canvas)
    {
        public void Draw(BoundingBox boundingBox, int imageWidth, int imageHeight)
        {
            var color = SKColors.Red;

            // Convert normalized [0,1000] coords to pixels
            var x1 = (int)(boundingBox.Coordinates.X1 / 1000.0f * imageWidth);
            var y1 = (int)(boundingBox.Coordinates.Y1 / 1000.0f * imageHeight);
            var x2 = (int)(boundingBox.Coordinates.X2 / 1000.0f * imageWidth);
            var y2 = (int)(boundingBox.Coordinates.Y2 / 1000.0f * imageHeight);

            var paint = new SKPaint
            {
                Color = color,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 4,
            };
            var path = new SKPath();
            path.MoveTo(x1, y1);
            path.LineTo(x2, y1);
            path.LineTo(x2, y2);
            path.LineTo(x1, y2);
            path.Close();
            canvas.DrawPath(path, paint);

            if (boundingBox.Value != null)
            {
                canvas.DrawText(
                    text: boundingBox.Value,
                    x: x1,
                    y: y1,
                    textAlign: SKTextAlign.Left,
                    font: new SKFont { Size = 40, },
                    paint: new SKPaint { Color = color, });
            }
        }
    }
}
