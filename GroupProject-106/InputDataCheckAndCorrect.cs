﻿using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject_106
{
    public class InputDataCheckAndCorrect
    {
        private string expr;
        private string[] funcs = { "cos", "sin", "tan", "cot", "ln" , "x" };
        private double low, up;
        public double Low => low;
        public double Up => up;
        private Dictionary<string, string> namesAndConsts = new Dictionary<string, string> { ["pi"] = "3,14159" , ["e"] = "2,71828" };
        public InputDataCheckAndCorrect(string expr , ListBox listbox , double low , double up) 
        {
            this.up = up;
            this.low = low;
            foreach(string item in listbox.Items)
            {
                string save1 = "";
                string save2 = "";
                for (int i = 0; i < item.Length; i++)
                {
                    if (item[i] == '=')
                    {
                        save1 = save2;
                    }
                    save2 += item[i];
                }
                save2 = "";
                for(int i = save1.Length+1; i < item.Length; i++)
                {
                    save2 += item[i];
                }
                bool flag = true;
                foreach (var i in namesAndConsts)
                {
                    if (i.Key.Equals(save1))
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    namesAndConsts.Add(save1, save2);
                }
                else
                {
                    MessageBox.Show("Название константы занято используется служебная версия.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
            }
            this.expr = expr;
        }

        public bool InputDataDiagnostic() 
        {
            bool flag = Up - Low > 0;
            if (!flag) MessageBox.Show("Ошибка! Неправильно заданы пределы интегрирования.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return ConstantDiagnostic() && BracketDiagnostic() && WordDiagnostic() && SgnDiagnostic()&&flag;
        }
        private bool ConstantDiagnostic()
        {
            foreach (var item in namesAndConsts)
            {
                
                for(int i = 0; i < item.Key.Length; i++)
                {
                    if (IsSgn(item.Key[i]) || item.Key[i] == ',' || item.Key[i] == '(' || item.Key[i] == ')')
                    {
                        MessageBox.Show("Ошибка! Запрещенный символ в названии константы.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            string save = "";
            string result = "";
            bool flag = false;
            for (int i = 0; i <= expr.Length; i++)
            {
                if (i == expr.Length || expr[i] == '(' || expr[i] == ')' || expr[i] == ',' || IsSgn(expr[i]) || expr[i] == ' ')
                {
                    if (IsConst(save)) result += namesAndConsts[save];
                    else result += save;
                    save = "";
                    if (i != expr.Length && expr[i] != ' ' && !flag)
                    {
                        result += expr[i];
                        flag = false;
                    }
                }
                else save += expr[i];
            }
            expr = result;
            return true;

        }
        private bool BracketDiagnostic() 
        {
            Stack<char> c = new Stack<char>();
            Stack<int> ind = new Stack<int>();
            for (int i = 0; i < expr.Length; i++)
            {
                if (expr[i] == '(')
                {
                    c.Push(expr[i]);
                    ind.Push(i);
                }
                else if (expr[i] == ')')
                {
                    if (c.Count > 0)
                    {
                        c.Pop();
                        ind.Pop();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка расставления скобок , попробуйте убрать скобку на " + (i + 1) + " месте.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            if (c.Count > 0)
            {
                MessageBox.Show("Ошибка расставления скобок , попробуйте куда-нибудь поставить скобку." , "Error" , MessageBoxButtons.OK , MessageBoxIcon.Error);
                return false;
            }
            else return true;
        }
        private bool WordDiagnostic() 
        {
            string save = "";
            for (int i = 0; i <= expr.Length; i++)
            {
                if (i == expr.Length || expr[i] == '(' || expr[i] == ')' ||  expr[i] == ',' || IsSgn(expr[i]))
                {
                    if (!IsFunc(save) && !IsNumber(save)&&!save.Equals(""))
                    {
                        MessageBox.Show("Ошибка! Не найдена функция или константа с именем " + save + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    save = "";
                }
                else save += expr[i];
            }
            return true;
        }
        private bool SgnDiagnostic()
        {
            if (expr.Length == 0)
            {
                MessageBox.Show("Ошибка! Введите хоть что-то!.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; 
            }
            string save = "";
            for (int i = 0; i <= expr.Length; i++)
            {
                if (i == expr.Length && (save.Equals("") || IsNumber(save) || expr[i-1] == 'x')) return true;
                else if (i == expr.Length )
                {
                    MessageBox.Show("Ошибка! После имени функции должна стоять откр. скобка.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (expr[i] == '(' || expr[i] == ')' || IsSgn(expr[i])) save = "";
                else save += expr[i];
                if (i == 0 && (expr[i] == ',' || IsSgn(expr[i]) && expr[i] != '-'))
                {
                    MessageBox.Show("Ошибка! Это " + expr[i] + " не может стоять вначале.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (i + 1 == expr.Length && (expr[i] == ',' || IsSgn(expr[i])))
                {
                    MessageBox.Show("Ошибка! Это " + expr[i] + " не может стоять в конце.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (IsSgn(expr[i]) && i + 1 != expr.Length && (IsSgn(expr[i + 1]) || expr[i + 1] == ',' || expr[i + 1] == ')'))
                {
                    MessageBox.Show("Ошибка! После операции должен стоять операнд. ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (IsSgn(expr[i]) && expr[i] != '-' && i != 0 && (expr[i - 1] == ',' || expr[i - 1] == '('))
                {
                    MessageBox.Show("Ошибка! Перед операцией должен стоять операнд. ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (expr[i] == '-' && i != 0 && expr[i - 1] == ',')
                {
                    MessageBox.Show("Ошибка! Перед минусом не может стоять запятая. ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (expr[i] == '(' && i + 1 != expr.Length && (expr[i + 1] == ',' || expr[i + 1] == ')'))
                {
                    MessageBox.Show("Ошибка! Скобки должны быть чем-то заполнены.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (expr[i] == '(' && i != 0 && (expr[i-1] == 'x' || IsNumber(expr[i-1]+"")))
                {
                    MessageBox.Show("Ошибка! Перед откр. скобкой должна стоять операция или откр. скобка.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (expr[i] == ')' && i + 1 != expr.Length && expr[i + 1] != ')' && !IsSgn(expr[i + 1]))
                {
                    MessageBox.Show("Ошибка! После закр. скобки должна стоять операция или закр. скобка.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (IsFunc(save) && expr[i] != 'x' && i + 1 != expr.Length && expr[i + 1] != '(')
                {
                    MessageBox.Show("Ошибка! После имени функции должна стоять откр. скобка.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (IsFunc(save) && expr[i] != 'x' && i - save.Length >= 0 && expr[i - save.Length] != '(' && !IsSgn(expr[i - save.Length]))
                {
                    MessageBox.Show("Ошибка! Перед именем функции должна стоять операция или откр. скобка.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (expr[i] == 'x' && i + 1 != expr.Length && expr[i + 1] != ')' && !IsSgn(expr[i + 1]))
                {
                    MessageBox.Show("Ошибка! После операнда должна стоять операция или закр. скобка.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (expr[i] == 'x' && i != 0 && expr[i-1] != '(' && !IsSgn(expr[i - 1]))
                {
                    MessageBox.Show("Ошибка! Перед операндом доложна стоять операция или откр. скобка.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (IsNumber(save) && i + 1 != expr.Length && !IsSgn(expr[i + 1]) && !IsNumber(expr[i + 1] + "") && expr[i + 1] != ')' && expr[i + 1] != ',')
                {
                    MessageBox.Show("Ошибка! После операнда должна стоять операция или закр. скобка.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;

                }
                else if (IsNumber(save) && i - save.Length >= 0 && !IsSgn(expr[i - save.Length]) && expr[i - save.Length] != '(' && expr[i - save.Length] != ',')
                {
                    MessageBox.Show("Ошибка! Перед операндом доложна стоять операция или откр. скобка.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }
        private bool IsNumber(string str) 
        {
            bool flag = false;
            try
            {
                Convert.ToDouble(str);
                flag = true;
            }
            catch {}
            return flag;
        }
        private bool IsFunc(string str)
        {
            bool flag = false;
            for (int j = 0; j < funcs.Length; j++)
            {
                if (funcs[j].Equals(str))
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }
        private bool IsConst(string str) 
        {
            bool flag = false;
            foreach(var item in namesAndConsts)
            {
                if (item.Key.Equals(str)) 
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }
        private bool IsSgn(char ch)
        {
            if (ch == '+' || ch == '-' || ch == '*' || ch == '/' || ch == '^') return true;
            return false;
        }
        public string ExprChangeForParsing()
        {
            string save = "";
            for(int i = 0; i <expr.Length; i++)
            {
                if (expr[i] == '-' && ( i == 0 || i!=0 && expr[i-1] == '('))
                {
                    save += "0";
                } 
                save+= expr[i];
            }
            return save;
        }
    }
}
