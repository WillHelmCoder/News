using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Web;

namespace Dal.Attributes
{
    public class ValidImage : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;

            if (file == null)
            {
                return true;
            }

            if (file.ContentLength > (1024 * 1024 * 1.6))
            {
                ErrorMessage = "Tamaño maximo: 1.6 Megas";

                return false;
            }

            try
            {
                using (var img = Image.FromStream(file.InputStream))
                {
                    switch (file.ContentType)
                    {
                        //case "image/jpeg": return img.RawFormat.Guid.Equals(ImageFormat.Jpeg.Guid);                            
                        //case "image/png": return img.RawFormat.Guid.Equals(ImageFormat.Png.Guid);
                        case "image/jpeg": return true;
                        case "image/png": return true;
                        default:
                            ErrorMessage = "Formato inválido. Debe ser jpg o png";
                            return false;
                    }
                }
            }
            catch
            {
                ErrorMessage = "Hubo un error al subir la imagen. Intentalo de nuevo.";
            }

            return false;
        }
    }
}