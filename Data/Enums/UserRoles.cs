namespace Data.Enums;

public static class UserRoles
{
    public const string Admin = "Admin";
    public const string Docente = "Docente";
    public const string Estudiante = "Estudiante";

    public static readonly string[] All = [Admin, Docente, Estudiante];
}
