namespace Core.Basic
{
    public class Counter
    {
        public int Value { get; private set; }

        public void Increase() => Value++;
        public void Decrease() => Value--;
        public bool IsEmpty => Value == 0;
    }
}
