namespace ToilettenArbitrator.ToilettenWars
{
    internal class SilverDice
    {
        private Random Dice;
        private double minChange, maxChange, theShot;
        private float chance = 0.0f;

        public int GetCoordinate => new Random().Next(0, 200);
        public int D4 => new Random().Next(1, 5);
        public int D6 => new Random().Next(1, 7);
        public int D8 => new Random().Next(1, 9);
        public int D10 => new Random().Next(1, 11);
        public int D12 => new Random().Next(1, 13);
        public int D20 => new Random().Next(1, 21);
        public int D100 => new Random().Next(0, 101);
        public float HandDamage => new Random().Next(30, 81) / 100.0f;

        public SilverDice()
        {
            Dice = new Random();
        }

        public int JustRandom(int minValue, int maxValue)
        {
            int num = new int();

            if (minValue > maxValue)
            {
                num = Dice.Next(maxValue, minValue);
            }
            else
            {
                num = Dice.Next(minValue, maxValue);
            }
            return num;
        }

        

        public bool Luck(int Chance)
        {
            chance = (float)(Chance / 100.0f);

            maxChange = new Random().NextDouble();

            if (maxChange - chance < 0.0f) maxChange = chance + 0.02f;

            minChange = maxChange - chance;
            theShot = new Random().NextDouble();

            if (theShot > minChange && theShot < maxChange) return true;
            else return false;
        }
    }
}
