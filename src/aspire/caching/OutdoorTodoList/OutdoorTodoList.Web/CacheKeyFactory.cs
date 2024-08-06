namespace OutdoorTodoList.Web;

public static class CacheKeyFactory
{
    public static string GetWeather(string endpoint) =>  $"{nameof(GetWeather)}({endpoint})";
    public static string GetToDo(string endpoint, int id) =>  $"{nameof(GetToDo)}({endpoint}, {id})";
    
    public static string GetToDoList(string endpoint) =>  $"{nameof(GetToDo)}({endpoint})";
}