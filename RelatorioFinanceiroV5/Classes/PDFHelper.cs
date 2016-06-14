using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace RelatorioFinanceiroV5.Classes
{
    public class PDFHelper
    {
        ///

        /// Exportar um HTML fornecido.
        ///
        /// O HTML.
        /// Nome do Arquivo.
        /// Link para o CSS.
        public static void Export(string html, string fileName, string linkCss)
        {
            ////reset response
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/pdf";

            ////define pdf filename
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);


            //Gera o arquivo PDF
            using (var document = new Document(PageSize.A4, 10, 10, 10, 10))
            {
                html = FormatImageLinks(html);

                //define o  output do  HTML
                var memStream = new MemoryStream();
                TextReader xmlString = new StringReader(html);

                PdfWriter writer = PdfWriter.GetInstance(document, memStream);

                document.Open();

                //Registra todas as fontes no computador cliente.
                FontFactory.RegisterDirectories();

                // Set factories
                var htmlContext = new HtmlPipelineContext(null);
                htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());

                // Set css
                ICSSResolver cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(false);
                cssResolver.AddCssFile(HttpContext.Current.Server.MapPath(linkCss), true);

                // Exporta
                IPipeline pipeline = new CssResolverPipeline(cssResolver,
                                                             new HtmlPipeline(htmlContext,
                                                                              new PdfWriterPipeline(document, writer)));
                var worker = new XMLWorker(pipeline, true);
                var xmlParse = new XMLParser(true, worker);
                xmlParse.Parse(xmlString);
                xmlParse.Flush();

                document.Close();
                document.Dispose();

                HttpContext.Current.Response.BinaryWrite(memStream.ToArray());
            }

            HttpContext.Current.Response.End();
            HttpContext.Current.Response.Flush();
        }

        ///

        /// Convertemos o link relativo para um link absoluto
        /// 
        /// input.
        /// 
        public static string FormatImageLinks(string input)
        {
            if (input == null)
                return string.Empty;
            string tempInput = input;
            const string pattern = @"";
            HttpContext context = HttpContext.Current;

            //Modificamos a URL relativa para abosuluta,caso exista alguma imagem em nossa pagina HTML.
            foreach (Match m in Regex.Matches(input, pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.RightToLeft))
            {
                if (!m.Success) continue;
                string tempM = m.Value;
                const string pattern1 = "src=[\'|\"](.+?)[\'|\"]";
                var reImg = new Regex(pattern1, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                Match mImg = reImg.Match(m.Value);

                if (!mImg.Success) continue;
                string src = mImg.Value.ToLower().Replace("src=", "").Replace("\"", "").Replace("\'", "");

                if (src.StartsWith("http://") || src.StartsWith("https://")) continue;
                //Inserimos a nova URL na tag img
                src = "src=\"" + context.Request.Url.Scheme + "://" +
                      context.Request.Url.Authority + src + "\"";
                try
                {
                    tempM = tempM.Remove(mImg.Index, mImg.Length);
                    tempM = tempM.Insert(mImg.Index, src);

                    // inserimos a nova url img para todo o código html
                    tempInput = tempInput.Remove(m.Index, m.Length);
                    tempInput = tempInput.Insert(m.Index, tempM);
                }
                catch (Exception)
                {

                }
            }
            return tempInput;
        }
    }
}