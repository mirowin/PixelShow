using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System;
using Gif.Components;

namespace GifMaker
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Надпись:");
			string text = Console.ReadLine().ToUpper();
			Console.WriteLine("Время в милисекундах (1сек - 1000мсек):");
			int Time = Int32.Parse(Console.ReadLine());
			Console.WriteLine("Задержка между кадрами:");
			int fd  = Int32.Parse(Console.ReadLine());
			Console.WriteLine("Ширина матрицы:");
			int w = Int32.Parse(Console.ReadLine());
			Console.WriteLine("Высота матрицы:");
			int h = Int32.Parse(Console.ReadLine());

			CreateGif(text, Time, fd, w, h);
		}

		private static void CreateGif(string imageText, double time, int frameDelay, int width, int height)
		{
			string outputFilePath = "gif/" + $"{imageText}.gif";
			int x = width;
			double frameShift = 0;
			var stringFormat = new StringFormat();
			Bitmap bitmap = new Bitmap(width, height);
			SolidBrush Brush = new SolidBrush(Color.FromArgb(255, 255, 255));

			// Создаем объект Font для "рисования" им текста.
			Font font = new Font("Arial", 18, FontStyle.Bold, GraphicsUnit.Pixel);
			// Создаем объект Graphics для вычисления ширины текста.
			Graphics graphics = Graphics.FromImage(bitmap);
			// Пишем (рисуем) текст
			stringFormat.Alignment = StringAlignment.Near;
			stringFormat.LineAlignment = StringAlignment.Near;
			var textWidth = (int)graphics.MeasureString(imageText, font).Width + width;

			var frameCount = Convert.ToInt32(Math.Floor(time / frameDelay));
			var textSpeed = Convert.ToDouble(textWidth) / frameCount;

			AnimatedGifEncoder GifEncoder = new AnimatedGifEncoder();
			GifEncoder.Start(outputFilePath);
			GifEncoder.SetDelay(frameDelay);
			GifEncoder.SetRepeat(0);
			for (int i = 0; i <= frameCount; i++)
			{
				// Пересоздаем объект Bitmap
				bitmap = new Bitmap(bitmap, new Size(width, height));
				// Пересоздаем объект Graphics
				graphics = Graphics.FromImage(bitmap);
				// Задаем параметры анти-алиасинга
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
				// Стираем старый кадр
				graphics.Clear(Color.Black);
				graphics.DrawRectangle(new Pen(Color.Black), 1, 1, width, height);
				// Пишем новый кадр
				graphics.DrawString(imageText, font, Brush, x, -1, stringFormat);
				graphics.Flush();

				frameShift = frameShift + textSpeed;
				x = Convert.ToInt32(Math.Floor(frameShift)) - Convert.ToInt32(Math.Floor(frameShift)) * 2 + width;
				GifEncoder.AddFrame(bitmap);
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.Write(".");
			}
			GifEncoder.Finish();
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Все готово!");
		}
	}
}
