using System;
using System.Collections.Generic;

namespace InstaBot.Utils
{
    public class RandomGenerator
    {
        private readonly List<int> numbers;
        private readonly Random random;
        private readonly int maxValue;

        public RandomGenerator(int maxValue)
        {
            this.maxValue = maxValue;
            numbers = new List<int>();
            random = new Random();
        }

        public int Different()
        {
            int num = random.Next(maxValue);
            if (numbers.Contains(num))
            {
                return Different();
            }
            else
            {
                numbers.Add(num);
                return num;
            }

        }
    }
}
