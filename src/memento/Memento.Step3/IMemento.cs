namespace Memento.Step3;

public interface IMemento
{
    IMementoable Originator { get; }
    DateTime MementoTime { get; }
}