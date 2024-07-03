namespace CleanArchitecture.Domain.Shared;

public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>
    where TEnum : Enumeration<TEnum> // Restricción genérica para asegurar que TEnum es una subclase de Enumeration<TEnum>
{
    private static readonly Dictionary<int, TEnum> Enumerations = CreateEnumerations();
    public int Id { get; protected init; }
    public string Name { get; protected init; } = string.Empty;

    public Enumeration(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public static TEnum? FromValue(int id)
    {
        return Enumerations.TryGetValue(id, out TEnum? enumeration) ? enumeration : default;
    }

    public static TEnum? FromName(string name)
    {
        return Enumerations.Values.SingleOrDefault(x => x.Name == name);
    }

    public static List<TEnum> GetValues()
    {
        return Enumerations.Values.ToList();
    }

    public bool Equals(Enumeration<TEnum>? other)
    {
        if (other is null)
        {
            return false;
        }

        return GetType() == other.GetType() && Id == other.Id;
    }

    /// <summary>
    /// Sobrecarga del método Equals para comparar objetos de tipo 'object'
    /// </summary>
    public override bool Equals(object? obj)
    {
        return obj is Enumeration<TEnum> other && Equals(other);
    }

    /// <summary>
    /// Sobrecarga del método GetHashCode para retornar el hash del Id
    /// </summary>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString()
    {
        return Name;
    }

    private static Dictionary<int, TEnum> CreateEnumerations()
    {
        var enumerationType = typeof(TEnum);

        // Obtiene todos los campos estáticos públicos y jerárquicos que son del tipo de enumeración
        var fieldsForType = enumerationType
            .GetFields(
                System.Reflection.BindingFlags.Public
                    | System.Reflection.BindingFlags.Static
                    | System.Reflection.BindingFlags.FlattenHierarchy
            )
            .Where(fieldInfo => enumerationType.IsAssignableFrom(fieldInfo.FieldType))
            .Select(fieldInfo => (TEnum)fieldInfo.GetValue(default)!);

        return fieldsForType.ToDictionary(x => x.Id);
    }
}
