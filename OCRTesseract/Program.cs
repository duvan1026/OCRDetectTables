/*
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Tesseract;
using System.Drawing;
using System;
using System.Collections.Generic;

namespace TableDetectionExample
{
    class Program
    {
        private const string environmentVariable = "TESDATA_PREFIX";
        private const string tessdataPath = @"C:\Program Files (x86)\Tesseract-OCR\tessdata";
        private const string language = "spa";


        static void Main(string[] args)
        {
            // Ruta de la imagen .tif
            string imagePath = "D:\\Equipo\\Imagenes\\procesos-scrum-tif.tif";

            // Cargar la imagen utilizando Emgu.CV
            Image<Bgr, byte> image = new Image<Bgr, byte>(imagePath);

            // Convertir la imagen a escala de grises para simplificar el proceso
            Image<Gray, byte> grayImage = image.Convert<Gray, byte>();

            // Aplicar el umbral adaptativo para obtener una imagen binaria
            grayImage = grayImage.ThresholdAdaptive(new Gray(255), AdaptiveThresholdType.GaussianC, ThresholdType.Binary, 15, new Gray(2));

            // Detectar los contornos en la imagen binaria
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            Mat hierachy = new Mat();
            CvInvoke.FindContours(grayImage, contours, hierachy, RetrType.External, ChainApproxMethod.ChainApproxSimple);

            // Iterar a través de los contornos y dibujarlos en la imagen original
            for (int i = 0; i < contours.Size; i++)
            {
                CvInvoke.DrawContours(image, contours, i, new MCvScalar(0, 0, 255), 2);
            }

            Environment.SetEnvironmentVariable(environmentVariable,tessdataPath);

            // Inicializar el motor de Tesseract para el reconocimiento de texto
            using (var engine = new TesseractEngine(tessdataPath, language, EngineMode.Default))
            {
                // Opcional: Configurar parámetros de Tesseract, si es necesario
                //engine.SetVariable("tessedit_char_whitelist", "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ.,;:!?");

                // Convertir la imagen binaria a Bitmap para que Tesseract pueda procesarla
                Bitmap bitmap = ConvertToBitmap(grayImage);

                // Realizar OCR en el bitmap para detectar texto (incluyendo números)
                using (var page = engine.Process(bitmap))
                {
                    string recognizedText = page.GetText();
                    Console.WriteLine("Texto reconocido por Tesseract:");
                    Console.WriteLine(recognizedText);
                }
            }

            // Mostrar la imagen con los contornos y líneas detectados
            CvInvoke.Imshow("Tabla Detectada", image);
            CvInvoke.WaitKey(0);

            Console.ReadLine();
        }

        private static Bitmap ConvertToBitmap(Image<Gray, byte> grayImage)
        {
            int width = grayImage.Width;
            int height = grayImage.Height;
            byte[] rawValues = new byte[width * height];

            int counter = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    rawValues[counter++] = (byte)grayImage[y, x].Intensity;
                }
            }

            Bitmap bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            System.Drawing.Imaging.BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, width, height), System.Drawing.Imaging.ImageLockMode.WriteOnly, bitmap.PixelFormat);
            System.Runtime.InteropServices.Marshal.Copy(rawValues, 0, bmpData.Scan0, rawValues.Length);
            bitmap.UnlockBits(bmpData);

            return bitmap;
        }
    }
}
*/



// Opcion Numero 1 
/* Deteccion de tabla, sin aplicar mas filtros adicionales
using System;
using System.Collections.Generic;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace TableDetection
{
    class Program
    {
        static void Main(string[] args)
        {
            DetectAndDrawTables("D:\\Equipo\\Imagenes\\procesos-scrum-tif.tif");
        }

        static void DetectAndDrawTables(string imagePath)
        {
            //Cargar la imagen:
            Image<Bgr, byte> image = new Image<Bgr, byte>(imagePath);

            //Convertir la imagen a escala de grises:
            Image<Gray, byte> grayImage = image.Convert<Gray, byte>();

            //Aplicar suavizado (opcional, pero puede ayudar a mejorar la detección de contornos)
            CvInvoke.GaussianBlur(grayImage, grayImage, new Size(5, 5), 0);

            //Detectar bordes utilizando Canny:
            double cannyThreshold = 100;
            double cannyThresholdLinking = 50;
            Image<Gray, byte> cannyEdges = grayImage.Canny(cannyThreshold, cannyThresholdLinking);

            //Encontrar los contornos en la imagen
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            Mat hierarchy = new Mat();
            CvInvoke.FindContours(cannyEdges, contours, hierarchy, RetrType.List, ChainApproxMethod.ChainApproxSimple);


            //Iterar a través de los contornos encontrados y seleccionar aquellos que parezcan ser tablas
            List<RotatedRect> tableRectangles = new List<RotatedRect>();
            for (int i = 0; i < contours.Size; i++)
            {
                using (VectorOfPoint contour = contours[i])
                {
                    double area = CvInvoke.ContourArea(contour);
                    if (area > 350)
                    {
                        RotatedRect rotatedRect = CvInvoke.MinAreaRect(contour);
                        if (Math.Abs(rotatedRect.Angle) < 15)
                        {
                            tableRectangles.Add(rotatedRect);
                        }
                    }
                }
            }

            //Dibujar los contornos de las tablas detectadas en la imagen original
            foreach (RotatedRect rect in tableRectangles)
            {
                PointF[] vertices = rect.GetVertices();
                Point[] points = Array.ConvertAll(vertices, Point.Round);
                VectorOfPoint contour = new VectorOfPoint(points);
                CvInvoke.DrawContours(image, new VectorOfVectorOfPoint(contour), -1, new MCvScalar(0, 255, 0), 2);
            }

            //Mostrar la imagen resultante con los contornos resaltados
            CvInvoke.Imshow("Tablas Detectadas", image);
            CvInvoke.WaitKey(0);
        }
    }
}*/


// Opcion numero 2
// Se aplican filtro de la transformada de hough para detectar lineas directamente
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using OCRTesseract.Imagen;
using OCRTesseract.OCR;
using Tesseract;
using static Emgu.CV.Dai.OpenVino;

namespace TableDetection
{
    class Program
    {

        static void Main(string[] args)
        {
            //DetectAndDrawTables("D:\\Equipo\\Imagenes\\procesos-scrum-tif.tif");
            //DetectAndDrawTables("D:\\Equipo\\Imagenes\\PASIVO00000788F.tif");   

            OcrResult ocrResult1 = new OcrResult();

            //string imagePath = "D:\\Equipo\\Imagenes\\PASIVO00000788F.tif";
            string imagePath = "D:\\Equipo\\Imagenes\\procesos-scrum-tif.tif";



            string[] fileSaves =
            {
            "D:\\DUVAN\\PruebasContornoTablasOCR\\imagen_rectangulos_palabras.tif",
            "D:\\DUVAN\\PruebasContornoTablasOCR\\imagen_sin_palabras.tif",
            "D:\\DUVAN\\PruebasContornoTablasOCR\\imagen_Contorno_Tablas_Option1.tif",
            "D:\\DUVAN\\PruebasContornoTablasOCR\\imagen_Contorno_Tablas_Option2.tif"
            };


            var ocrResult = ocrResult1.PerformOcr(imagePath);

            Console.WriteLine("Texto detectado: ");
            Console.WriteLine(ocrResult.Text);


            // Mostrar las coordenadas de las palabras detectadas
            Console.WriteLine("Coordenadas de las palabras detectadas: ");
            foreach (var rect in ocrResult.Regions)
            {
                Console.WriteLine($"X: {rect.X}, Y: {rect.Y}, Width: {rect.Width}, Height: {rect.Height}");
            }

            ImageDrawer.DrawRectanglesOnImage(imagePath, ocrResult.Regions, fileSaves);


            DetectAndDrawTables(fileSaves);
            DetectAndDrawTablesOption2(fileSaves);

            Console.WriteLine("Ha terminado el proceso................" );
            Console.ReadLine();
        }


        static void DetectAndDrawTablesOption2(string[] imagePathArray)
        {
            string imagePath = imagePathArray[1];
            string imageOutPath = imagePathArray[3];

            // Cargamos la imagen desde el archivo
            Image<Gray, byte> imagen = new Image<Gray, byte>(imagePath);


            /*Opcion 1 muy buena */
            // 2. Aplicar el filtro Laplaciano para resaltar los bordes
            int kernelSizeFilterLaplaciano = 1; // Tamaño del kernel, puedes cambiar este valor (debe ser un número impar)
            double scaleFilterLaplaciano = 1;   // Valor de escala, puedes cambiar este valor
            CvInvoke.Laplacian(imagen, imagen, DepthType.Cv8U, kernelSizeFilterLaplaciano, scaleFilterLaplaciano, 0, BorderType.Default);
            CvInvoke.Imshow("Aplicar el filtro Laplaciano para resaltar los bordes", imagen);
            CvInvoke.WaitKey(0);

            // Aplicamos un filtro Gaussiano para reducir el ruido
            imagen = imagen.SmoothGaussian(5);
            CvInvoke.Imshow("Bordes filtro Gaussiano para reducir el ruido", imagen);
            CvInvoke.WaitKey(0);

            // 3. Ajustar el contraste para mejorar la visualización
            imagen._EqualizeHist();
            CvInvoke.Imshow("Ajustar el contraste para mejorar la visualización", imagen);
            CvInvoke.WaitKey(0);

            // 2. Aplicamos el Filtro de Media para eliminar el ruido manteniendo la estructura principal
            int kernelSize = 5;
            CvInvoke.Blur(imagen, imagen, new Size(kernelSize, kernelSize), new Point(-1, -1));
            CvInvoke.Imshow("Filtro de Media para eliminar el ruido manteniendo la estructura principal", imagen);
            CvInvoke.WaitKey(0);

            // 3. Aplicamos la Dilatación y Erosión para mejorar la continuidad de las líneas y eliminar puntos o segmentos
            kernelSize = 3;
            Mat kernel = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(kernelSize, kernelSize), new Point(-1, -1));
            CvInvoke.Dilate(imagen, imagen, kernel, new Point(-1, -1), 1, BorderType.Reflect, default(MCvScalar));
            CvInvoke.Erode(imagen, imagen, kernel, new Point(-1, -1), 1, BorderType.Reflect, default(MCvScalar));
            CvInvoke.Imshow("Dilatación y Erosión para mejorar la continuidad de las líneas y eliminar puntos o segmentos", imagen);
            CvInvoke.WaitKey(0);


            // Aplicamos el filtrado bilateral para reducir el ruido manteniendo bordes
            Image<Gray, byte> imagenFiltrada = new Image<Gray, byte>(imagen.Size);
            CvInvoke.BilateralFilter(imagen, imagenFiltrada, 9, 75, 75);
            CvInvoke.Imshow("filtrado bilateral para reducir el ruido manteniendo bordes", imagen);
            CvInvoke.WaitKey(0);

            //// Aplicamos el filtrado de mediana para eliminar detalles pequeños
            //imagenFiltrada = imagen.SmoothMedian(5);
            //CvInvoke.Imshow("filtrado de mediana para eliminar detalles pequeños", imagen);
            //CvInvoke.WaitKey(0);


            // 4. Aplicamos la Umbralización Adaptativa para segmentar la imagen en regiones de iluminación uniforme
            CvInvoke.AdaptiveThreshold(imagen, imagen, 255, AdaptiveThresholdType.GaussianC, ThresholdType.Binary, 11, 10);
            CvInvoke.Imshow("la Umbralización Adaptativa ", imagen);
            CvInvoke.WaitKey(0);

            // 5. Utilizamos técnicas de umbralización para encontrar los contornos en la imagen
            VectorOfVectorOfPoint contornos = new VectorOfVectorOfPoint();
            Mat hierarchy = new Mat();
            CvInvoke.FindContours(imagen, contornos, hierarchy, RetrType.List, ChainApproxMethod.ChainApproxSimple);

            // 6. Analizamos los contornos para encontrar los puntos de las esquinas
            Image<Bgr, byte> imagenOriginal = new Image<Bgr, byte>(imagePath);
            for (int i = 0; i < contornos.Size; i++)
            {
                double epsilon = 0.03 * CvInvoke.ArcLength(contornos[i], true);
                VectorOfPoint approx = new VectorOfPoint();
                CvInvoke.ApproxPolyDP(contornos[i], approx, epsilon, true);

                if (approx.Size == 4) // Si el contorno tiene cuatro lados (cuadrilátero), probablemente sea una esquina
                {
                    Point[] corners = approx.ToArray();
                    foreach (Point corner in corners)
                    {
                        CvInvoke.Circle(imagenOriginal, corner, 5, new MCvScalar(0, 0, 255), -1);
                    }
                }
            }

            // 7. Aplicamos el operador Canny para detectar bordes
            Image<Gray, byte> bordes = imagen.Canny(100, 200);

            // 8. Aplicamos la transformación de Hough para detectar líneas en los bordes
            LineSegment2D[] lineas = CvInvoke.HoughLinesP(
                bordes,                    // Imagen con los bordes detectados
                1,                         // Resolución del parámetro rho
                Math.PI / 180,             // Resolución del parámetro theta (en radianes)
                150,                       // Umbral de detección (número mínimo de intersecciones)
                5,                        // Longitud mínima de línea aceptada (ajusta este valor según tus necesidades)
                10                         // Separación máxima entre píxeles para considerarlas como parte de la misma línea
            );

            // Dibujamos las líneas detectadas en la imagen original
            foreach (LineSegment2D linea in lineas)
            {
                imagenOriginal.Draw(linea, new Bgr(0, 255, 0), 2);
            }

            // Mostramos la imagen con los bordes de la tabla y las esquinas detectadas
            CvInvoke.Imshow("Bordes de la tabla y esquinas detectadas", imagenOriginal);
            CvInvoke.WaitKey(0);

            imagenOriginal.Save(imageOutPath);
        }


        static void DetectAndDrawTables(string[] imagePathArray)
        {
            string imagePath = imagePathArray[1];
            string imageOutPath = imagePathArray[2];

            // Cargamos la imagen desde el archivo
            Image<Gray, byte> imagen = new Image<Gray, byte>(imagePath);


            // Aplicamos un filtro Gaussiano para reducir el ruido
            imagen = imagen.SmoothGaussian(5);
            CvInvoke.Imshow("Bordes filtro Gaussiano para reducir el ruido", imagen);
            CvInvoke.WaitKey(0);

            // Aplicamos el filtrado bilateral para reducir el ruido manteniendo bordes
            Image<Gray, byte> imagenFiltrada = new Image<Gray, byte>(imagen.Size);
            CvInvoke.BilateralFilter(imagen, imagenFiltrada, 9, 75, 75);
            CvInvoke.Imshow("filtrado bilateral para reducir el ruido manteniendo bordes", imagen);
            CvInvoke.WaitKey(0);

            // Aplicamos el filtrado de mediana para eliminar detalles pequeños
            imagenFiltrada = imagen.SmoothMedian(5);
            CvInvoke.Imshow("filtrado de mediana para eliminar detalles pequeños", imagen);
            CvInvoke.WaitKey(0);

            // 4. Aplicamos la Umbralización Adaptativa para segmentar la imagen en regiones de iluminación uniforme
            CvInvoke.AdaptiveThreshold(imagen, imagen, 255, AdaptiveThresholdType.GaussianC, ThresholdType.Binary, 11, 4);
            CvInvoke.Imshow("la Umbralización Adaptativa ", imagen);
            CvInvoke.WaitKey(0);

            // 2. Aplicamos el Filtro de Media para eliminar el ruido manteniendo la estructura principal
            int kernelSize = 5;
            CvInvoke.Blur(imagen, imagen, new Size(kernelSize, kernelSize), new Point(-1, -1));
            CvInvoke.Imshow("Filtro de Media para eliminar el ruido manteniendo la estructura principal", imagen);
            CvInvoke.WaitKey(0);

            // 3. Aplicamos la Dilatación y Erosión para mejorar la continuidad de las líneas y eliminar puntos o segmentos
            kernelSize = 3;
            Mat kernel = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(kernelSize, kernelSize), new Point(-1, -1));
            CvInvoke.Dilate(imagen, imagen, kernel, new Point(-1, -1), 1, BorderType.Reflect, default(MCvScalar));
            CvInvoke.Erode(imagen, imagen, kernel, new Point(-1, -1), 1, BorderType.Reflect, default(MCvScalar));
            CvInvoke.Imshow("Dilatación y Erosión para mejorar la continuidad de las líneas y eliminar puntos o segmentos", imagen);
            CvInvoke.WaitKey(0);

            /*Opcion 1 muy buena */
            // 2. Aplicar el filtro Laplaciano para resaltar los bordes
            int kernelSizeFilterLaplaciano = 3; // Tamaño del kernel, puedes cambiar este valor (debe ser un número impar)
            double scaleFilterLaplaciano = 1;   // Valor de escala, puedes cambiar este valor
            CvInvoke.Laplacian(imagen, imagen, DepthType.Cv8U, kernelSizeFilterLaplaciano, scaleFilterLaplaciano, 0, BorderType.Default);
            CvInvoke.Imshow("Aplicar el filtro Laplaciano para resaltar los bordes", imagen);
            CvInvoke.WaitKey(0);

            // Aplicamos el filtrado de mediana para eliminar detalles pequeños
            imagenFiltrada = imagen.SmoothMedian(5);
            CvInvoke.Imshow("filtrado de mediana para eliminar detalles pequeños", imagen);
            CvInvoke.WaitKey(0);

            // 7. Aplicamos el operador Canny para detectar bordes
            Image<Gray, byte> bordes = imagen.Canny(100, 150);


            // 7. Aplicar un umbral adicional para eliminar detalles no deseados y resaltar bordes
            CvInvoke.Threshold(bordes, bordes, 50, 255, ThresholdType.Binary);
            CvInvoke.Imshow(" umbral adicional para eliminar detalles no deseados y resaltar bordes", imagen);
            CvInvoke.WaitKey(0);

            // 8. Aplicar dilatación y erosión adicionales para mejorar la continuidad de las líneas
            int kernelSizeDilEro = 3;
            Mat kernelDilEro = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(kernelSizeDilEro, kernelSizeDilEro), new Point(-1, -1));
            CvInvoke.Dilate(bordes, bordes, kernelDilEro, new Point(-1, -1), 1, BorderType.Reflect, default(MCvScalar));
            CvInvoke.Erode(bordes, bordes, kernelDilEro, new Point(-1, -1), 1, BorderType.Reflect, default(MCvScalar));
            CvInvoke.Imshow(" dilatación y erosión adicionales para mejorar la continuidad de las línea", imagen);
            CvInvoke.WaitKey(0);



            // 5. Utilizamos técnicas de umbralización para encontrar los contornos en la imagen
            VectorOfVectorOfPoint contornos = new VectorOfVectorOfPoint();
            Mat hierarchy = new Mat();
            CvInvoke.FindContours(imagen, contornos, hierarchy, RetrType.List, ChainApproxMethod.ChainApproxSimple);

            // 6. Analizamos los contornos para encontrar los puntos de las esquinas
            Image<Bgr, byte> imagenOriginal = new Image<Bgr, byte>(imagePath);
            for (int i = 0; i < contornos.Size; i++)
            {
                double epsilon = 0.03 * CvInvoke.ArcLength(contornos[i], true);
                VectorOfPoint approx = new VectorOfPoint();
                CvInvoke.ApproxPolyDP(contornos[i], approx, epsilon, true);

                if (approx.Size == 4) // Si el contorno tiene cuatro lados (cuadrilátero), probablemente sea una esquina
                {
                    Point[] corners = approx.ToArray();
                    foreach (Point corner in corners)
                    {
                        CvInvoke.Circle(imagenOriginal, corner, 5, new MCvScalar(0, 0, 255), -1);
                    }
                }
            }


            // 8. Aplicamos la transformación de Hough para detectar líneas en los bordes
            LineSegment2D[] lineas = CvInvoke.HoughLinesP(
                bordes,                    // Imagen con los bordes detectados
                1,                         // Resolución del parámetro rho
                Math.PI / 180,             // Resolución del parámetro theta (en radianes)
                100,                       // Umbral de detección (número mínimo de intersecciones)
                10,                        // Longitud mínima de línea aceptada (ajusta este valor según tus necesidades)
                10                         // Separación máxima entre píxeles para considerarlas como parte de la misma línea
            );

            // Dibujamos las líneas detectadas en la imagen original
            foreach (LineSegment2D linea in lineas)
            {
                imagenOriginal.Draw(linea, new Bgr(0, 255, 0), 2);
            }

            // Mostramos la imagen con los bordes de la tabla y las esquinas detectadas
            CvInvoke.Imshow("Bordes de la tabla y esquinas detectadas", imagenOriginal);
            CvInvoke.WaitKey(0);

            imagenOriginal.Save(imageOutPath);
        }
    }
}