namespace MaksiKo.Shared.Common.Models;

public class IdNamePair : IdNamePair<int, string>
{
    public IdNamePair(){}
    public IdNamePair(int id, string name) : base(id, name)
    {
    }
}

public class IdNamePair<T, U>
{
    public T Id { get; set; }
    public U Name { get; set; }

    public IdNamePair(){}
    public IdNamePair(T id, U name)
    {
        Id = id;
        Name = name;
    }
}