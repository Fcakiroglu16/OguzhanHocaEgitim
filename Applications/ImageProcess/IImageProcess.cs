using System;
using System.Collections.Generic;
using System.Text;

namespace Applications.ImageProcess
{
    internal interface IImageProcess
    {
        void AddWatermark(string filePath, string watermarkText);
    }


    public class ImageProcess : IImageProcess
    {
        public void AddWatermark(string filePath, string watermarkText)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new Exception("filePath null olamaz");
            }


            // Logic to add watermark to the image at filePath using watermarkText
            Console.WriteLine($"Adding watermark '{watermarkText}' to image at '{filePath}'");
        }
    }
}
