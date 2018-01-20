﻿namespace DataMaker.Parsers
{
    public interface IParser
    {
        /// <summary>
        /// 该Parser正在编辑的Json的Key
        /// </summary>
        string Key { get; set; }

        /// <summary>
        /// 根据指定的Json文件设置parser
        /// </summary>
        /// <param name="json">指定Json</param>
        void SetParser(string json);

        /// <summary>
        /// 为parser设置指定Json
        /// </summary>
        /// <param name="json">指定Json</param>
        void SetJson(string json);

        /// <summary>
        /// 获得parser当前编辑的Json
        /// </summary>
        /// <returns>Json文本</returns>
        string GetJson();
    }
}