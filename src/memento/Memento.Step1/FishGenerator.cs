using NameGenerator;

namespace Memento.Step1
{
    public sealed class FishGenerator : IFishGenerator
    {
        private static readonly string[] FishSpecies =
        [
            "Clownfish",
            "Damselfish",
            "Dottyback",
            "Fairy Basslet",
            "Goby",
            "Hawkfish",
            "Jawfish",
            "Lionfish",
            "Mandarin Dragonet",
            "Neon Goby",
            "Pseudochromis",
            "Royal Gramma",
            "Tang",
            "Wrasse",
            "Scuba Diver"
        ];

        private readonly GeneratorBase _nameGenerator;
        private readonly Random _random;

        public FishGenerator( GeneratorBase nameGenerator )
        {
            _nameGenerator = nameGenerator;
            _random = new Random();
        }

        public string GetNewName()
        {
            return _nameGenerator.Generate();
        }

        public string GetNewSpecies()
        {
            return FishSpecies[_random.Next( FishSpecies.Length )];
        }
    }
}