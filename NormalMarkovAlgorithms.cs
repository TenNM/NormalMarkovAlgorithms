using System;
using System.Collections.Generic;

namespace NormalMarkovAlgorithms
{
    static class NormalMarkovAlgorithms
    {
        private static int iterLimit = 100;
        internal static int IterLimit
        {
            set { if (value > 0 && value < int.MaxValue) iterLimit = value; }
            get { return iterLimit; }
        }
        //--------------------------------------------
        private static int iterationsSystem = 0;
        private static int iterationsMarkov = 0;
        //-------------------------------------------
        internal static bool IsStepByStep = false;
        //----------------------------------------------------
        private static string baseStr;
        internal static string BaseStr
        {
            set { if (value != null) baseStr = value; }
            get { return baseStr; }
        }
        //-----
        private static string resStr;
        internal static string ResStr
        {
            get { return resStr; }
        }
        //------------------------------------------------------
        private static int ruleIdx;
        internal static int RuleIndex
        {
            get { return ruleIdx; }
        }
        private static List<OneRule> rules = new List<OneRule>();
        internal static List<OneRule> Rules
        {
            get { return rules; }
        }
        internal static bool AddRule(string find, string replaceWith, bool finishRule = false)
        {
            if (null == find || null == replaceWith) return false;
            rules.Add(new OneRule(find, replaceWith, finishRule));
            return true;
        }
        internal static bool DeleteRule(string find, string replaceWith)
        {
            int iDeleted = rules.RemoveAll(
                r => r.find.Equals(find) &&
                r.replaceWith.Equals(replaceWith)
                );
            return iDeleted > 0;
        }
        internal static bool DeleteRule(int index)
        {
            if (index > 0 && index < rules.Count - 1)
            {
                rules.RemoveAt(index);
                return true;
            }
            return false;
        }
        internal static bool DeleteAllRules()
        {
            if(rules.Count > 0)
            {
                rules.Clear();
                return true;
            }
            return false;
        }
        internal static void MoveRules(int ruleIndex, int moveSide)//+-1
        {
            if (ruleIndex < 0 && ruleIndex > rules.Count - 1) return;//out of bounds
            if (0 == ruleIndex && moveSide < 0) return; // cant dec 0 pos
            if (rules.Count - 1 == ruleIndex && moveSide > 0) return; // cant inc max pos
            if (ruleIndex + moveSide < 0 || ruleIndex + moveSide > rules.Count - 1) return;// out of bounds shift
            if (moveSide != 1 && moveSide != -1) return;// bad moveSide arg

            OneRule or = rules[ruleIndex];
            rules[ruleIndex] = rules[ruleIndex + moveSide];
            rules[ruleIndex + moveSide] = or;
        }
        //-------------------------------------------------------------
        internal static void Start()
        {
            bool needSwap = false;
            iterationsSystem = 0;
            iterationsMarkov = 0;
            resStr = baseStr;
            Console.WriteLine("\n{0, -5}{1, -5}", iterationsMarkov, resStr);

            for (ruleIdx = 0; ruleIdx < rules.Count && iterationsSystem < iterLimit; iterationsSystem++)
            {
                if (rules[ruleIdx].find.Equals(""))
                {
                    resStr = rules[ruleIdx].replaceWith + resStr;
                    needSwap = true;
                }
                else if (resStr.Contains(rules[ruleIdx].find))
                {
                    resStr = ReplaceFirst(
                        resStr,
                        rules[ruleIdx].find,
                        rules[ruleIdx].replaceWith
                        );
                    needSwap = true;
                }
                if (needSwap)
                {
                    if (IsStepByStep) { Console.ReadKey(true); }
                    needSwap = false;
                    iterationsMarkov++;
                    Console.WriteLine("{0, -5}{1, -5}   {2}", iterationsMarkov, resStr, rules[ruleIdx].ToString());
                    if (rules[ruleIdx].finishRule) return;
                    ruleIdx = 0;
                    continue;
                }
                else ruleIdx++;
            }//for
        }//start
        //-------------------------------------------------------------
        internal static void TestIt1()//binary to unary
        {
            BaseStr = "101";
            AddRule("1", "0|");
            AddRule("|0", "0||");
            AddRule("0", "");

            Start();
        }
        internal static void TestIt2()//doubling of str
        {
            BaseStr = "xxx";
            AddRule("*x", "xx*");
            AddRule("*", "", true);
            AddRule("", "*");
            IsStepByStep = true;

            Start();
        }
        //-----------------------------------------------------------
        internal static string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
        //----------------------------------------------------------------------
    }//c
    class OneRule
    {
        internal string find;
        internal string replaceWith;
        internal bool finishRule;

        internal OneRule(string find, string replaceWith, bool finishRule = false)
        {
            this.find = find;
            this.replaceWith = replaceWith;
            this.finishRule = finishRule;
        }
        public override string ToString()
        {
            return $"({find} -> {replaceWith})";
        }
    }//c
}//n
