namespace Memento.Step2;

public interface IMemento
{
    IMementoable Originator { get; }
}