using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Staffinfo.Desktop.Helpers
{
    public static class BitmapImageHelper
    {
        /// <summary>
        /// Установить расширение (наверняка костыль=))
        /// </summary>
        /// <param name="btm">изображение</param>
        /// <param name="height">высота,px</param>
        /// <param name="width">ширина,px</param>
        /// <returns></returns>
        public static BitmapImage SetSize(BitmapImage btm, int height, int width)
        {
            var imageBytes = ImageToByte(btm);      //конвертируем изображение в byte[]

            using (var ms = new MemoryStream(imageBytes))
            {
                ms.Seek(0, SeekOrigin.Begin);

                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = ms;
                image.DecodePixelHeight = height;
                image.DecodePixelWidth = width;
                image.CacheOption = BitmapCacheOption.OnLoad; ;
                image.EndInit();

                return image;
            }
        }

        /// <summary>
        /// Конвертирует из массива байт в BitmapImage
        /// </summary>
        /// <param name="imageBytes">массив байт</param>
        /// <returns></returns>
        public static BitmapImage ByteToImage(byte[] imageBytes)
        {
            using (var ms = new MemoryStream(imageBytes))
            {
                ms.Seek(0, SeekOrigin.Begin);

                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = ms;
                image.DecodePixelHeight = 600;
                image.DecodePixelWidth = 600;
                image.CacheOption = BitmapCacheOption.OnLoad; ;
                image.EndInit();

                return image;
            }
        }

        /// <summary>
        /// Конвертирует картинку в массив байт
        /// </summary>
        /// <param name="image">исходное изображение</param>
        /// <returns></returns>
        public static byte[] ImageToByte(BitmapImage image)
        {
            if (image == null) return null;

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var ms = new MemoryStream())
            {
                encoder.Save(ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Сравнивает 2 изображения
        /// </summary>
        /// <param name="btm1">первое изображение</param>
        /// <param name="btm2">второе изображение</param>
        /// <returns></returns>
        public static bool ImageCompare(BitmapImage btm1, BitmapImage btm2)
        {
            return Convert.ToBase64String(ImageToByte(btm1))
                == Convert.ToBase64String(ImageToByte(btm2));
        }
    }
}