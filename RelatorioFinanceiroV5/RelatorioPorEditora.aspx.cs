using MySql.Data.MySqlClient;
using RelatorioFinanceiroV5.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RelatorioFinanceiroV5
{
    public partial class RelatorioPorEditora : System.Web.UI.Page
    {
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
                dt = Service.getQuantidadeConteudoPorEditora(mesReferencia, conn);


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

        protected void btnPDF_Click(object sender, EventArgs e)
        {

        }

        protected void GridViewQuantidades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Visualizar")
            {
                var myConn = Connection.conn();
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = GridViewQuantidades.Rows[index];
               
                int idEditora = (int)Convert.ToInt32(row.Cells[2].Text);
                string mesReferencia = row.Cells[5].Text;

                //calcula o percentual da quantidade sobre o total de conteudos

                int quantidadeTotal = Service.QuantidadeTotalPorEditora(myConn, mesReferencia);
                decimal percentual = Math.Round(Convert.ToDecimal(row.Cells[4].Text) / (decimal)quantidadeTotal * 100,6);

                lblPercentualEditoraTotal.Text = Math.Round(percentual, 2).ToString() + "%";

                int maisAcessados = Service.GetMaisAcessados(myConn, mesReferencia, idEditora);
                int totalRefxMaisAcessados = Service.TotalReferenciaMaisAcessadosPorEditora(myConn, mesReferencia);
                lblTotalRefMaisAcessados.Text = maisAcessados.ToString();

                decimal percentualReferenciaMaisAcessado = Convert.ToDecimal(maisAcessados / (decimal)totalRefxMaisAcessados * 100);

                lblPercentual10MaisAcessados.Text = Math.Round(percentualReferenciaMaisAcessado, 2).ToString() + "%";



                decimal receita = Service.GetReceita(myConn, mesReferencia);
                lblReceita.Text = receita.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"));


                decimal receita20 = Math.Round((decimal)receita * (decimal)0.2, 8);
                decimal receita10 = Math.Round((decimal)receita * (decimal)0.1, 8);


                lblReceita_20.Text = receita20.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"));
                lblReceita_10.Text = receita10.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"));

                decimal receitaTotal = receita10 + receita20;
                lblReceitaTotalASerDividida.Text = receitaTotal.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"));

                //ok ate aqui

                decimal valorASerRepassadoPelaQuantidade = Math.Round(percentual * receita20 / 100, 8);
                lblValorRepasseQuantidade.Text = valorASerRepassadoPelaQuantidade.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"));

                decimal valorASerRepassadoPelaReferMaisAcessados = Math.Round(percentualReferenciaMaisAcessado * receita10 / 100, 8);
                lblValorRepasseRefMaisAcessados.Text = valorASerRepassadoPelaReferMaisAcessados.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"));

                decimal valorTotalRepasse = valorASerRepassadoPelaQuantidade + valorASerRepassadoPelaReferMaisAcessados;
                lblValorTotalRepasse.Text = valorTotalRepasse.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"));

                lblGrupo.Text = row.Cells[1].Text;
                lblMes.Text = row.Cells[5].Text;
                lblQuantidadeConteudos.Text = row.Cells[4].Text;

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "openModal();", true);
            }
        }
    }
}