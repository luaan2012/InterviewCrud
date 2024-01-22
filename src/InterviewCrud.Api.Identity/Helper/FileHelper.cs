namespace InterviewCrud.Api.Identity.Helper
{
    public class FileHelper
    {
        public static bool SaveBase64Image(string base64String, string folderPath, string fileName)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                byte[] imageBytes = Convert.FromBase64String(base64String);

                var appBasePath = AppContext.BaseDirectory;
                string filePath = Path.Combine(appBasePath, folderPath, fileName);

                File.WriteAllBytes(filePath, imageBytes);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DeleteImage(string folderPath, string fileName)
        {
            try
            {
                string filePath = Path.Combine(folderPath, fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }

                return false; 
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
