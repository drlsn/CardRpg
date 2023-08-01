using Core;
using System.Collections.Generic;

namespace CardRPG.Entities.Gameplay
{
    public class StatisticPoint
    {
        public int OriginalValue { get; }
        public List<StatisticPointModifier> Modifiers { get; private set; } = new();
        public List<StatisticPointModifier> ModifiersLate { get; private set; } = new();

        public StatisticPoint(int value)
        {
            OriginalValue = value;
        }

        public int CalculatedValue {
            get {
                var value = OriginalValue;

                Modifiers.ForEach(m =>
                    value = m.IsFactor ? value = (int)(value * m.Value) : (int)(value + m.Value)
                );

                ModifiersLate.ForEach(m =>
                    value = m.IsFactor ? value = (int)(value * m.Value) : (int)(value + m.Value)
                );

                return value;
            }
        }

        public void ModifyClamped(double value)
        {
            var currentValue = CalculatedValue;
            value = value.Clamp(-currentValue, OriginalValue - currentValue);
            if (value == 0)
                return;

            Modifiers.Add(new(value));
        }

        public void Modify(double value, bool isFactor = false) =>
            Modifiers.Add(new(value, isFactor));

        public void Modify(double value, string id, bool isFactor = false) =>
            Modifiers.Add(new(value, isFactor, id));

        public void ModifyLate(double value, string id, bool isFactor = false) =>
            ModifiersLate.Add(new(value, isFactor, id));

        public void RemoveAll(string id)
        {
            Modifiers.RemoveIf(v => v.Id == id);
            ModifiersLate.RemoveIf(v => v.Id == id);
        }

        public void RemoveAllWithId()
        {
            Modifiers.RemoveIf(v => !v.Id.IsNullOrEmpty());
            ModifiersLate.RemoveIf(v => !v.Id.IsNullOrEmpty());
        }

        public void RemoveAll()
        {
            Modifiers = new();
            ModifiersLate = new();
        }

        public static implicit operator StatisticPoint(int value) => new(value);
    }

    public class StatisticPointModifier
    {
        public StatisticPointModifier(double value, bool isFactor = false, string id = null)
        {
            Value = value;
            IsFactor = isFactor;
            Id = id;
        }

        public double Value { get; }
        public bool IsFactor { get; }
        public string Id { get; }
    }
}
