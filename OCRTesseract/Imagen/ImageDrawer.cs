using OCRTesseract.OCR;
using System.Drawing;
using System.Drawing.Imaging;

namespace OCRTesseract.Imagen
{
    internal class ImageDrawer
    {
        //public static void DrawRectanglesOnImage(string imagePath, Rectangle[] rectangles, string outputImagePath)
        //{
        //    // Cargar la imagen original
        //    using (var image = new Bitmap(imagePath))
        //    {
        //        // Crear una nueva instancia de Bitmap con el formato adecuado para trabajar con Graphics
        //        using (var newImage = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb))
        //        {
        //            // Copiar la imagen original a la nueva imagen
        //            using (var graphics = Graphics.FromImage(newImage))
        //            {
        //                graphics.DrawImage(image, 0, 0, image.Width, image.Height);
        //            }

        //            // Crear un objeto Graphics para dibujar en la nueva imagen
        //            using (var graphics = Graphics.FromImage(newImage))
        //            {
        //                // Definir el color y el grosor del borde del rectángulo
        //                Pen pen = new Pen(Color.Red, 2);

        //                // Dibujar cada rectángulo en la imagen
        //                foreach (var rect in rectangles)
        //                {
        //                    graphics.DrawRectangle(pen, rect);
        //                }
        //            }
        //            // Guardar la nueva imagen con los rectángulos dibujados en un nuevo archivo
        //            newImage.Save(outputImagePath);
        //        }

        //        // Crear una nueva instancia de Bitmap con el formato adecuado para trabajar con Graphics
        //        using (var newImage2 = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb))
        //        {
        //            // Crear un objeto Graphics para dibujar sobre la imagen de salida
        //            using (var graphics = Graphics.FromImage(newImage2))
        //            {
        //                // Dibujar la imagen original en la imagen de salida
        //                graphics.DrawImage(image, 0, 0, image.Width, image.Height);

        //                // Crear un pincel para pintar sobre las regiones de palabras identificadas
        //                using (var brush = new SolidBrush(Color.White))
        //                {
        //                    // Iterar a través de las regiones de palabras identificadas y rellenarlas con el color del pincel
        //                    foreach (var rect in rectangles)
        //                    {
        //                        graphics.FillRectangle(brush, rect);
        //                    }
        //                }
        //            }
        //            // Guardar la nueva imagen con los rectángulos dibujados en un nuevo archivo
        //            newImage2.Save(outputImagePath.Replace(".tif","2.tif"));
        //        }
        //    }
        //}


        public static void DrawRectanglesOnImage(string imagePath, Rectangle[] rectangles, string[] outputImagePath)
        {
            // Cargar la imagen original
            using (var image = new Bitmap(imagePath))
            {
                // Crear una nueva instancia de Bitmap con el formato adecuado para trabajar con Graphics
                using (var newImage = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb))
                {
                    // Copiar la imagen original a la nueva imagen
                    using (var graphics = Graphics.FromImage(newImage))
                    {
                        graphics.DrawImage(image, 0, 0, image.Width, image.Height);
                    }

                    // Crear un objeto Graphics para dibujar en la nueva imagen
                    using (var graphics = Graphics.FromImage(newImage))
                    {
                        // Definir el color y el grosor del borde del rectángulo
                        Pen pen = new Pen(Color.Red, 2);

                        // Dibujar cada rectángulo en la imagen
                        foreach (var rect in rectangles)
                        {
                            graphics.DrawRectangle(pen, rect);
                        }
                    }
                    // Guardar la nueva imagen con los rectángulos dibujados en un nuevo archivo
                    newImage.Save(outputImagePath[0]);
                }

                // Crear una nueva instancia de Bitmap con el formato adecuado para trabajar con Graphics
                using (var newImage2 = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb))
                {
                    // Crear un objeto Graphics para dibujar sobre la imagen de salida
                    using (var graphics = Graphics.FromImage(newImage2))
                    {
                        // Dibujar la imagen original en la imagen de salida
                        graphics.DrawImage(image, 0, 0, image.Width, image.Height);

                        // Crear un pincel para pintar sobre las regiones de palabras identificadas
                        using (var brush = new SolidBrush(Color.White))
                        {
                            // Iterar a través de las regiones de palabras identificadas y rellenarlas con el color del pincel
                            foreach (var rect in rectangles)
                            {
                                graphics.FillRectangle(brush, rect);
                            }
                        }
                    }
                    // Guardar la nueva imagen con los rectángulos dibujados en un nuevo archivo
                    newImage2.Save(outputImagePath[1]);
                }
            }
        }
    }
}
