using SkiaSharp;
using System.Text;

namespace SignalRDemo.Comm
{
    public class UserTool
    {
        /// <summary>
        /// 随机产生常用汉字
        /// </summary>
        /// <param name="count">要产生汉字的个数</param>
        /// <returns>常用汉字</returns>
        public static string GetName(int count)
        {
            //启用 GB2312 编码
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var chineseWords = "";
            var rm = new System.Random();
            var gb = Encoding.GetEncoding("gb2312");

            for (var i = 0; i < count; i++)
            {
                // 获取区码(常用汉字的区码范围为16-55)
                var regionCode = rm.Next(16, 56);

                // 获取位码(位码范围为1-94 由于55区的90,91,92,93,94为空,故将其排除)
                int positionCode;
                if (regionCode == 55)
                {
                    // 55区排除90,91,92,93,94
                    positionCode = rm.Next(1, 90);
                }
                else
                {
                    positionCode = rm.Next(1, 95);
                }

                // 转换区位码为机内码
                var regionCode_Machine = regionCode + 160;// 160即为十六进制的20H+80H=A0H
                var positionCode_Machine = positionCode + 160;// 160即为十六进制的20H+80H=A0H

                // 转换为汉字
                var bytes = new byte[]
                {
                    (byte)regionCode_Machine,
                    (byte)positionCode_Machine
                };
                chineseWords += gb.GetString(bytes);
            }
            return chineseWords;
        }

        /// <summary>
        /// 生成图片
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static byte[] GetImage(string name)
        {
            var random = new Random();
            //颜色集合
            var colors = new[] { SKColors.Black, SKColors.Red, SKColors.Green, SKColors.Orange, SKColors.Brown, SKColors.DarkCyan, SKColors.Purple };
            //相当于js的 canvas.getContext('2d')
            using var image2d = new SKBitmap(50, 50, SKColorType.Bgra8888, SKAlphaType.Premul);
            //相当于前端的canvas
            using var canvas = new SKCanvas(image2d);
            //填充白色背景
            canvas.DrawColor(colors[random.Next(0, colors.Length - 1)]);
            //样式 跟xaml差不多
            using var drawStyle = new SKPaint();
            //样式
            drawStyle.IsAntialias = true;
            drawStyle.TextSize = 32;
            drawStyle.Typeface = SKTypeface.FromFamilyName("微软雅黑", SKFontStyleWeight.SemiBold, SKFontStyleWidth.ExtraCondensed, SKFontStyleSlant.Upright);
            drawStyle.Color = SKColors.White;
            //写字
            canvas.DrawText(name, 10, 35, drawStyle);
            //创建对象信息
            using var img = SKImage.FromBitmap(image2d);
            using var p = img.Encode(SKEncodedImageFormat.Png, 100);
            using var ms = new MemoryStream();
            //保存到流
            p.SaveTo(ms);
            var bytes = ms.GetBuffer();
            return bytes;
        }
    }
}
