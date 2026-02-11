public class Word
{
    public string Value { get; }

    public Word(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be empty.");
        }

        if (value.Length != 5)
        {
            throw new ArgumentException("Word must be exactly 5 characters.");
        }

        Value = value;
    }

    public override string ToString() => Value;
}