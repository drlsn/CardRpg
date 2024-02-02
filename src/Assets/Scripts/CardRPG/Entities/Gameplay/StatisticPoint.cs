using Core.Collections;
using System.Collections.Generic;
using System.Linq;

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

        public StatisticPoint DeepCopy()
        {
            var point = new StatisticPoint(OriginalValue);
            
            point.Modifiers = Modifiers.DeepCopy().ToList();
            point.ModifiersLate = ModifiersLate.DeepCopy().ToList();

            return point;
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

        public void ModifyClamped(double value, out double clampedValue)
        {
            clampedValue = value;

            var newValue = CalculatedValue + value;
            if (newValue < 0)
                clampedValue -= newValue;

            if (clampedValue == 0)
                return;

            Modifiers.Add(new(clampedValue));
        }

        public void ModifyClamped(double value) =>
            ModifyClamped(value, out var clampedValue);

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

        public StatisticPointModifier DeepCopy() =>
            (StatisticPointModifier) MemberwiseClone();
    }

    public static class StatisticPointModifierExtensions
    {
        public static StatisticPointModifier[] DeepCopy(this IEnumerable<StatisticPointModifier> items) =>
            items.Select(item => item.DeepCopy()).ToArray();
    }
}
