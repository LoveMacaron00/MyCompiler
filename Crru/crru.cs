using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Crru
{
    public partial class crru : Form
    {
        public crru()
        {
            InitializeComponent();
            //DTest();
        }

        private void Close_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Close?", "confirm", MessageBoxButtons.OKCancel) == DialogResult.OK)
                this.Close();
        }

        private void bNew_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox2.Clear();
            richTextBox1.Focus();
        }

        private void bRun_Click(object sender, EventArgs e)
        {
            richTextBox2.Clear();
            Dictionary<string, object> variables = new Dictionary<string, object>();
            string[] commands = richTextBox1.Lines;
            runCommand(0, commands.Length - 1, commands, variables);
        }

        private void runCommand(int start, int stop, string[] commands, Dictionary<string, object> variables)
        {
            for (int i = start; i <= stop; i++)
            {
                string command = commands[i].Trim();

                try
                {
                    ProcessCommand(command, ref i, commands, variables);
                }
                catch (Exception ex)
                {
                    HandleError(i, ex, command);
                }
            }
        }

        private void ProcessCommand(string command, ref int i, string[] commands, Dictionary<string, object> variables)
        {
            if (string.IsNullOrWhiteSpace(command)) return;

            string lowerCommand = command.ToLower();

            switch (lowerCommand.Split(' ')[0])
            {
                case "input":
                    ProcessInputCommand(command, variables);
                    break;
                case "output":
                    ProcessOutputCommand(command, variables);
                    break;
                case "if":
                    i = ProcessIfElseCommand(i, commands, variables);
                    break;
                case "while":
                    i = ProcessWhileDoCommand(i, variables);
                    break;
                case "do":
                    i = ProcessDoWhileCommand(i, variables);
                    break;
                case "case":
                    i = ProcessCase(i, variables);
                    break;
                case "for":
                    i = ProcessForCommand(i, commands, variables);
                    break;
                default:
                    if (command.Contains("="))
                    {
                        ProcessAssignmentCommand(command, variables);
                    }
                    else
                    {
                        throw new Exception("ข้อผิดพลาดทางไวยากรณ์: คำสั่งไม่รู้จัก.");
                    }
                    break;
            }
        }

        private int ProcessForCommand(int start, string[] commands, Dictionary<string, object> variables)
        {
            string commandLine = commands[start];
            string[] parts = commandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < 4 || parts[0] != "for" || parts[2] != "to")
            {
                throw new Exception("ข้อผิดพลาดทางไวยากรณ์: รูปแบบคำสั่ง for ไม่ถูกต้อง.");
            }

            string variableName = parts[1].Split('=')[0].Trim();
            double initialValue = GetValueOrVariable(parts[1].Split('=')[1].Trim(), variables);
            double finalValue = GetValueOrVariable(parts[3].Trim(), variables);
            double stepValue = parts.Length > 4 && parts[4] == "step" ? GetValueOrVariable(parts[5].Trim(), variables) : 1;

            if (!IsValidVariableName(variableName))
            {
                throw new Exception($"ข้อผิดพลาดทางไวยากรณ์: ชื่อตัวแปร '{variableName}' ไม่ถูกต้อง.");
            }

            variables[variableName] = initialValue;

            int endFor = getBox(start, "next " + variableName);

            if (endFor == -1)
            {
                throw new Exception($"ข้อผิดพลาดทางไวยากรณ์: ไม่พบ 'next {variableName}' สำหรับ 'for'.");
            }

            if (stepValue > 0)
            {
                for (double i = initialValue; i <= finalValue; i += stepValue)
                {
                    variables[variableName] = i;
                    runCommand(start + 1, endFor - 1, commands, variables);
                }
            }
            else
            {
                for (double i = initialValue; i >= finalValue; i += stepValue)
                {
                    variables[variableName] = i;
                    runCommand(start + 1, endFor - 1, commands, variables);
                }
            }

            return endFor;
        }

        private int ProcessDoWhileCommand(int start, Dictionary<string, object> variables)
        {
            string[] commands = richTextBox1.Lines;

            int endWhile = getBox(start, "while");

            if (endWhile == -1)
            {
                throw new Exception("ข้อผิดพลาดทางไวยากรณ์: ไม่พบ 'while' สำหรับ 'do'");
            }

            string whileCondition = commands[endWhile].Substring(5).Trim();
            do
            {
                runCommand(start + 1, endWhile - 1, commands, variables);
            }
            while (EvaluateCondition(whileCondition, variables));

            return endWhile;
        }

        private int ProcessWhileDoCommand(int start, Dictionary<string, object> variables)
        {
            string[] commands = richTextBox1.Lines;

            int endDo = getBox(start, "do");

            if (endDo == -1)
            {
                throw new Exception("ข้อผิดพลาดทางไวยากรณ์: ไม่พบ 'do' สำหรับ 'while'");
            }

            string whileCondition = commands[endDo].Substring(5).Trim();
            while (EvaluateCondition(whileCondition, variables))
            {
                runCommand(start + 1, endDo - 1, commands, variables);
            }

            return endDo;
        }

        private int ProcessIfElseCommand(int start, string[] commands, Dictionary<string, object> variables)
        {
            bool conditionResult = EvaluateCondition(commands[start].Substring(3).Trim(), variables);
            int indexElse = getBox(start, "else");
            int indexEndIf = getBox(start, "endIf");

            if (indexEndIf == -1)
            {
                throw new Exception("ข้อผิดพลาดทางไวยากรณ์: ไม่พบ 'end if' สำหรับ 'if'");
            }

            if (conditionResult)
            {
                runCommand(start + 1, indexElse == -1 ? indexEndIf - 1 : indexElse - 1, commands, variables);
            }
            else if (indexElse != -1)
            {
                runCommand(indexElse + 1, indexEndIf - 1, commands, variables);
            }

            return indexEndIf;
        }

        private int getBox(int start, string xx)
        {
            string[] commands = richTextBox1.Lines;
            for (int i = start + 1; i < commands.Length; i++)
            {
                string command = commands[i].Trim();

                if (command.StartsWith(xx))
                {
                    return i;
                }
            }
            return -1;
        }

        private void ProcessInputCommand(string command, Dictionary<string, object> variables)
        {
            string variablePart = command.Substring(6).Trim();
            if (string.IsNullOrEmpty(variablePart))
            {
                throw new Exception("ข้อผิดพลาดทางไวยากรณ์: คำสั่ง input ต้องมีตัวแปรอย่างน้อยหนึ่งตัว.");
            }
            string[] variableNames = variablePart.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string vName in variableNames)
            {
                if (!IsValidVariableName(vName.Trim()))
                {
                    throw new Exception($"ข้อผิดพลาดทางไวยากรณ์: ชื่อตัวแปร '{vName}' ไม่ถูกต้อง.");
                }
                string data = new input(vName.Trim()).getData();
                if (double.TryParse(data, out double value))
                {
                    variables[vName.Trim()] = value;
                }
                else
                {
                    variables[vName.Trim()] = data;
                }
            }
        }

        private void ProcessOutputCommand(string command, Dictionary<string, object> variables)
        {
            string outputPart = command.Substring(7).Trim();
            if (string.IsNullOrEmpty(outputPart))
            {
                throw new Exception("ข้อผิดพลาดทางไวยากรณ์: คำสั่ง output ต้องมีอย่างน้อยหนึ่งรายการ.");
            }

            string[] outputItems = outputPart.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<object> outputValues = new List<object>();

            foreach (string item in outputItems)
            {
                string trimmedItem = item.Trim();

                if (double.TryParse(trimmedItem, out double numericValue))
                {
                    outputValues.Add(numericValue);
                }
                else if (trimmedItem.StartsWith("\"") && trimmedItem.EndsWith("\""))
                {
                    outputValues.Add(trimmedItem.Trim('"'));
                }
                else if (variables.TryGetValue(trimmedItem, out object value))
                {
                    outputValues.Add(value);
                }
                else
                {
                    throw new Exception($"ข้อผิดพลาดทางไวยากรณ์: ไม่สามารถแยกรายการ '{trimmedItem}' ในคำสั่ง output.");
                }
            }

            if (outputValues.Count > 0)
            {
                Output.Show(richTextBox2, outputValues.ToArray());
            }
        }

        private int ProcessCase(int start, Dictionary<string, object> variables)
        {
            string commandLine = richTextBox1.Lines[start];

            int endCase = getBox(start, "endCase");

            if (endCase == -1)
            {
                throw new Exception("ข้อผิดพลาดทางไวยากรณ์: ไม่พบ 'endCase' สำหรับ 'case'");
            }

            string vname = commandLine.Substring(5).Trim();

            if (!variables.ContainsKey(vname))
            {
                throw new Exception($"ไม่พบตัวแปร '{vname}'");
            }

            double caseValue = Convert.ToDouble(variables[vname]);

            for (int i = start + 1; i < endCase; i++)
            {
                string command = richTextBox1.Lines[i].Trim();

                // ข้ามบรรทัดว่าง
                if (string.IsNullOrWhiteSpace(command)) continue;

                // แยกเงื่อนไขและคำสั่งด้วย ':'
                string[] parts = command.Split(new[] { ':' }, 2);

                if (parts.Length != 2)
                {
                    throw new Exception($"ข้อผิดพลาดทางไวยากรณ์: บรรทัด '{command}' ไม่ถูกต้อง.");
                }

                double conditionValue = Convert.ToDouble(parts[0].Trim());
                string action = parts[1].Trim();

                // ตรวจสอบเงื่อนไข
                if (caseValue == conditionValue)
                {
                    // ตรวจสอบคำสั่ง input และ assignment ภายใน case
                    if (action.StartsWith("input"))
                    {
                        ProcessInputCommand(action, variables);
                    }
                    else if (action.Contains("="))
                    {
                        ProcessAssignmentCommand(action, variables);
                    }
                    else if (action.StartsWith("output"))
                    {
                        ProcessOutputCommand(action, variables);
                    }
                    break; // หยุดการวนลูปกรณีตรงเงื่อนไขแล้ว
                }
            }

            return endCase;
        }


        //private void DTest ()
        //{
        //Dictionary<string, int> grade = new Dictionary<string, int>();
        //grade.Add("A", 4);
        //grade.Add("B", 3);
        //grade.Add("C", 2);
        //grade.Add("D", 1);

        //string g = "A";
        //int v = grade[g];
        //int v = 0;
        //for (int i =0; i <grade.Count; i++)
        //{
        //    v = grade[g];
        //}

        //if (grade.ContainsKey(g))
        //{
        //    v = grade[g];
        //    System.Console.WriteLine(v);
        //}


        //System.Console.WriteLine(v);

        //}


        private bool EvaluateCondition(string condition, Dictionary<string, object> variables)
        {
            string pattern = @"(or|and|\(|\))";
            string[] parts = Regex.Split(condition, pattern).Where(p => !string.IsNullOrWhiteSpace(p)).ToArray();
            Stack<bool> results = new Stack<bool>();
            Stack<string> operators = new Stack<string>();

            foreach (string part in parts)
            {
                string trimmedPart = part.Trim().ToLower();

                if (trimmedPart == "or" || trimmedPart == "and")
                {
                    operators.Push(trimmedPart);
                }
                else if (trimmedPart == "(")
                {
                    operators.Push(trimmedPart);
                }
                else if (trimmedPart == ")")
                {
                    while (operators.Peek() != "(")
                    {
                        bool right = results.Pop();
                        bool left = results.Pop();
                        string op = operators.Pop();
                        results.Push(EvaluateLogicalOperation(left, right, op));
                    }
                    operators.Pop();
                }
                else
                {
                    results.Push(EvaluateSingleCondition(trimmedPart, variables));
                }
            }

            while (operators.Count > 0)
            {
                bool right = results.Pop();
                bool left = results.Pop();
                string op = operators.Pop();
                results.Push(EvaluateLogicalOperation(left, right, op));
            }

            return results.Pop();
        }

        private bool EvaluateLogicalOperation(bool left, bool right, string op)
        {
            switch (op)
            {
                case "or":
                    return left || right;
                case "and":
                    return left && right;
                default:
                    throw new Exception($"Unknown logical operator: {op}");
            }
        }

        private bool EvaluateSingleCondition(string condition, Dictionary<string, object> variables)
        {
            string pattern = @"(<=|>=|!=|<|>|=)";
            var match = Regex.Match(condition, pattern);

            if (!match.Success)
            {
                throw new Exception("ข้อผิดพลาดทางไวยากรณ์: เงื่อนไขต้องมีเครื่องหมายเปรียบเทียบเช่น '=', '>', '<', '>=', '<=', '!='");
            }

            string selectedOperator = match.Value;
            string[] tokens = condition.Split(new[] { selectedOperator }, StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length != 2)
            {
                throw new Exception("ข้อผิดพลาดทางไวยากรณ์: เงื่อนไขต้องมีการเปรียบเทียบสองฝั่ง.");
            }

            double leftValue = GetValueOrVariable(tokens[0].Trim(), variables);
            double rightValue = GetValueOrVariable(tokens[1].Trim(), variables);

            switch (selectedOperator)
            {
                case "=":
                    return leftValue == rightValue;
                case ">":
                    return leftValue > rightValue;
                case "<":
                    return leftValue < rightValue;
                case ">=":
                    return leftValue >= rightValue;
                case "<=":
                    return leftValue <= rightValue;
                case "!=":
                    return leftValue != rightValue;
                default:
                    throw new Exception("ข้อผิดพลาดทางไวยากรณ์: เครื่องหมายเปรียบเทียบไม่ถูกต้อง.");
            }
        }

        private double GetValueOrVariable(string input, Dictionary<string, object> variables)
        {
            if (double.TryParse(input, out double value))
            {
                return value;
            }
            else if (variables.TryGetValue(input, out object variableValue) && variableValue is double)
            {
                return (double)variableValue;
            }
            else
            {
                throw new Exception($"ข้อผิดพลาดทางไวยากรณ์: ไม่พบตัวแปรหรือค่าที่ถูกต้อง '{input}'.");
            }
        }

        private void ProcessAssignmentCommand(string command, Dictionary<string, object> variables)
        {
            string[] parts = command.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
            {
                throw new Exception("ข้อผิดพลาดทางไวยากรณ์: คำสั่งกำหนดค่าผิดรูปแบบ.");
            }
            string variableName = parts[0].Trim();
            string expression = parts[1].Trim();

            if (!IsValidVariableName(variableName))
            {
                throw new Exception($"ข้อผิดพลาดทางไวยากรณ์: ชื่อตัวแปร '{variableName}' ไม่ถูกต้อง.");
            }

            if (expression.Contains("\""))
            {
                string evaluatedExpression = EvaluateStringExpression(expression, variables);
                variables[variableName] = evaluatedExpression;
            }
            else
            {
                string evaluatedExpression = convertExpression(expression, variables);
                double result = EvaluateExpression(evaluatedExpression, variables);
                variables[variableName] = result;
            }
        }

        private bool IsValidVariableName(string name)
        {
            return !string.IsNullOrEmpty(name) && char.IsLetter(name[0]) && name.All(c => char.IsLetterOrDigit(c) || c == '_');
        }

        private double EvaluateExpression(string expression, Dictionary<string, object> variables)
        {
            foreach (var variable in variables)
            {
                if (variable.Value is double)
                {
                    expression = expression.Replace(variable.Key, variable.Value.ToString());
                }
            }

            DataTable table = new DataTable();
            return Convert.ToDouble(table.Compute(expression, string.Empty));
        }

        private string convertExpression(string exp, Dictionary<string, object> variables)
        {
            string[] token = System.Text.RegularExpressions.Regex.Split(exp, @"(\+|\-|\*|\/)");
            string newExpression = "";

            for (int i = 0; i < token.Length; i++)
            {
                if (isOperator(token[i]) || isNumber(token[i]))
                {
                    newExpression += token[i];
                }
                else if (variables.ContainsKey(token[i].Trim()))
                {
                    newExpression += variables[token[i].Trim()].ToString();
                }
                else
                {
                    throw new Exception($"ข้อผิดพลาดทางไวยากรณ์: ตัวแปร '{token[i]}' ไม่พบในตัวแปรที่กำหนด.");
                }
            }

            return newExpression;
        }

        private bool isOperator(string opt)
        {
            opt = opt.Trim();
            string optList = "+-*/";
            return optList.IndexOf(opt[0]) >= 0;
        }

        private bool isNumber(string token)
        {
            return double.TryParse(token.Trim(), out _);
        }

        private string EvaluateStringExpression(string expression, Dictionary<string, object> variables)
        {
            string result = expression;

            foreach (var variable in variables)
            {
                if (variable.Value is string)
                {
                    result = result.Replace(variable.Key, variable.Value.ToString());
                }
            }

            result = result.Replace("\"", "");

            if (result.Contains("+"))
            {
                string[] parts = result.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
                result = string.Join(string.Empty, parts.Select(p => p.Trim()));
            }

            return result;
        }

        private void HandleError(int lineNumber, Exception ex, string command)
        {
            string errorMessage = $"ข้อผิดพลาดที่บรรทัด {lineNumber + 1}: {ex.Message} - {command}";
            DisplayError(errorMessage);
        }

        private void DisplayError(string errorMessage)
        {
            richTextBox2.SelectionColor = Color.Red;
            richTextBox2.AppendText(errorMessage + Environment.NewLine);
            richTextBox2.SelectionColor = richTextBox2.ForeColor;
        }
    }
}