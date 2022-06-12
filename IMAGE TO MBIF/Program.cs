using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace IMAGE_TO_MBIF
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("No File provided!");
                Console.ReadLine();
                return;
            }

            Bitmap image = new Bitmap(args[0]);

            BitmapData bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            Console.WriteLine($"Image: {image.Width}x{image.Height} = {image.Width * image.Height} ({image.Width * image.Height * 4} bytes)");
            byte[] imageData = new byte[image.Height * image.Width * 4];
            try
            {
                unsafe
                {
                    byte* ptr = (byte*)bitmapData.Scan0;
                    for (int i = 0; i < image.Height * image.Width * 4; i++)
                        imageData[i] = ptr[i];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Reading Image failed!");
            }
            Console.WriteLine("Image was read.");
            Console.WriteLine();

            Console.WriteLine("Writing Image File.");
            using (BinaryWriter writer = new BinaryWriter(new FileStream($"{Path.GetFileNameWithoutExtension(args[0])}.mbif", FileMode.Create)))
            {
                writer.Write(image.Width);
                writer.Write(image.Height);
                writer.Write(imageData.Length);
                writer.Write(imageData);
            }
            Console.WriteLine("Done.");

            Console.WriteLine("\n\nEnd.");
            Console.ReadLine();
        }
    }
}
