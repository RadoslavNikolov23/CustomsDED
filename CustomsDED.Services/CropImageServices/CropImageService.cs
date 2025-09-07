namespace CustomsDED.Services.CropImageServices
{
    using SkiaSharp;

    using CustomsDED.Common.Helpers;

    public static class CropImageService
    {
        public static async Task<byte[]> CropToOverlayAsync(byte[] photoBytes)
        {
            try
            {
                using var ms = new MemoryStream(photoBytes);
                using var bitmap = SKBitmap.Decode(ms);

                if (bitmap == null)
                    throw new Exception("Failed to decode image.");

                int originalWidth = bitmap.Width;
                int originalHeight = bitmap.Height;

                int centerWidth = originalWidth / 2;

                int cropHeightTop = (int)(originalHeight * 0.1);
                int cropHeightBottom = (int)(originalHeight - cropHeightTop);

                int cropWidthLeft = (int)(centerWidth * 0.8);
                int cropWidthRight = (int)(originalWidth - cropWidthLeft);


                var cropRect = new SKRectI(cropWidthLeft, cropHeightTop, cropWidthRight, cropHeightBottom);

                // Crop
                using var cropped = new SKBitmap(cropRect.Width, cropRect.Height);

                using (var canvas = new SKCanvas(cropped))
                {
                    // Draw the source bitmap's cropRect into the full cropped bitmap
                    canvas.DrawBitmap(bitmap, cropRect, new SKRect(0, 0, cropped.Width, cropped.Height));
                    canvas.Flush();
                }

                // Encode back to JPEG
                using var image = SKImage.FromBitmap(cropped);
                using var encoded = image.Encode(SKEncodedImageFormat.Jpeg, 90);

                return encoded.ToArray();
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in CropToOverlayAsync, in the CropImageService class.");
                throw;
            }

        }
    }
}
