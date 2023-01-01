using System.Drawing;
using System.Drawing.Imaging;
using Classes;
using Classes.ScriptActions;
using Imager.Interface;
using BenchmarkDotNet.Attributes;


namespace Benchmarks
{

    [MemoryDiagnoser]
    [RankColumn]
    public class ImageFilter
    {
        //Props and Fiels :
        readonly ScriptEngine scriptEngine = new ScriptEngine();
        Image sourceImage;
        Image resultImage;

        public ImageFilter()
        {
            LoadImage("Images/test.bmp");
            ScaleImageAllMethods();
        }

        public void ScaleImageAllMethods()
        {
            var manipulators = SupportedManipulators.MANIPULATORS;

            // Stopwatch stopwatch = new Stopwatch();
            for (int i = 0; i < 10; i++)
            {
                //stopwatch.Restart();
                ScaleImage(i, false, 8);
                //Console.WriteLine("Scaling Using : " + manipulators[i].Key + " | Time: " + stopwatch.ElapsedMilliseconds + "ms");
            }
        }
        private void LoadImage(string fileName)
        {
            try
            {
                scriptEngine.ExecuteAction(new LoadFileCommand(fileName));
                sourceImage = scriptEngine.GdiSource;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Resources.txCouldNotLoadImage", fileName, exception.Message);
            }

        }

        private void SaveImage(string method)
        {
            var getDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            string invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalidChars)
                method = method.Replace(c.ToString(), "");


            string fileName = getDirectory + @"\Images\Results\" + method + ".png";

            Directory.CreateDirectory(getDirectory + @"\Images\Results");
            resultImage.Save(fileName, ImageFormat.Png);
        }

        private void ScaleImage(int methodIndex, bool saveImage = false, float factor = 4)
        {
            bool applyToTarget = false;
            string methodName = SupportedManipulators.MANIPULATORS[methodIndex].Key;
            IImageManipulator method = SupportedManipulators.MANIPULATORS[methodIndex].Value;
            ushort targetWidth = Convert.ToUInt16(factor * sourceImage.Width);
            ushort targetHeight = Convert.ToUInt16(factor * sourceImage.Height);
            bool maintainAspect = false;
            bool useThresholds = true;
            bool useCenteredGrid = false;
            byte repetitionCount = 1;
            OutOfBoundsMode horizontalBph = (OutOfBoundsMode)Enum.GetValues(typeof(OutOfBoundsMode)).GetValue(0);
            OutOfBoundsMode verticalBph = (OutOfBoundsMode)Enum.GetValues(typeof(OutOfBoundsMode)).GetValue(0);
            float radius = 1;

            if (targetWidth <= 0 && method.SupportsWidth || targetHeight <= 0 && method.SupportsHeight)
            {
                Console.WriteLine("Resources.txNeedWidthAndHeightAboveZero" + " " + "Resources.ttNeedWidthAndHeightAboveZero");
                return;
            }

            var command = new ResizeCommand(applyToTarget, method, targetWidth, targetHeight, 0, maintainAspect, horizontalBph, verticalBph, repetitionCount, useThresholds, useCenteredGrid, radius);

            scriptEngine.ExecuteAction(command);
            resultImage = scriptEngine.GdiTarget;

            if (saveImage)
                SaveImage(methodName);
        }

        #region MethodsBenchmark 

        public void NearestNeighborGDI() { ScaleImage(0, false, 4); }
        public void BilinearGDI() { ScaleImage(1, false, 4); }
        public void BicubicGDI() { ScaleImage(2, false, 4); }
        public void HighQualityBilinearGDI() { ScaleImage(3, false, 4); }
        public void HighQualityBicubicGDI() { ScaleImage(4, false, 4); }
        public void Rectangular() { ScaleImage(5, false, 4); }
        public void Bicubic() { ScaleImage(6, false, 4); }
        public void Schaum2() { ScaleImage(7, false, 4); }
        public void Schaum3() { ScaleImage(8, false, 4); }
        public void BSpline2() { ScaleImage(9, false, 4); }
        public void BSpline3() { ScaleImage(10, false, 4); }
        public void BSpline5() { ScaleImage(11, false, 4); }
        public void BSpline7() { ScaleImage(12, false, 4); }
        public void BSpline9() { ScaleImage(13, false, 4); }
        public void BSpline11() { ScaleImage(14, false, 4); }
        public void OMoms3() { ScaleImage(15, false, 4); }
        public void OMoms5() { ScaleImage(16, false, 4); }
        public void OMoms7() { ScaleImage(17, false, 4); }
        public void Triangular() { ScaleImage(18, false, 4); }
        public void Welch() { ScaleImage(19, false, 4); }
        public void Hann() { ScaleImage(20, false, 4); }
        public void Hamming() { ScaleImage(21, false, 4); }
        public void Blackman() { ScaleImage(22, false, 4); }
        public void Nuttal() { ScaleImage(23, false, 4); }
        public void BlackmanNuttal() { ScaleImage(24, false, 4); }
        public void BlackmanHarris() { ScaleImage(25, false, 4); }
        public void FlatTop() { ScaleImage(26, false, 4); }
        public void PowerOfCosine() { ScaleImage(27, false, 4); }
        public void Cosine() { ScaleImage(28, false, 4); }
        public void Gauss() { ScaleImage(29, false, 4); }
        public void Tukey() { ScaleImage(30, false, 4); }
        public void Poisson() { ScaleImage(31, false, 4); }
        public void BartlettHann() { ScaleImage(32, false, 4); }
        public void HanningPoisson() { ScaleImage(33, false, 4); }
        public void Bohman() { ScaleImage(34, false, 4); }
        public void Cauchy() { ScaleImage(35, false, 4); }
        public void Lanczos() { ScaleImage(36, false, 4); }

        public void Scanlines50Minus() { ScaleImage(37, false, 4); }
        public void Scanlines50Plus() { ScaleImage(38, false, 4); }
        public void Scanlines100Plus() { ScaleImage(39, false, 4); }
        public void VScanlines50Minus() { ScaleImage(40, false, 4); }
        public void VScanlines50Plus() { ScaleImage(41, false, 4); }
        public void VScanlines100Plus() { ScaleImage(42, false, 4); }
        public void MAMETV2x() { ScaleImage(43, false, 4); }
        public void MAMETV3x() { ScaleImage(44, false, 4); }
        public void MAMERGB2x() { ScaleImage(45, false, 4); }
        public void MAMERGB3x() { ScaleImage(46, false, 4); }
        public void HawkyntTV2x() { ScaleImage(47, false, 4); }
        public void HawkyntTV3x() { ScaleImage(48, false, 4); }
        public void BilinearPlusOriginal() { ScaleImage(49, false, 4); }
        public void BilinearPlus() { ScaleImage(50, false, 4); }
        public void Eagle2x() { ScaleImage(51, false, 4); }
        [Benchmark] public void Eagle3x() { ScaleImage(52, false, 4); }
        public void Eagle3xB() { ScaleImage(53, false, 4); }
        public void SuperEagle() { ScaleImage(54, false, 4); }
        public void SaI2x() { ScaleImage(55, false, 4); }
        public void SuperSaI() { ScaleImage(56, false, 4); }
        public void AdvInterp2x() { ScaleImage(57, false, 4); }
        public void AdvInterp3x() { ScaleImage(58, false, 4); }
        public void Scale2x() { ScaleImage(59, false, 4); }
        public void Scale3x() { ScaleImage(60, false, 4); }
        public void EPXB() { ScaleImage(61, false, 4); }
        public void EPXC() { ScaleImage(62, false, 4); }
        public void EPX3() { ScaleImage(63, false, 4); }
        public void ReverseAA() { ScaleImage(64, false, 4); }
        public void DES() { ScaleImage(65, false, 4); }
        public void DESII() { ScaleImage(66, false, 4); }
        public void SCL2x() { ScaleImage(67, false, 4); }
        public void Super2xSCL() { ScaleImage(68, false, 4); }
        public void Ultra2xSCL() { ScaleImage(69, false, 4); }
        public void XBR2xNoBlend() { ScaleImage(70, false, 4); }
        public void XBR3xNoBlend() { ScaleImage(71, false, 4); }
        public void XBR3xmodifiedNoBlend() { ScaleImage(72, false, 4); }
        public void XBR4xNoBlend() { ScaleImage(73, false, 4); }
        public void XBR2x() { ScaleImage(74, false, 4); }
        public void XBR3x() { ScaleImage(75, false, 4); }
        public void XBR3xmodified() { ScaleImage(76, false, 4); }
        public void XBR4x() { ScaleImage(77, false, 4); }
        public void XBR5xlegacy() { ScaleImage(78, false, 4); }
        public void XBRz2x() { ScaleImage(79, false, 4); }
        public void XBRz3x() { ScaleImage(80, false, 4); }
        public void XBRz4x() { ScaleImage(81, false, 4); }
        public void XBRz5x() { ScaleImage(82, false, 4); }
        public void HQ2x() { ScaleImage(83, false, 4); }
        public void HQ2xBold() { ScaleImage(84, false, 4); }
        public void HQ2xSmart() { ScaleImage(85, false, 4); }
        public void HQ2x3() { ScaleImage(86, false, 4); }
        public void HQ2x3Bold() { ScaleImage(87, false, 4); }
        public void HQ2x3Smart() { ScaleImage(88, false, 4); }
        public void HQ2x4() { ScaleImage(89, false, 4); }
        public void HQ2x4Bold() { ScaleImage(90, false, 4); }
        public void HQ2x4Smart() { ScaleImage(91, false, 4); }
        public void HQ3x() { ScaleImage(92, false, 4); }
        public void HQ3xBold() { ScaleImage(93, false, 4); }
        public void HQ3xSmart() { ScaleImage(94, false, 4); }
        public void HQ4x() { ScaleImage(95, false, 4); }
        public void HQ4xBold() { ScaleImage(96, false, 4); }
        public void HQ4xSmart() { ScaleImage(97, false, 4); }
        public void LQ2x() { ScaleImage(98, false, 4); }
        public void LQ2xBold() { ScaleImage(99, false, 4); }
        public void LQ2xSmart() { ScaleImage(100, false, 4); }
        public void LQ2x3() { ScaleImage(101, false, 4); }
        public void LQ2x3Bold() { ScaleImage(102, false, 4); }
        public void LQ2x3Smart() { ScaleImage(103, false, 4); }
        public void LQ2x4() { ScaleImage(104, false, 4); }
        public void LQ2x4Bold() { ScaleImage(105, false, 4); }
        public void LQ2x4Smart() { ScaleImage(106, false, 4); }
        public void LQ3x() { ScaleImage(107, false, 4); }
        public void LQ3xBold() { ScaleImage(108, false, 4); }
        public void LQ3xSmart() { ScaleImage(109, false, 4); }
        public void LQ4x() { ScaleImage(110, false, 4); }
        public void LQ4xBold() { ScaleImage(111, false, 4); }
        public void LQ4xSmart() { ScaleImage(112, false, 4); }
        public void Red() { ScaleImage(113, false, 4); }
        public void Green() { ScaleImage(114, false, 4); }
        public void Blue() { ScaleImage(115, false, 4); }
        public void Alpha() { ScaleImage(116, false, 4); }
        public void Luminance() { ScaleImage(117, false, 4); }
        public void ChrominanceU() { ScaleImage(118, false, 4); }
        public void ChrominanceV() { ScaleImage(119, false, 4); }
        public void u() { ScaleImage(120, false, 4); }
        public void v() { ScaleImage(121, false, 4); }
        public void Hue() { ScaleImage(122, false, 4); }
        public void HueColored() { ScaleImage(123, false, 4); }
        public void Brightness() { ScaleImage(124, false, 4); }
        public void Min() { ScaleImage(125, false, 4); }
        public void Max() { ScaleImage(126, false, 4); }
        public void ExtractColors() { ScaleImage(127, false, 4); }
        public void ExtractDeltas() { ScaleImage(128, false, 4); }


        #endregion


}
}
