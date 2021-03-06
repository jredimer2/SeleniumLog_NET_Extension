﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumLogger
{

    public sealed partial class SeleniumLog
    {
        public partial class _MessageSettings
        {
            /// <summary>
            /// Aggregate all settings and finally generate the string that will be written to the text log.
            /// </summary>
            /// <returns></returns>
            public string FormMessageString()
            {
                Stack MessageStack = new Stack();
                string ReturnString = "";
                string PathString = "";
                string FormattingString = "";

                // Step 1: Write flags
                if (WatchdogStart == true)
                {
                    MessageStack.Push("WATCHDOG_START");
                    Pass = false;
                }

                if (WatchdogEnd == true)
                {
                    MessageStack.Push("WATCHDOG_END");
                    Pass = false;
                }

                if (Pass == true)
                {
                    MessageStack.Push("PASS");
                    Pass = false;
                }

                if (Fail == true)
                {
                    MessageStack.Push("FAIL");
                    Fail = false;
                }

                if (Warning == true)
                {
                    MessageStack.Push("WARNING");
                    Warning = false;
                }

                if (Error == true)
                {
                    MessageStack.Push("ERROR");
                    Error = false;
                }

                if (Indent > 0)
                {
                    for (int i = 0; i < Indent; i++)
                    {
                        MessageStack.Push("INDENT");
                    }

                    if (Indent > 1)
                    {
                        CurrentIndentLevel = CurrentIndentLevel - Indent + 1;  //adjust, because INDENT;INDENT;INDENT really only causes one indent.
                    }
                    Indent = 0;
                }

                if (Unindent > 0)
                {
                    for (int i = 0; i < Unindent; i++)
                    {
                        MessageStack.Push("UNINDENT");
                    }
                    Unindent = 0;
                }

                if (Root == true)
                {
                    MessageStack.Push("ROOT");
                    Root = false;
                }

                if (RGB != null)
                {
                    MessageStack.Push("RGB:" + RGB.red + "," + RGB.green + "," + RGB.blue);
                    RGB = null;
                }

                if (DefaultRGB != null)
                {
                    MessageStack.Push("DEFAULT_RGB:" + DefaultRGB.red + "," + DefaultRGB.green + "," + DefaultRGB.blue);
                    DefaultRGB = null;
                }

                if ((Image != "") && (Image != null))
                {
                    MessageStack.Push("IMAGE:" + Image);
                    Image = "";
                }

                if ((File != "") && (File != null))
                {
                    MessageStack.Push("FILE:" + File);
                    File = "";
                }

                if ((Path != "") && (Path != null))
                {
                    MessageStack.Push("PATH:" + Path);
                    Path = "";
                }

                if ((Tab != "") && (Tab != null))
                {
                    MessageStack.Push("TAB:" + Tab);
                    Tab = "";
                }


                // Form the FormattingString    
                foreach (Object StackObj in MessageStack)
                {
                    //if (obj.ToString
                    FormattingString = (StackObj.ToString() + ";" + FormattingString).TrimStart(';').TrimEnd(';');
                }

                // Add line numbers
                if (ShowLineNumbers == true)
                {
                    MessageStr = "Line " + LineNum++.ToString() + ":   " + MessageStr;
                }
                else
                {
                    LineNum++;
                }

                // Form ReturnString
                if (FormattingString == "" || FormattingString == null)
                {
                    ReturnString = "<" + "TIMESTAMP:" + DateTime.Now.ToString(_TimestampFormat) + ">" + MessageStr;
                }
                else
                {
                    ReturnString = "<" + FormattingString + ";TIMESTAMP:" + DateTime.Now.ToString(_TimestampFormat) + ">" + MessageStr;
                }

                ResetDefaultValues();

                indentModel.SimulateIndentations(ReturnString);
                //Console.WriteLine("FORMAT_STR: indentModel.CurrentLevel = " + indentModel.CurrentLevel);
                return ReturnString;
            }
            // Stack Code:
            // http://msdn.microsoft.com/en-us/library/system.collections.stack%28v=vs.110%29.aspx

        }
    }
}
