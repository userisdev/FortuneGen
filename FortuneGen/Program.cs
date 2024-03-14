using SkiaSharp;
using System;
FileInfo Generate(string text, SKColor color, SKColor bg)
{
    var width = 100;
    var height = 170;

    var borderWidth = 20;
    var borderMargin = 1;

    var textDrawWidth = width - (borderWidth + borderMargin) * 2;
    var textDrawHeight = height - (borderWidth + borderMargin) * 2;

    string fontPath = Path.Combine(Environment.CurrentDirectory, "玉ねぎ楷書激無料版v7改.ttf");


    using SKSurface surface = SKSurface.Create(new SKImageInfo(width, height));
    using SKCanvas canvas = surface.Canvas;

    canvas.Clear(bg);

    using SKPaint rectPaint = new()
    {
        Style = SKPaintStyle.Stroke,
        Color = color,
        StrokeWidth = borderWidth,
    };

    SKRect borderRect = new(0, 0, width, height);
    canvas.DrawRect(borderRect, rectPaint);

    var items = text.Divide().ToArray();

    using SKPaint textPaint = new()
    {
        Color = color,
        TextSize = textDrawHeight / items.Length,
        TextAlign = SKTextAlign.Center,
        Typeface = SKTypeface.FromFile(fontPath),
        SubpixelText = true
    };

    while(textDrawWidth < items.Select(textPaint.MeasureText).Max())
    {
        textPaint.TextSize -= 1;
    }

    var charHeight = textPaint.FontMetrics.Descent - textPaint.FontMetrics.Ascent;
    var textHeight = charHeight * items.Length;
    var yOffset = (height - textHeight)/2;

    foreach (var (item, index) in items.Select((e, i) => (e, i)))
    {
        float drawSize = textPaint.MeasureText(text);
        canvas.DrawText(item, 1.0f*width/2, yOffset +index*charHeight - textPaint.FontMetrics.Ascent, textPaint);
    }

    using SKImage img = surface.Snapshot();
    using SKData tmp = img.Encode();

    var savePath = Path.Combine(Environment.CurrentDirectory, $"{text}.png");
    if (File.Exists(savePath))
    {
        File.Delete(savePath);
    }

    using var fs = File.Create(savePath);
    tmp.SaveTo(fs);

    return new FileInfo(savePath);
}

var items = new[] { "大吉", "吉", "中吉", "小吉", "末吉", "凶", "大凶","こんにちは" };
foreach (var item in items)
{
    Generate(item, SKColors.Red, SKColors.White);
}