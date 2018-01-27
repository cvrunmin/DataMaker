namespace DataMaker.Parsers
{
    public interface IParser
    {
        /// <summary>
        /// 该Parser正在编辑的Json的Key
        /// </summary>
        string Key { get; set; }

        /// <summary>
        /// 该 Parser 所属 Frame 的文件名
        /// 用于显示本地语言
        /// </summary>
        string FrameFileName { get; set; }

        /// <summary>
        /// 该 Parser 所编辑的 Json
        /// </summary>
        string Json { get; set; }

        /// <summary>
        /// 根据指定的Json文件设置parser
        /// </summary>
        /// <param name="json">指定Json</param>
        void SetParser(string json);

        /// <summary>
        /// 设置Parser大小
        /// </summary>
        void SetSize(int width);
    }
}
