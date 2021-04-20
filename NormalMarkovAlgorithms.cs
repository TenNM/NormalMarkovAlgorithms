using System;
using System.Collections.Generic;

namespace NormalMarkovAlgorithms
{
    static class NormalMarkovAlgorithms
    {
        private static int _iterLimit = 100;
        internal static int IterLimit
        {
            set { if (value > 0 && value < int.MaxValue) _iterLimit = value; }
            get { return _iterLimit; }
        }
        //--------------------------------------------
        private static int iterationsSystem = 0;
        private static int iterationsMarkov = 0;
        //-------------------------------------------
        internal static bool IsStepByStep = false;
        //----------------------------------------------------
        private static string _baseStr;
        internal static string BaseStr
        {
            set { if (value != null) _baseStr = value; }
            get { return _baseStr; }
        }
        //-----
        private static string _resStr;
        internal static string ResStr
        {
            get { return _resStr; }
        }
        //------------------------------------------------------
        private static int _ruleIdx;
        internal static int RuleIndex
        {
            get { return _ruleIdx; }
        }
        private static List<OneRule> _rules = new List<OneRule>();
        internal static List<OneRule> Rules
        {
            get { return _rules; }
        }
        internal static bool AddRule(string find, string replaceWith, bool finishRule = false)
        {
            if (null == find || null == replaceWith) return false;
            _rules.Add(new OneRule(find, replaceWith, finishRule));
            return true;
        }
        /*internal static bool AddRule(string find, string replaceWith)
        {
            if (null == find || null == replaceWith) return false;
            _rules.Add(new OneRule(find, replaceWith));
            return true;
        }*/
        internal static bool DeleteRule(string find, string replaceWith)
        {
            int iDeleted = _rules.RemoveAll(
                r => r._find.Equals(find) &&
                r._replaceWith.Equals(replaceWith)
                );
            if (iDeleted > 0) return true;
            return false;
        }
        internal static bool DeleteRule(int index)
        {
            if (index > 0 && index < _rules.Count - 1)
            {
                _rules.RemoveAt(index);
                return true;
            }
            return false;
        }
        internal static void MoveRules(int ruleIndex, int moveSide)//+-1
        {
            if (ruleIndex < 0 && ruleIndex > _rules.Count - 1) return;//out of bounds
            if (0 == ruleIndex && moveSide < 0) return; // cant dec 0 pos
            if (_rules.Count - 1 == ruleIndex && moveSide > 0) return; // cant inc max pos
            if (ruleIndex + moveSide < 0 || ruleIndex + moveSide > _rules.Count - 1) return;// out of bounds shift
            if (moveSide != 1 && moveSide != -1) return;// bad moveSide arg

            OneRule or = _rules[ruleIndex];
            _rules[ruleIndex] = _rules[ruleIndex + moveSide];
            _rules[ruleIndex + moveSide] = or;
        }
        //-------------------------------------------------------------
        internal static void Start()
        {
            bool swapProc = false;
            iterationsSystem = 0;
            iterationsMarkov = 0;
            _resStr = _baseStr;
            Console.WriteLine("\n{0, -5}{1, -5}", iterationsMarkov, _resStr);

            for (_ruleIdx = 0; _ruleIdx < _rules.Count && iterationsSystem < _iterLimit; iterationsSystem++)
            {
                if (_rules[_ruleIdx]._find.Equals(""))
                {
                    _resStr = _rules[_ruleIdx]._replaceWith + _resStr;
                    swapProc = true;
                }
                else if (_resStr.Contains(_rules[_ruleIdx]._find))
                {
                    _resStr = ReplaceFirst(
                        _resStr,
                        _rules[_ruleIdx]._find,
                        _rules[_ruleIdx]._replaceWith
                        );
                    swapProc = true;
                }
                if (swapProc)
                {
                    if (IsStepByStep) { Console.ReadKey(true); }
                    swapProc = false;
                    iterationsMarkov++;
                    Console.WriteLine("{0, -5}{1, -5}   {2}", iterationsMarkov, _resStr, _rules[_ruleIdx].ToString());
                    if (_rules[_ruleIdx]._finishRule) return;
                    _ruleIdx = 0;
                    continue;
                }
                else _ruleIdx++;
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
        internal string _find;
        internal string _replaceWith;
        internal bool _finishRule;

        internal OneRule(string find, string replaceWith, bool finishRule = false)
        {
            _find = find;
            _replaceWith = replaceWith;
            _finishRule = finishRule;
        }
        /*internal OneRule(string find, string replaceWith)
        {
            _find = find;
            _replaceWith = replaceWith;
            _finishRule = false;
        }*/
        public override string ToString()
        {
            return $"({_find} -> {_replaceWith})";
        }
    }//c
}//n
