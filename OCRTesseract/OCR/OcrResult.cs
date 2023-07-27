using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Tesseract;


namespace OCRTesseract.OCR
{

    internal class OcrResult
    {
        private const string tessDataPath = @"C:\Program Files (x86)\Tesseract-OCR\tessdata";

        // Umbral de confianza para aceptar una palabra como válida
        private const int confidenceThreshold = 50;

        public string Text { get; set; }
        public Rectangle[] Regions { get; set; }

        public OcrResult PerformOcr(string imagePath)
        {
            // Configurar el motor Tesseract con el idioma "spa" (español) y el modo por defecto
            using (var engine = new TesseractEngine(tessDataPath, "spa", EngineMode.Default))
            {
                using (var img = Pix.LoadFromFile(imagePath))
                {
                    using (var page = engine.Process(img))
                    {
                        var text = page.GetText();

                        // Lista para almacenar las coordenadas de las palabras detectadas
                        List<Rectangle> rectangles = new List<Rectangle>();

                        using (var iter = page.GetIterator())
                        {
                            // Establecer el nivel del iterador en palabras (words)
                            iter.Begin();

                            do
                            {
                                Rect bounds;

                                // Obtener las coordenadas del rectángulo que rodea la palabra
                                if (iter.TryGetBoundingBox(PageIteratorLevel.Word, out bounds))
                                {
                                    // Obtener la palabra actual
                                    string word = iter.GetText(PageIteratorLevel.Word);

                                    // Obtener la confianza de la palabra
                                    int confidence = (int)iter.GetConfidence(PageIteratorLevel.Word);

                                    // Verificar si la palabra supera el umbral de confianza
                                    if (confidence >= confidenceThreshold)
                                    {
                                        // Valida que la palabra no sea NUll o contenga espacios
                                        if (!string.IsNullOrEmpty(word.Trim()))
                                        {
                                            rectangles.Add(new Rectangle(bounds.X1, bounds.Y1, bounds.X2 - bounds.X1, bounds.Y2 - bounds.Y1));
                                            Console.WriteLine($" Palabra encontrada = {word}");
                                        }
                                    }
                                }

                            } while (iter.Next(PageIteratorLevel.Word)); // Moverse a la siguiente palabra
                        }

                        return new OcrResult
                        {
                            Text = text,
                            Regions = rectangles.ToArray()
                        };
                    }
                }
            }
        }
    }
}
