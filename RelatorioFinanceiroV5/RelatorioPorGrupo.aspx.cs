using MySql.Data.MySqlClient;
using RelatorioFinanceiroV5.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RelatorioFinanceiroV5
{
    public partial class RelatorioPorGrupo : System.Web.UI.Page
    {
        int total = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            var myConn = Connection.conn();
            if (!this.IsPostBack)
            {
                BindGrid("Jan_16", myConn);
                pnlbody.Visible = true;
            }
        }

        private void BindGrid(string mesReferencia, MySqlConnection conn)
        {

            DataTable dt = new DataTable();
            using (conn)
            {
                dt = Service.getQuantidadeConteudoPorGrupo(mesReferencia, conn);


                using (dt)
                {

                    GridViewQuantidades.DataSource = dt;
                    GridViewQuantidades.DataBind();

                    GridViewQuantidades.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;

                    //decimal total = dt.AsEnumerable().Sum(row => row.Field<decimal>("quantidade"));
                    //GridViewQuantidades.FooterRow.Cells[2].Text = "Total";
                    //GridViewQuantidades.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                    //GridViewQuantidades.FooterRow.Cells[3].Text = total.ToString();

                }
            }
        }

        protected void GridViewQuantidades_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void GridViewQuantidades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Visualizar")
            {
                var myConn = Connection.conn();
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = GridViewQuantidades.Rows[index];
                //Grupo grupo = new Grupo();
                //grupo.Id = Convert.ToInt32(row.Cells[0]);
                //grupo.Nome = Convert.ToString(row.Cells[1]);
                //grupo.IdGrupo = Convert.ToInt32(row.Cells[2]);
                //grupo.Quantidade = Convert.ToInt32(row.Cells[3]);
                //grupo.MesReferencia = Convert.ToString(row.Cells[4]);

                int idGrupo = (int)Convert.ToInt32(row.Cells[2].Text);
                string mesReferencia = row.Cells[4].Text;

                //calcula o percentual da quantidade sobre o total de conteudos

                int quantidadeTotal = Service.QuantidadeTotal(myConn, mesReferencia);
                decimal percentual = Math.Round(Convert.ToDecimal(row.Cells[3].Text) / (decimal)quantidadeTotal * 100, 6);

                lblPercentualEditoraTotal.Text = Math.Round(percentual, 2).ToString() + "%";

                int maisAcessados = Service.GetMaisAcessados(myConn, idGrupo, mesReferencia);
                int totalRefxMaisAcessados = Service.TotalReferenciaMaisAcessados(myConn, mesReferencia);
                lblTotalRefMaisAcessados.Text = maisAcessados.ToString();

                decimal percentualReferenciaMaisAcessado = Math.Round(Convert.ToDecimal(maisAcessados / (decimal)totalRefxMaisAcessados * 100), 6);

                lblPercentual10MaisAcessados.Text = Math.Round(percentualReferenciaMaisAcessado, 2).ToString() + "%";



                decimal receita = Service.GetReceita(myConn, mesReferencia);
                lblReceita.Text = receita.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"));


                decimal receita20 = Math.Round((decimal)receita * (decimal)0.2, 6);
                decimal receita10 = Math.Round((decimal)receita * (decimal)0.1, 6);


                lblReceita_20.Text = receita20.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"));
                lblReceita_10.Text = receita10.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"));

                decimal receitaTotal = receita10 + receita20;
                lblReceitaTotalASerDividida.Text = receitaTotal.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"));

                //ok ate aqui

                double valorASerRepassadoPelaQuantidade = Math.Round((Convert.ToDouble(percentual) * Convert.ToDouble(receita20)) / 100, 6);
                lblValorRepasseQuantidade.Text = valorASerRepassadoPelaQuantidade.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"));

                double valorASerRepassadoPelaReferMaisAcessados = Math.Round((Convert.ToDouble(percentualReferenciaMaisAcessado) * Convert.ToDouble(receita10)) / 100, 6);
                lblValorRepasseRefMaisAcessados.Text = valorASerRepassadoPelaReferMaisAcessados.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"));

                double valorTotalRepasse = valorASerRepassadoPelaQuantidade + valorASerRepassadoPelaReferMaisAcessados;
                lblValorTotalRepasse.Text = valorTotalRepasse.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"));

                lblGrupo.Text = row.Cells[1].Text;
                lblMes.Text = row.Cells[4].Text;
                lblQuantidadeConteudos.Text = row.Cells[3].Text;

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "openModal();", true);
            }
        }

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            MakePDF();
        }


        private void MakePDF()
        {
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table class='table table-bordered table-striped'><thead><tr><th colspan='2'><img src='http://localhost:50403/images/cabecalho.png'/></th></tr><tr><th colspan='2' style='color: #000000; background-color: #337ab7; font-size: 20px;' class='text-center'>Relatório Financeiro - Nuvem de Livros</th></tr><tr style='background-color: #b9defe'><th width='350'><strong>");
                    sb.Append(lblGrupo.Text);
                    sb.Append("</strong></th><th width='100'><strong>");
                    sb.Append(lblMes.Text);
                    sb.Append("</strong></th></tr></thead><tbody><tr><td colspan='2'><strong>Número de Ítens da Editora</strong></td></tr><tr><td><i>Quantidade de Conteúdos</i></td><td class='text-center'><strong>");
                    sb.Append(lblQuantidadeConteudos.Text);
                    sb.Append("</strong></td></tr><tr><td><i>% da editora do total</i></td><td class='text-center'><strong>");
                    sb.Append(lblPercentualEditoraTotal.Text);
                    sb.Append("</strong></td></tr><tr><td colspan='2'><strong>Número de Ítens da Editora</strong></td></tr><tr><td><i>Conteúdo de ref. e mais acessados</i></td><td class='text-center'><strong>");
                    sb.Append(lblTotalRefMaisAcessados.Text);
                    sb.Append("</strong></td></tr><tr><td><i>% da editora dos 10% mais acessados e referência</i></td><td class='text-center'><strong>");
                    sb.Append(lblPercentual10MaisAcessados.Text);
                    sb.Append("</strong></td></tr><tr><td><i>Receita líquida total da Nuvem de Livros</i></td><td class='text-center'><strong>");
                    sb.Append(lblReceita.Text);
                    sb.Append("</strong></td></tr><tr><td><i>Receita a ser dividida entre as editoras pelo conteúdo (20%)</i></td><td class='text-center'><strong>");
                    sb.Append(lblReceita_20.Text);
                    sb.Append("</strong></td></tr><tr><td><i>Receita a ser dividida entre as editoras pelas obras de referência e mais acessados (10%)</i></td><td class='text-center'><strong>");
                    sb.Append(lblReceita_10.Text);
                    sb.Append("</strong></td></tr><tr><td><i>Receita total a ser dividida entre as editoras</i></td><td class='text-center'><strong>");
                    sb.Append(lblReceitaTotalASerDividida.Text);
                    sb.Append("</strong></td></tr><tr><td><i>Valor a ser repassado para a editora pela quantidade de conteúdos</i></td><td class='text-center'><strong>");
                    sb.Append(lblValorRepasseQuantidade.Text);
                    sb.Append("</strong></td></tr><tr><td><i>Valor a ser repassado para a editora pelas obras de referência e mais acessados</i></td><td class='text-center'><strong>");
                    sb.Append(lblValorRepasseRefMaisAcessados.Text);
                    sb.Append("</strong></td></tr><tr><td><i>Valor total ser repassado para a editora</i></td><td class='text-center'><strong>");
                    sb.Append(lblValorTotalRepasse.Text);
                    sb.Append("</strong></td></tr></tbody></table>");



                    string myFile = HttpUtility.HtmlDecode(lblGrupo.Text);

                    string myFileName = Service.RemoveAccents(myFile);

                    PDFHelper.Export(sb.ToString(), "RelFin_" + lblMes.Text + "_" + myFileName + ".pdf", "~/Content/bootstrap.css");

                }
            }
        }
    }
}