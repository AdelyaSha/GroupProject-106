﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject_106
{
    public class CheckForSyntax
    {
        private string expr;
        public CheckForSyntax(string expr) 
        {
            this.expr = expr;   
        }

        public bool ExpressionDiagnostic() 
        {
            return BracketDiagnostic() && WordDiagnostic();
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
                        MessageBox.Show("Ошибка расставления скобок , попробуйте убрать скобку на " + (i + 1) + " месте", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            if (c.Count > 0)
            {
                MessageBox.Show("Ошибка расставления скобок , попробуйте убрать скобку на " + (ind.Pop()+1) + " месте" , "Error" , MessageBoxButtons.OK , MessageBoxIcon.Error);
                return false;
            }
            else return true;
        }
        private bool WordDiagnostic() 
        {
            return true;
        }
        public string ExprAdjustment()
        {
            return expr;
        }
    }
}
