public class WordRepository : IWordRepository
{
    private readonly List<Word> _words;
    public WordRepository()
    {
        _words = loadWords();
    }

    private List<Word> loadWords()
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "WordleApi", "Data", "wordle_ord.txt");

        return File.ReadLines(filePath)
            .Where(line => !String.IsNullOrWhiteSpace(line))
            .Where(line => !line.StartsWith("#"))
            .Select((line, Index) => new Word(line.Trim().ToString()))
            .ToList();
    }

    public IReadOnlyList<Word> GetAll()
    {
        return _words;
    }
}