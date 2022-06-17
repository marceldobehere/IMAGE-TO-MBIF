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

            Console.WriteLine("Enter X Offset:");
            int xOff = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter Y Offset:");
            int yOff = int.Parse(Console.ReadLine());

            Console.WriteLine();

            if (Directory.Exists(args[0]))
            {
                string newFolder = $"{args[0]}-conv";
                Directory.CreateDirectory(newFolder);
                string[] files = Directory.GetFiles(args[0]);
                foreach (string file in files)
                    ConvImage(file, newFolder, xOff, yOff);
            }
            else if (File.Exists(args[0]))
            {
                ConvImage(args[0], ".", xOff, yOff);
            }
            else
            {
                Console.WriteLine("No valid File provided!");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("\n\nEnd.");
            Console.ReadLine();
        }


        static void ConvImage(string path, string resPath, int xOff, int yOff)
        {
            Bitmap image = new Bitmap(path);

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
            using (BinaryWriter writer = new BinaryWriter(new FileStream($"{resPath}/{Path.GetFileNameWithoutExtension(path)}.mbif", FileMode.Create)))
            {
                writer.Write(image.Width);          // 4 byte width
                writer.Write(image.Height);         // 4 byte height
                writer.Write(xOff);                 // 4 byte x offset
                writer.Write(yOff);                 // 4 byte y offset
                writer.Write(imageData.LongLength); // 8 byte image lenght
                writer.Write(imageData);            // image data
            }
            Console.WriteLine("Done.");
        }
    }
}
