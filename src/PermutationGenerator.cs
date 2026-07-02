#nullable enable
namespace BringMIPHome.Simulation
{
    using System;
    using System.Collections.Generic;

    public class PermutationGenerator
    {
        private readonly Random random;
        private readonly int numberOfRoles;
        private readonly int[][] all;

        public PermutationGenerator(Random random, int numberOfRoles)
        {
            this.random = random;
            this.numberOfRoles = numberOfRoles;
            this.all = GetCombinations(numberOfRoles);
        }
        
        public int[] GetRandomCombination()
        {
            if (this.all.Length == 0)
            {
                throw new InvalidOperationException("No combinations available.");
            }
         
            var index = this.random.Next(this.all.Length);
            return this.all[index];
        }

        public int[] GetRandomFeasibleReassignment(int[] state, int newZeroPosition)
        {
            var feasibleReassignments = this.GetFeasibleReassignments(state, newZeroPosition);
            if (feasibleReassignments.Length == 0)
            {
                throw new InvalidOperationException("No feasible reassignments available.");
            }

            var index = this.random.Next(feasibleReassignments.Length);
            return feasibleReassignments[index];
        }

        public int[][] GetFeasibleReassignments(int[] state, int newZeroPosition)
        {
            if (state.Length != this.numberOfRoles)
            {
                throw new ArgumentException($"State length must be equal to numberOfRoles ({this.numberOfRoles}).");
            }

            var list = new List<int[]>();

            foreach (var combination in this.all)
            {
                var good = true;

                for (var i = 0; i < state.Length; i++)
                {
                    if (i == newZeroPosition)
                    {
                        if (combination[newZeroPosition] != 0)
                        {
                            //ignore this combination because it does not have 0 in the newZeroPosition
                            good = false;
                            break;
                        }
                    }
                    else
                    {
                        if (state[i] == combination[i])
                        {
                            //ignore this combination because the state is the same in this position
                            good = false;
                            break;
                        }
                    }
                }

                if (good)
                {
                    list.Add(combination);
                }
            }

            return list.ToArray();
        }


        // Roles 0..numberOfRoles-1
        public static int[][] GetCombinations(int numberOfRoles)
        {
            if (numberOfRoles <= 0)
            {
                return Array.Empty<int[]>();
            }

            var resultList = new List<int[]>();
            var currentPermutation = new int[numberOfRoles];
            var used = new bool[numberOfRoles];

            GeneratePermutations(0, numberOfRoles, currentPermutation, used, resultList);

            return resultList.ToArray();
        }

        private static void GeneratePermutations(int position, int numberOfRoles, int[] current, bool[] used, List<int[]> resultList)
        {
            // Base case: A complete permutation of all roles has been built
            if (position == numberOfRoles)
            {
                var clone = new int[numberOfRoles];
                Array.Copy(current, clone, numberOfRoles);
                resultList.Add(clone);
                return;
            }

            // Try placing every available role at the current position
            for (var role = 0; role < numberOfRoles; role++)
            {
                if (!used[role])
                {
                    used[role] = true;
                    current[position] = role;

                    // Move to the next position
                    GeneratePermutations(position + 1, numberOfRoles, current, used, resultList);

                    // Backtrack: free up the role for other combinations
                    used[role] = false;
                }
            }
        }
    }
}