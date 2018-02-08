using DataMaker.Parsers;
using System;

namespace DataMaker
{
    /// <summary>
    /// 表示一个条件
    /// </summary>
    public class Condition
    {
        private string content;
        private FrameParser parser;

        public Condition(string content, FrameParser parser)
        {
            this.content = content;
            this.parser = parser;
        }

        public static Condition Parse(string content, FrameParser parser)
        {
            return new Condition(content, parser);
        }

        public override string ToString()
        {
            return content;
        }

        public bool IsTrue()
        {
            if (string.IsNullOrEmpty(content))
            {
                return true;
            }
            else if (content.Contains("=="))
            {
                var objects = content.Split(new[] { "==" }, StringSplitOptions.None);
                objects[0] = objects[0].Replace("%Value:", "").Replace("%", "");
                foreach (var i in parser.PanelControls)
                {
                    if (i is IParser)
                    {
                        if (((IParser)i).Key == objects[0])
                        {
                            if (i is TextParser)
                            {
                                return ((TextParser)i).Value.Equals(objects[1]);
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}
