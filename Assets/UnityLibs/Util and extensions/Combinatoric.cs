//By Dominic Brodeur-Gendron
using System;
using System.Collections;
using System.Collections.Generic;


class Combinatorics<T>
{
    #region public
    public static CombinatoiricSet<T> GetCombination(List<T> set, int numberToPick)
    {
        return InternalGetCombination(set, numberToPick, null, null);
    }

    public static CombinatoiricSet<T> GetCombination(List<T> set, int numberToPick, Func<List<T>, bool> listValidation)
    {
        return InternalGetCombination(set, numberToPick, listValidation, null);
    }

    public static CombinatoiricSet<T> GetCombination(List<T> set, int numberToPick, Func<T, bool> validation)
    {
        return InternalGetCombination(set, numberToPick, null, validation);
    }

    public static CombinatoiricSet<T> GetCombination(List<T> set, int numberToPick, Func<List<T>, bool> listValidation, Func<T, bool> validation)
    {
        return InternalGetCombination(set, numberToPick, listValidation, validation);
    }

    public static CombinatoiricSet<T> GetAllCombinations(List<T> set, int numberToPick)
    {
        return InternalGetAllCombinaisons(set, numberToPick, null, null);
    }

    public static CombinatoiricSet<T> GetAllCombinations(List<T> set, int numberToPick, Func<T, bool> validation)
    {
        return InternalGetAllCombinaisons(set, numberToPick, null, validation);
    }

    /// <summary>
    /// ex : CombinatoiricSet<Card> cardSumLowerThan10 = Combinatorics<Card>.GetAllCombinations(cards, list => list.Sum( card => card.manaCost) < 10);
    /// </summary>
    public static CombinatoiricSet<T> GetAllCombinations(List<T> set, int numberToPick, Func<List<T>, bool> listValidation)
    {
        return InternalGetAllCombinaisons(set, numberToPick, listValidation, null);
    }

    /// <summary>
    /// ex : CombinatoiricSet<Card> cardSumLowerThan10NoKing = Combinatorics<Card>.GetAllCombinations(cards, list => list.Sum(card => card.manaCost) < 10, card => card.name != "king");
    /// </summary>
    public static CombinatoiricSet<T> GetAllCombinations(List<T> set, int numberToPick, Func<List<T>, bool> listValidation, Func<T, bool> validation)
    {
        return InternalGetAllCombinaisons(set, numberToPick, listValidation, validation);
    }

    public static CombinatoiricSet<T> GetAllCombinations(List<T> set)
    {
        return InternalGetAllCombinaisons(set, set.Count, null, null);
    }

    public static CombinatoiricSet<T> GetAllCombinations(List<T> set, Func<List<T>, bool> listValidation)
    {
        return InternalGetAllCombinaisons(set, set.Count, listValidation, null);
    }

    public static CombinatoiricSet<T> GetAllCombinations(List<T> set, Func<T, bool> validation)
    {
        return InternalGetAllCombinaisons(set, set.Count, null, validation);
    }

    public static CombinatoiricSet<T> GetAllCombinations(List<T> set, Func<List<T>, bool> listValidation, Func<T, bool> validation)
    {
        return InternalGetAllCombinaisons(set, set.Count, listValidation, validation);
    }

    public static CombinatoiricSet<T> GetPermutation(List<T> set)
    {
        return InternalGetPermutation(set);
    }
    public static CombinatoiricSet<T> GetPermutation(List<T> set, Func<List<T>, bool> listValidation)
    {
        return InternalGetPermutation(set, listValidation);
    }

    public static CombinatoiricSet<T> GetArrangement(List<T> set, int numberToPick)
    {
        return InternalGetArrangement(set, numberToPick, null);
    }
    public static CombinatoiricSet<T> GetArrangement(List<T> set, int numberToPick, Func<List<T>, bool> listValidation)
    {
        return InternalGetArrangement(set, numberToPick, listValidation);
    }

    public static CombinatoiricSet<T> GetAllArrangements(List<T> set)
    {
        return InternalGetAllArrangements(set, null);
    }

    public static CombinatoiricSet<T> GetAllArrangements(List<T> set, Func<List<T>, bool> listValidation)
    {
        return InternalGetAllArrangements(set, listValidation);
    }
    #endregion

    #region internal
    static CombinatoiricSet<T> InternalGetCombination(List<T> set, int numberToPick, Func<List<T>, bool> listValidation = null, Func<T, bool> validation = null)
    {
        List<List<T>> output = new List<List<T>>();
        GetCombinationRecursif(set, numberToPick, 0, new List<T>(), output, listValidation, validation);
        return new CombinatoiricSet<T>(output);
    }

    static CombinatoiricSet<T> InternalGetAllCombinaisons(List<T> set, int numberToPick, Func<List<T>, bool> listValidation = null, Func<T, bool> validation = null)
    {
        List<List<T>> output = new List<List<T>>();
        for (int i = 1; i <= numberToPick; i++)
        {
            List<List<T>> tempOutput = new List<List<T>>();
            GetCombinationRecursif(set, i, 0, new List<T>(), tempOutput, listValidation, validation);
            output.AddRange(tempOutput);
        }
        return new CombinatoiricSet<T>(output);
    }

    static CombinatoiricSet<T> InternalGetPermutation(List<T> set, Func<List<T>, bool> listValidation = null)
    {
        List<List<T>> output = new List<List<T>>();
        bool[] currentlPicked = new bool[set.Count];
        GetPermutationRecursif(set, currentlPicked, set.Count, new List<T>(), output, listValidation);
        return new CombinatoiricSet<T>(output);
    }

    static CombinatoiricSet<T> InternalGetArrangement(List<T> set, int numberToPick, Func<List<T>, bool> listvalidation = null)
    {
        List<List<T>> output = new List<List<T>>();
        bool[] currentlPicked = new bool[set.Count];
        GetPermutationRecursif(set, currentlPicked, numberToPick, new List<T>(), output, listvalidation);
        return new CombinatoiricSet<T>(output);
    }

    static CombinatoiricSet<T> InternalGetAllArrangements(List<T> set, Func<List<T>, bool> validation = null)
    {
        List<List<T>> output = new List<List<T>>();

        for (int i = 1; i <= set.Count; i++)
        {
            List<List<T>> tempOutput = new List<List<T>>();
            bool[] currentlPicked = new bool[set.Count];
            GetPermutationRecursif(set, currentlPicked, i, new List<T>(), tempOutput, validation);
            output.AddRange(tempOutput);
        }
        return new CombinatoiricSet<T>(output);
    }
    #endregion

    #region algorithms
    static void GetCombinationRecursif(List<T> set, int numberToPick, int startIndex, List<T> currentList, List<List<T>> output, Func<List<T>, bool> listValidation, Func<T, bool> validation)
    {
        if (currentList.Count == numberToPick)
        {
            if (listValidation == null || listValidation(currentList))
                output.Add(currentList);
            return;
        }

        for (int i = startIndex; i < set.Count; i++)
        {
            if (validation != null && !validation(set[i]))
                continue;

            List<T> newList = new List<T>(currentList);
            newList.Add(set[i]);

            GetCombinationRecursif(set, numberToPick, i + 1, newList, output, listValidation, validation);
        }
    }
    static void GetPermutationRecursif(List<T> set, bool[] currentlPicked, int depth, List<T> currentList, List<List<T>> output, Func<List<T>, bool> validation = null)
    {
        if (depth == 0)
        {
            if (validation == null || validation(currentList))
                output.Add(currentList);
            return;
        }

        for (int i = 0; i < set.Count; i++)
        {
            if (currentlPicked[i])
                continue;

            bool[] newCurrentlPicked = new bool[currentlPicked.Length];
            currentlPicked.CopyTo(newCurrentlPicked, 0);
            newCurrentlPicked[i] = true;

            List<T> newList = new List<T>(currentList);
            newList.Add(set[i]);

            GetPermutationRecursif(set, newCurrentlPicked, depth - 1, newList, output, validation);
        }
    }

#endregion
}

public class CombinatoiricSet<T> : IEnumerable
{
    List<List<T>> set;

    public CombinatoiricSet(List<List<T>> set)
    {
        this.set = set;
    }

    public List<T> this[int index]
    {
        get
        {
            return set[index];
        }
    }

    public IEnumerator GetEnumerator()
    {
        for (int index = 0; index < set.Count; index++)
        {
            yield return set[index];
        }
    }

    public override string ToString()
    {
        string fullOutput = "";
        foreach (List<T> list in set)
        {
            string output = "(";
            for (int i = 0; i < list.Count; i++)
            {
                output += list[i].ToString();
                if (i < list.Count - 1)
                {
                    output += ", ";
                }
            }
            output += ") \n";

            fullOutput += output;
        }
        return fullOutput;
    }
}

