namespace GiftManagement_Version2.Helpers
{
    public class ImageHelper
    {
        public static async Task<string> SaveImageAsync(IFormFile imageFile, string uploadDirectory)
        {
            string relativePath = $"/images/";
            if (imageFile == null || imageFile.Length == 0)
            {
                return null;
            }

            string fileExtension = Path.GetExtension(imageFile.FileName).ToLower();

            string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            if (!allowedExtensions.Contains(fileExtension))
            {
                return null;
            }

            string newFileName = $"{Path.GetFileNameWithoutExtension(imageFile.FileName)}_{DateTime.UtcNow.Ticks}{fileExtension}";

            string filePath = Path.Combine(uploadDirectory, newFileName);

            using (FileStream stream = new(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return relativePath + newFileName;
        }
    }
}
