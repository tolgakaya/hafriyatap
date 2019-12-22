﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="MasrafGirisler.aspx.cs" Inherits="TeknikServis.TeknikMakine.MasrafGirisler" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";

            if (confirm("Kaydı silmek istiyor musunuz?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }

    </script>

    <div class="kaydir">

        <div id="panelContents" runat="server" class="panel panel-info">
            <div class="panel-heading">
                <h4 class="panel-title">
                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseOne" class="collapsed">Arama Kriterleri</a>
                </h4>
            </div>

            <div id="collapseOne" class="panel-collapse collapse" style="height: 0px;">
                <div class="panel-body">
                    <div class="row ">
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label for="datetimepicker6">Şu Tarihten:</label>
                                <input type='text' runat="server" class="form-control" id="datetimepicker6" />

                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label for="datetimepicker7">Şu Tarihe:</label>
                                <input type='text' runat="server" class="form-control" id="datetimepicker7" />
                            </div>
                        </div>
                    </div>


                    <div class="form-group">

                        <button id="btnAra" runat="server" class="btn btn-info btn-lg btn-block" type="submit" onserverclick="AlimAra">Ara...</button>
                    </div>
                    <!--body-->
                </div>
            </div>
        </div>

        <div class="panel panel-info">
            <!-- Default panel contents -->
            <div class="panel-heading">
                Tarihe Göre Makine Masraf Girişleri
            </div>
            <%--<div class="panel-body">
               
            </div>--%>
            <div class="table-responsive ">

                <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                    <ProgressTemplate>

                        <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                            <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                        </div>

                    </ProgressTemplate>
                </asp:UpdateProgress>

                <asp:UpdatePanel ID="upCrudGrid" runat="server">
                    <ContentTemplate>
                        <div id="cariOzet" runat="server" class="pull-right">
                            <span id="txtAdet" runat="server" class="label label-success"></span>
                            <span id="txtMiktar" runat="server" class="label label-success"></span>
                            <span id="txtTutar" runat="server" class="label label-warning"></span>
                        </div>

                        <asp:GridView ID="grdAlimlar" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover" DataKeyNames="id"
                            EmptyDataText="Satın alma kaydı bulunmuyor" OnRowCommand="grdAlimlar_RowCommand"
                            AllowPaging="true" PageSize="10" OnPageIndexChanging="grdAlimlar_PageIndexChanging" OnRowCreated="grdAlimlar_RowCreated" OnRowDataBound="grdAlimlar_RowDataBound">

                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>

                                <asp:TemplateField>
                                    <ItemTemplate>

                                        <asp:LinkButton ID="btnDel"
                                            runat="server"
                                            CssClass="btn btn-danger"
                                            CommandName="del" CommandArgument='<%# Eval("id") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />
                                        </div>
                                    </ItemTemplate>


                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Adı" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("makine_adi") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>

                                        <asp:LinkButton ID="btnTedarikci"
                                            runat="server"
                                            CssClass="btn btn-primary"
                                            CommandName="detail" CommandArgument='<%#Eval("makine_id") %>' Text=' <%#Eval("makine_adi") %> '>
                           
                                        </asp:LinkButton>
                                    </ItemTemplate>

                                </asp:TemplateField>

                                <asp:BoundField DataField="makine_plaka" HeaderText="Plaka" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg"></asp:BoundField>
                                <asp:BoundField DataField="masraf_id" HeaderText="MasrafID"></asp:BoundField>
                                <asp:BoundField DataField="masraf_adi" HeaderText="Masraf" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg"></asp:BoundField>
                                <asp:BoundField DataField="miktar" HeaderText="Miktar" DataFormatString="{0:C}" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                <asp:BoundField DataField="tutar" HeaderText="Tutar" DataFormatString="{0:C}" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                <asp:BoundField DataField="aciklama" HeaderText="Açıklama" />
                                <asp:BoundField DataField="tarih" HeaderText="Tarih" DataFormatString="{0:D}" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />

                            </Columns>

                        </asp:GridView>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAra" EventName="ServerClick" />

                    </Triggers>
                </asp:UpdatePanel>

            </div>
            <div class="panel-footer pull-right">
                <div class=" btn-group">

                   
                    <asp:LinkButton ID="btnPrint"
                        runat="server"
                        CssClass="btn btn-info " OnClick="btnPrnt_Click"
                        Text="<i class='fa fa-print icon-2x'></i>" />

                    <asp:LinkButton ID="btnExportExcel"
                        runat="server"
                        CssClass="btn btn-info " OnClick="btnExportExcel_Click"
                        Text="<i class='fa fa-file-excel-o icon-2x'></i>" />

                    <asp:LinkButton ID="btnExportWord"
                        runat="server"
                        CssClass="btn btn-info " OnClick="btnExportWord_Click"
                        Text="<i class='fa fa-wikipedia-w icon-2x'></i>" />
                    <asp:LinkButton ID="btnMusteriDetayim"
                        runat="server" Visible="false"
                        CssClass="btn btn-info " OnClick="btnMusteriDetayim_Click"
                        Text="<i class='fa fa-user icon-2x'></i>" />

                </div>

            </div>

        </div>

    </div>

    <script type="text/javascript">
        $(function () {
            $('#ContentPlaceHolder1_datetimepicker6').datetimepicker({
                format: 'L',

                locale: 'tr'
            });
            $('#ContentPlaceHolder1_datetimepicker7').datetimepicker({
                format: 'L',

                locale: 'tr'
            });
            $("#ContentPlaceHolder1_datetimepicker6").on("dp.change", function (e) {
                $('#ContentPlaceHolder1_datetimepicker7').data("DateTimePicker").minDate(e.date);
            });
            $("#ContentPlaceHolder1_datetimepicker7").on("dp.change", function (e) {
                $('#ContentPlaceHolder1_datetimepicker6').data("DateTimePicker").maxDate(e.date);
            });
        });

    </script>
</asp:Content>
