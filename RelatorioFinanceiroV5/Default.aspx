<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RelatorioFinanceiroV5.Default" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .Absolute-Center {
            margin: auto;
            position: absolute;
            top: 0;
            left: 0;
            bottom: 0;
            right: 0;
        }

            .Absolute-Center.is-Responsive {
                width: 50%;
                height: 50%;
                min-width: 200px;
                max-width: 400px;
                padding: 40px;
            }

        #logo-container {
            
            margin: 100px auto;
            margin-bottom: 10px;
            width: 300px;
            height: 300px;
            background-image: url('http://localhost:50403/images/logo_gol.png');
            background-repeat: no-repeat;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="absolute-center is-responsive">
        <div id="logo-container">
        </div>
    </div>
    
</asp:Content>
