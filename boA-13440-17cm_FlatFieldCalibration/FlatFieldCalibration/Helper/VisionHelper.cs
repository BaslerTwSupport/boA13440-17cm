using OpenCvSharp;
namespace FlatFieldCalibration.Helper;

public static class VisionHelper
{
    /// <summary>
    /// Flat Field Correction for VisualApplets.
    /// </summary>
    /// <param name="imgDark"></param>
    /// <param name="imgWhite"></param>
    /// <returns></returns>
    public static float[] FFC(string imgDark, string imgWhite)
    {
        var maxGain = 4095.0;
        using var dark = new Mat(imgDark, ImreadModes.Grayscale);
        using var white = new Mat(imgWhite, ImreadModes.Grayscale);
        using Mat mask1 = new Mat(dark.Size(), MatType.CV_8U);
        using Mat value1 = new Mat(dark.Size(), MatType.CV_32F, new Scalar(1));
        white.ConvertTo(white, MatType.CV_32F);
        dark.ConvertTo(dark, MatType.CV_32F);
        using Mat diff = white - dark;

        #region diff <= 0 = 1
        Cv2.Threshold(diff, mask1, 1, 255, ThresholdTypes.BinaryInv);
        mask1.ConvertTo(mask1, MatType.CV_8U);
        Cv2.CopyTo(value1, diff, mask1);
        #endregion

        #region Divide targetvalue by digg image value
        var centerFirst = Math.Floor(Convert.ToDouble(diff.Height) / 2);
        var centerSecond = Math.Floor(Convert.ToDouble(diff.Width) / 2);
        var roiSize = diff.Height * 0.75;
        var roiSizeHalf = Convert.ToInt32(Math.Floor(roiSize / 2));
        #endregion

        #region gain calculation
        using Mat roi = new Mat(diff, new Rect(Convert.ToInt32(centerSecond - roiSizeHalf), Convert.ToInt32(centerFirst - roiSizeHalf), Convert.ToInt32(roiSize), Convert.ToInt32(roiSize)));
        var mean = roi.Mean().Val0;
        using Mat gain = mean / diff;
        using Mat gainRaw = gain * 256;
        using Mat value2 = new Mat(dark.Size(), MatType.CV_32F, new Scalar(maxGain));
        using Mat mask2 = new Mat(dark.Size(), MatType.CV_8U);
        Cv2.Threshold(gainRaw, mask2, 0, maxGain, ThresholdTypes.BinaryInv);
        mask2.ConvertTo(mask2, MatType.CV_8U);
        Cv2.CopyTo(value2, gainRaw, mask2);
        #endregion

        #region offset calculation
        using Mat offsetRaw = dark * Math.Pow(2, 24);
        #endregion

        using Mat corrMatrix = offsetRaw + gainRaw;
        corrMatrix.GetArray(out float[] floats);
        return floats;
    }
}
