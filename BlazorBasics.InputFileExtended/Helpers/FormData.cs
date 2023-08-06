using System.Reflection;

namespace BlazorBasics.InputFileExtended.Helpers;

/// <summary>
/// Class with helpers about the form data
/// </summary>
public class FormData
{
    /// <summary>
    /// Get a object and encaprulate into a MultipartFormDataContent
    /// </summary>
    /// <typeparam name="TModel">Class with properties is not other class or list</typeparam>
    /// <param name="data">Object with all properties like a data not lists or class</param>
    /// <returns></returns>
    public static MultipartFormDataContent SetMultipartFormDataContent<TModel>(TModel data)
    {
        MultipartFormDataContent formData = new MultipartFormDataContent();
        Type t = data.GetType();
        PropertyInfo[] properties = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);      //get properties only
        int c = properties.Length;
        for(int i = 0; i < c; i++)
        {
            try
            {
                object toSend = properties[i].GetValue(data);
                if(toSend != null)
                {
                    if(properties[i].PropertyType.Name == nameof(DateTime)) formData.Add(new StringContent(Convert.ToDateTime(toSend).ToString("yyyy/MM/dd HH:mm:ss")), properties[i].Name);
                    else formData.Add(new StringContent(toSend.ToString()), properties[i].Name);
                }
            }
            catch { }
        }
        return formData;
    }


}
