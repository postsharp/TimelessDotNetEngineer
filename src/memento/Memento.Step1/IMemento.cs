namespace Memento.Step1;

public interface IMemento
{
    IMementoable Originator { get; }
}