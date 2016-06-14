<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RelatorioPorGrupo.aspx.cs" Inherits="RelatorioFinanceiroV5.RelatorioPorGrupo" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Scripts/jquery-2.2.3.js"></script>
    <script src="Scripts/bootstrap.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholder1" runat="server">

    <div>
        <div class="panel panel-default">
            <div class="panel-heading">
                <div class="text-center">
                    <h2><strong>Relatório Financeiro - Por Grupo</strong></h2>
                </div>
            </div>
            <div class="panel-body" runat="server" id="pnlbody">
                <asp:GridView AutoGenerateColumns="false" ID="GridViewQuantidades" ShowFooter="true" runat="server" CssClass="table table-bordered table-striped" OnRowDataBound="GridViewQuantidades_RowDataBound" OnRowCommand="GridViewQuantidades_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="ID" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle ForeColor="White" BackColor="#006372" VerticalAlign="Middle" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="nome" HeaderText="Grupo" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle ForeColor="White" BackColor="#006372" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="id_grupo" HeaderText="Id Grupo" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle ForeColor="White" BackColor="#006372" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="quantidade" HeaderText="Quantidade" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle ForeColor="White" BackColor="#006372" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <%--  <asp:TemplateField HeaderText="Quantidade">
                        <ItemTemplate>
                            <asp:Label ID="lblQuantidade" runat="server" Text='<%# Eval("quantidade") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblTotal" runat="server"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>--%>

                        <asp:BoundField DataField="mes_referencia" HeaderText="Mês Referência" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle ForeColor="White" BackColor="#006372" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle ForeColor="White" BackColor="#006372" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:Button ID="btnVisualizar" runat="server" Text="Visualizar" CommandName="Visualizar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CssClass="btn btn-primary btn-sm" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

            <!-- Modal -->
            <div class="modal fade bs-example-modal-lg" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
                <div class="modal-dialog modal-lg" role="dialog">
                    <div class="modal-content">
                        <%--<div class="modal-header">
                        <img src="../images/cabecalho.png" />
                    </div>--%>
                        <div class="modal-body">
                            <div class="panel panel-default" id="pnlPDF" runat="server">
                                <table class="table table-bordered">
                                    <thead>
                                        <tr>
                                            <th colspan="2" class="table-header">
                                                <h3 class="text-center">Relatório Financeiro - Nuvem de Livros</h3>
                                            </th>
                                        </tr>
                                        <tr class="table-sub-header">
                                            <th>
                                                <strong>
                                                    <asp:Label ID="lblGrupo" runat="server"></asp:Label></strong>
                                            </th>
                                            <th>
                                                <strong>
                                                    <asp:Label ID="lblMes" runat="server"></asp:Label></strong>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr class="sub-sub-header">
                                            <td colspan="2" class="text-center"><strong>Número de Ítens da Editora</strong></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <i>Quantidade de Conteúdos</i>
                                            </td>
                                            <td class="text-center">
                                                <strong>
                                                    <asp:Label ID="lblQuantidadeConteudos" runat="server" Text="999"></asp:Label></strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <i>% da editora do total</i>
                                            </td>
                                            <td class="text-center">
                                                <strong>
                                                    <asp:Label ID="lblPercentualEditoraTotal" runat="server" Text="0.2334%"></asp:Label></strong>
                                            </td>
                                        </tr>
                                        <tr class="sub-sub-header">
                                            <td colspan="2" class="text-center">
                                                <strong>Número de Ítens da Editora</strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <i>Conteúdo de ref. e mais acessados</i>
                                            </td>
                                            <td class="text-center">
                                                <strong>
                                                    <asp:Label ID="lblTotalRefMaisAcessados" runat="server"></asp:Label></strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <i>% da editora dos 10% mais acessados e referência</i>
                                            </td>
                                            <td class="text-center">
                                                <strong>
                                                    <asp:Label ID="lblPercentual10MaisAcessados" runat="server" Text="Label"></asp:Label></strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <i>Receita líquida total da Nuvem de Livros</i>
                                            </td>
                                            <td>
                                                <strong>
                                                    <asp:Label ID="lblReceita" runat="server" CssClass="pull-right"></asp:Label></strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <i>Receita a ser dividida entre as editoras pelo conteúdo (20%)</i>
                                            </td>
                                            <td>
                                                <strong>
                                                    <asp:Label ID="lblReceita_20" runat="server" Text="Label" CssClass="pull-right"></asp:Label></strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <i>Receita a ser dividida entre as editoras pelas obras de referência e mais acessados (10%)</i>
                                            </td>
                                            <td>
                                                <strong>
                                                    <asp:Label ID="lblReceita_10" runat="server" Text="Label" CssClass="pull-right"></asp:Label></strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <i>Receita total a ser dividida entre as editoras</i>
                                            </td>
                                            <td>
                                                <strong>
                                                    <asp:Label ID="lblReceitaTotalASerDividida" runat="server" CssClass="pull-right"></asp:Label></strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <i>Valor a ser repassado para a editora pela quantidade de conteúdos</i>
                                            </td>
                                            <td>
                                                <strong>
                                                    <asp:Label ID="lblValorRepasseQuantidade" runat="server" CssClass="pull-right"></asp:Label></strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <i>Valor a ser repassado para a editora pelas obras de referência e mais acessados</i>
                                            </td>
                                            <td>
                                                <strong>
                                                    <asp:Label ID="lblValorRepasseRefMaisAcessados" runat="server" Text="Label" CssClass="pull-right"></asp:Label></strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <i>Valor total ser repassado para a editora</i>
                                            </td>
                                            <td>
                                                <strong>
                                                    <asp:Label ID="lblValorTotalRepasse" runat="server" CssClass="pull-right"></asp:Label></strong>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>

                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnClose" runat="server" data-dismiss="modal" CssClass="btn btn-default" Text="Close" />
                            <asp:Button ID="btnPDF" runat="server" Text="PDF" CssClass="btn btn-primary" OnClick="btnPDF_Click" />

                        </div>
                    </div>
                </div>
            </div>


        </div>


    </div>
    <script type="text/javascript">
        if (typeof jQuery == 'undefined') {
            var script = document.createElement('script');
            script.type = "text/javascript";
            script.src = "http://ajax.googleapis.com/ajax/libs/jquery/2.2.3/jquery.min.js";
            document.getElementsByTagName('head')[0].appendChild(script);
        }
    </script>

    <script type="text/javascript">
        function openModal() {
            console.log("passei aqui");
            $('#myModal').modal('show');
        }
    </script>

</asp:Content>




