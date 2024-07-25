// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using NameGenerator;

namespace Memento.Step2
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
            this._nameGenerator = nameGenerator;
            this._random = new Random();
        }

        public string GetNewName()
        {
            return this._nameGenerator.Generate();
        }

        public string GetNewSpecies()
        {
            return FishSpecies[this._random.Next( FishSpecies.Length )];
        }
    }
}