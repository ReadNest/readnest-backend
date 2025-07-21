using System.Text.RegularExpressions;

namespace ReadNest.Shared.Utils
{
    public static class HtmlUtil
    {
        public static string StripHtml(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            input = Regex.Replace(input, "<(script|style)[^>]*>.*?</\\1>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            input = Regex.Replace(input, "<.*?>", " ", RegexOptions.Singleline);

            input = System.Net.WebUtility.HtmlDecode(input);

            return Regex.Replace(input, @"\s+", " ").Trim();
        }

        public static string NormalizeTextWithoutHtml(string text) => StringUtil.RemoveDiacritics(text?.ToLowerInvariant() ?? string.Empty);

        public static string NormalizeDescription(string text) => StringUtil.RemoveDiacritics(StripHtml(text ?? string.Empty).ToLowerInvariant());

        public static string GetSuccessEmailTemplate(string userName, string orderCode, string startDate, string endDate, string dashboardUrl)
        {
            return $@"
            <!DOCTYPE html>
            <html lang='vi'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <style>
                    body {{ font-family: Arial, sans-serif; background-color: #f9f9f9; padding: 20px; color: #333; }}
                    .container {{ background: #fff; border-radius: 8px; padding: 20px; max-width: 600px; margin: auto; box-shadow: 0    2px    8px    rgba(0,0,0,0.1); }}
                    .header {{ text-align: center; color: #2e86de; }}
                    .order-code {{ background: #f1f1f1; padding: 10px; border-radius: 4px; margin: 10px 0; font-weight: bold; }}
                    .btn {{ display: inline-block; background: #2e86de; color: #fff; padding: 10px 20px; border-radius: 4px; text-      decoration:   none; }}
                    .footer {{ margin-top: 20px; font-size: 0.9em; color: #777; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <h2 class='header'>🎉 Thanh toán thành công!</h2>
                    <p>Xin chào <b>{userName}</b>,</p>
                    <p>Cảm ơn bạn đã nâng cấp lên <b>Gói Premium</b>. Giao dịch của bạn đã được xử lý thành công.</p>
                    <p>Mã đơn hàng (Order Code):</p>
                    <div class='order-code'>{orderCode}</div>
                    <p>Gói Premium của bạn có hiệu lực từ <b>{startDate}</b> đến <b>{endDate}</b>.</p>
            
                    <p>Nếu bạn không hài lòng và muốn <b>hoàn tiền</b> trong vòng <b>3 ngày</b>, vui lòng reply lại email này và gửi    kèm    mã     Order Code.</p>
                                
                    <p class='footer'>
                        Nếu bạn có bất kỳ câu hỏi nào, hãy liên hệ <a href='mailto:readnest.information@gmail.com'>readnest.information@gmail.com</a>.
                    </p>
                </div>
            </body>
            </html>";
        }

        public static string GetFailureEmailTemplate(string userName, string orderCode)
        {
            return $@"
            <!DOCTYPE html>
            <html lang='vi'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <style>
                    body {{ font-family: Arial, sans-serif; background-color: #f9f9f9; padding: 20px; color: #333; }}
                    .container {{ background: #fff; border-radius: 8px; padding: 20px; max-width: 600px; margin: auto; box-shadow: 0    2px    8px    rgba(0,0,0,0.1); }}
                    .header {{ text-align: center; color: #d63031; }}
                    .order-code {{ background: #f1f1f1; padding: 10px; border-radius: 4px; margin: 10px 0; font-weight: bold; }}
                    .footer {{ margin-top: 20px; font-size: 0.9em; color: #777; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <h2 class='header'>❌ Thanh toán thất bại!</h2>
                    <p>Xin chào <b>{userName}</b>,</p>
                    <p>Chúng tôi rất tiếc thông báo rằng giao dịch của bạn <b>không thành công</b>.</p>
                    <p>Mã đơn hàng (Order Code):</p>
                    <div class='order-code'>{orderCode}</div>
                    <p>Vui lòng kiểm tra lại phương thức thanh toán hoặc liên hệ bộ phận hỗ trợ.</p>
            
                    <p class='footer'>
                        Nếu bạn có bất kỳ câu hỏi nào, hãy liên hệ <a href='mailto:readnest.information@gmail.com'>readnest.information@gmail.com</a>.
                    </p>
                </div>
            </body>
            </html>";
        }
    }
}
