using Newtonsoft.Json;

namespace WebAppWiki.Helpers
{
    public static class SessionExtension
    {
        public static void SaveObject<T>(this ISession session, T something) where T : class, new()
        {
            string jsonString=JsonConvert.SerializeObject(something);

            string key=something.GetType().Name;
            session.SetString(key, jsonString);
        }

        public static T? LoadObject<T>(this ISession session, bool isCreateNew = true) where T : class, new()
        {
            string key = typeof(T).Name;
            string? jsonString = session.GetString(key);

            if (jsonString == null)
            {
                return isCreateNew ? Activator.CreateInstance<T>() : null;
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(jsonString);    
            }
        }
    }
}
