using Android.Graphics;
using Android.Graphics.Drawables;
using Paint = Android.Graphics.Paint;
using Path = Android.Graphics.Path;

namespace TaskManagerUI.UI;

public class SemiCircleBackgroundDrawable : Drawable
{
    private readonly Paint _paint;

    public SemiCircleBackgroundDrawable()
    {
        _paint = new Paint
        {
            AntiAlias = true,
            Color = Android.Graphics.Color.ParseColor(Constant.AppStyle.PrimaryColor)
        };
        _paint.SetStyle(Paint.Style.Fill);
    }

    public override void Draw(Canvas canvas)
    {
        var width = Bounds.Width();
        var height = Bounds.Height();
        var cornerRadius = 80f;

        var dip = Android.App.Application.Context.Resources?.DisplayMetrics?.Density ?? 3;
        var curveWidth = 120f * dip;
        var curveDepth = 30f * dip;

        var path = new Path();

        path.MoveTo(0, height);
        path.LineTo(0, cornerRadius);
        path.QuadTo(0, 0, cornerRadius, 0);

        var leftCurveStart = (width - curveWidth) / 2;
        path.LineTo(leftCurveStart, 0);

        var controlPointOffset = curveWidth / 4;
        path.CubicTo(
            leftCurveStart + controlPointOffset, 0,
            leftCurveStart + controlPointOffset, curveDepth,
            leftCurveStart + curveWidth / 2, curveDepth
        );
        path.CubicTo(
            leftCurveStart + curveWidth - controlPointOffset, curveDepth,
            leftCurveStart + curveWidth - controlPointOffset, 0,
            leftCurveStart + curveWidth, 0
        );

        path.LineTo(width - cornerRadius, 0);
        path.QuadTo(width, 0, width, cornerRadius);
        path.LineTo(width, height);
        path.LineTo(0, height);
        path.Close();

        canvas.DrawPath(path, _paint);
    }

    public override void SetAlpha(int alpha) => _paint.Alpha = alpha;

    public override void SetColorFilter(ColorFilter? colorFilter) => _paint.SetColorFilter(colorFilter);

    public override int Opacity => (int)Format.Translucent;
}