using System;
using System.Collections.Generic;
using System.Drawing;

namespace CoinsActivity
{
    public static class DIPFilter
    {
        public static void GrayScale(ref Bitmap a)
        {
            for (int x = 0; x < a.Width; x++)
            {
                for (int y = 0; y < a.Height; y++)
                {
                    Color pixel = a.GetPixel(x, y);
                    int average = (pixel.R + pixel.G + pixel.B) / 3;
                    Color gray = Color.FromArgb(average, average, average);
                    a.SetPixel(x, y, gray);
                }
            }
        }

        public static void Threshold(ref Bitmap a)
        {
            int threshold = 200;
            for (int x = 0; x < a.Width; x++)
            {
                for (int y = 0; y < a.Height; y++)
                {
                    Color pixel = a.GetPixel(x, y);
                    int avg = (pixel.R + pixel.G + pixel.B) / 3;
                    a.SetPixel(x, y, avg < threshold ? Color.Black : Color.White);
                }
            }
        }

        public static void GaussianBlur(ref Bitmap a)
        {
            double[,] kernel = {
                { 1, 2, 1 },
                { 2, 4, 2 },
                { 1, 2, 1 }
            };
            double kernelSum = 16;

            Bitmap tempBitmap = new Bitmap(a.Width, a.Height);

            for (int x = 1; x < a.Width - 1; x++)
            {
                for (int y = 1; y < a.Height - 1; y++)
                {
                    double r = 0, g = 0, b = 0;

                    for (int kx = -1; kx <= 1; kx++)
                    {
                        for (int ky = -1; ky <= 1; ky++)
                        {
                            Color neighborPixel = a.GetPixel(x + kx, y + ky);
                            double weight = kernel[kx + 1, ky + 1];
                            r += neighborPixel.R * weight;
                            g += neighborPixel.G * weight;
                            b += neighborPixel.B * weight;
                        }
                    }

                    int newR = Math.Min(255, Math.Max(0, (int)(r / kernelSum)));
                    int newG = Math.Min(255, Math.Max(0, (int)(g / kernelSum)));
                    int newB = Math.Min(255, Math.Max(0, (int)(b / kernelSum)));

                    tempBitmap.SetPixel(x, y, Color.FromArgb(newR, newG, newB));
                }
            }

            a = (Bitmap)tempBitmap.Clone();
        }
    }

    public class Algo
    {
        public const float PESO_FIVE = 5;
        public const float PESO_ONE = 1;
        public const float CENT_25 = 0.25f;
        public const float CENT_10 = 0.10f;
        public const float CENT_5 = 0.05f;

        public static void Calculate(Bitmap a, ref float res, ref int count)
        {
            bool[,] visited = new bool[a.Width, a.Height];

            for (int x = 0; x < a.Width; x++)
            {
                for (int y = 0; y < a.Height; y++)
                {
                    if (visited[x, y]) continue;

                    Color pixel = a.GetPixel(x, y);

                    if (IsBlack(pixel))
                    {
                        int pixelCount = BFS(ref a, ref visited, x, y);
                        if (pixelCount < 1000) continue;

                        count++;
                        res += GetCoinType(pixelCount);
                    }
                }
            }
        }

        private static float GetCoinType(int pixelCount)
        {
            if (pixelCount > 7000) return PESO_FIVE;
            if (pixelCount > 5000) return PESO_ONE;
            if (pixelCount > 4000) return CENT_25;
            if (pixelCount > 3000) return CENT_10;
            if (pixelCount > 1000) return CENT_5;
            return 0;
        }

        private static int BFS(ref Bitmap a, ref bool[,] visited, int x, int y)
        {
            Queue<int[]> queue = new Queue<int[]>();
            queue.Enqueue(new int[] { x, y });
            int pixelCount = 0;

            while (queue.Count > 0)
            {
                int[] top = queue.Dequeue();
                int currX = top[0], currY = top[1];
                if (visited[currX, currY]) continue;

                visited[currX, currY] = true;
                pixelCount++;

                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        int newX = currX + dx, newY = currY + dy;
                        if (newX >= 0 && newX < a.Width && newY >= 0 && newY < a.Height &&
                            !visited[newX, newY] && IsBlack(a.GetPixel(newX, newY)))
                        {
                            queue.Enqueue(new int[] { newX, newY });
                        }
                    }
                }
            }

            return pixelCount;
        }

        private static bool IsBlack(Color a)
        {
            return a.R == 0 && a.G == 0 && a.B == 0;
        }
    }
}
