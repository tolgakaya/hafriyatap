<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="Makineler.aspx.cs" Inherits="TeknikServis.TeknikMakine.Makineler" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="kaydir">


        <div class="panel panel-info">
            <!-- Default panel contents -->
            <div class="panel-heading">
                Makine Tanımları
            </div>
            <%--<div class="panel-body">
               
            </div>--%>
            <div class="table-responsive ">
                <div class="input-group custom-search-form">
                    <input runat="server" type="text" id="txtAra" class="form-control" placeholder="Ara..." />
                    <span class="input-group-btn">
                        <button id="btnARA" runat="server" class="btn btn-default" type="submit" onserverclick="MakineAra">
                            <i class="fa fa-search"></i>
                        </button>
                    </span>
                </div>
                <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                    <ProgressTemplate>
                        <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                            <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <asp:UpdatePanel ID="upCrudGrid" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grdAlimlar" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover" DataKeyNames="makine_id"
                            EmptyDataText="Stok kaydı" OnRowCommand="grdAlimlar_RowCommand"
                            AllowPaging="true" PageSize="10" OnPageIndexChanging="grdAlimlar_PageIndexChanging" OnRowCreated="grdAlimlar_RowCreated" OnRowDataBound="grdAlimlar_RowDataBound">

                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>

                                <%-- <%# Container.DataItemIndex %>' Text="<i class='fa fa-pencil'></i>" /> --%>
                                <asp:TemplateField HeaderStyle-Width="120">
                                    <ItemTemplate>

                                        <asp:LinkButton ID="btnDetay"
                                            runat="server"
                                            CssClass="btn  btn-success btn-xs"
                                            CommandName="detay" CommandArgument='<%# Eval("makine_id") %>' Text="<i class='fa fa-bar-chart'></i>" />
                                        <asp:LinkButton ID="btnMusteriler"
                                            runat="server"
                                            CssClass="btn  btn-info btn-xs "
                                            CommandName="musteri" CommandArgument='<%# Eval("makine_id") %>' Text="<i class='fa fa-cog'></i>" />
                                        <asp:LinkButton ID="btnGuncelle"
                                            runat="server"
                                            CssClass="btn btn-danger btn-xs"
                                            CommandName="guncelle" CommandArgument='<%#Eval("makine_id")+ ";" + Container.DisplayIndex  %>' Text="<i class='fa fa-pencil'></i>" />

                                    </ItemTemplate>


                                </asp:TemplateField>

                                <asp:BoundField DataField="adi" HeaderText="Adı"></asp:BoundField>
                                <asp:BoundField DataField="plaka" HeaderText="Plaka" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg"></asp:BoundField>
                                <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg"></asp:BoundField>
                                <asp:BoundField DataField="son_sayac" HeaderText="Son Sayaç" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                <asp:BoundField DataField="toplam_calisma_saat" HeaderText="Saat Çalışma" />
                                <asp:BoundField DataField="toplam_calisma_gun" HeaderText="Gün Çalışma" />
                                <asp:BoundField DataField="toplam_calisma_hafta" HeaderText="Hafta Çalışma" />
                                <asp:BoundField DataField="toplam_calisma_ay" HeaderText="Ay Çalışma" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                <asp:BoundField DataField="toplam_calisma_ay" HeaderText="Ay Çalışma" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                <asp:BoundField DataField="toplam_masraf_teorik" HeaderText="Masraf Teorik" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                <asp:BoundField DataField="toplam_gelir" HeaderText="Toplam Gelir" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                <asp:BoundField DataField="servis_sayaci" HeaderText="Servis Sayacı" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                <asp:BoundField DataField="toplam_masraf_gercek" HeaderText="Masraf Gerçek" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />

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

                    <asp:LinkButton ID="btnYeni"
                        runat="server"
                        CssClass="btn btn-info " OnClick="btnYeni_Click"
                        Text="Yeni Makine" />

                    <asp:LinkButton ID="btnPrint"
                        runat="server"
                        CssClass="btn btn-info visible-lg " OnClick="btnPrnt_Click"
                        Text="<i class='fa fa-print icon-2x'></i>" />
                    <asp:LinkButton ID="btnExportExcel"
                        runat="server"
                        CssClass="btn btn-info visible-lg" OnClick="btnExportExcel_Click"
                        Text="<i class='fa fa-file-excel-o icon-2x'></i>" />

                    <asp:LinkButton ID="btnExportWord"
                        runat="server"
                        CssClass="btn btn-info visible-lg" OnClick="btnExportWord_Click"
                        Text="<i class='fa fa-wikipedia-w icon-2x'></i>" />

                </div>


            </div>

        </div>
        <!-- yeni ürün Starts here-->
        <div id="cihazModal" class="modal  fade" tabindex="-1" role="dialog"
            aria-labelledby="cihazModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-content modal-lg">
                <div class="modal-header modal-header-info">
                    <button type="button" class="close" data-dismiss="modal"
                        aria-hidden="true">
                        ×</button>
                    <h3 id="cihazModalLabel" class="baslik">Yeni Makine Tanımla</h3>
                </div>
                <asp:UpdatePanel ID="upAdd2" runat="server">
                    <ContentTemplate>
                        <%--   <script type="text/javascript">
                                        Sys.Application.add_load(jScript);
                                    </script>--%>
                        <div class="modal-body">
                            <div class="form-horizontal">

                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="adi" CssClass="col-md-2 control-label">Makine Adı</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="adi" ValidationGroup="cihazGrup" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="adi" ValidationGroup="cihazGrup" ErrorMessage="Lütfen ürün cinsi giriniz"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="aciklama" CssClass="col-md-2 control-label">Makine Açıklama</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="aciklama" ValidationGroup="cihazGrup" CssClass="form-control" />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="plaka" CssClass="col-md-2 control-label">Plaka</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="plaka" ValidationGroup="cihazGrup" CssClass="form-control" />

                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="son_sayac" CssClass="col-md-2 control-label">Son Sayaç</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="son_sayac" ValidationGroup="cihazGrup" CssClass="form-control" />
                                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="servis_sayaci" Type="Currency" ValidationGroup="cihazGrup" CssClass="text-danger" MaximumValue="1000000" MinimumValue="0" ErrorMessage="Ondalık sayılar için virgül kullanınız."></asp:RangeValidator>

                                    </div>
                                </div>


                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="toplam_calisma_saat" CssClass="col-md-2 control-label">Top. Çalışma Saat</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="toplam_calisma_saat" ValidationGroup="cihazGrup" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="toplam_calisma_gun" CssClass="col-md-2 control-label">Top. Çalışma Gün</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="toplam_calisma_gun" ValidationGroup="cihazGrup" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="toplam_calisma_hafta" CssClass="col-md-2 control-label">Top. Çalışma Hafta</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="toplam_calisma_hafta" ValidationGroup="cihazGrup" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="toplam_calisma_ay" CssClass="col-md-2 control-label">Top. Çalışma Ay</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="toplam_calisma_ay" ValidationGroup="cihazGrup" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="toplam_masraf_teorik" CssClass="col-md-2 control-label">Top. Tanımlı Masraf</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="toplam_masraf_teorik" ValidationGroup="cihazGrup" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="toplam_masraf_gercek" CssClass="col-md-2 control-label">Top. Gerçek Masraf</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="toplam_masraf_gercek" ValidationGroup="cihazGrup" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="toplam_gelir" CssClass="col-md-2 control-label">Top. Gelir</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="toplam_gelir" ValidationGroup="cihazGrup" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="servis_sayaci" CssClass="col-md-2 control-label">Servis Sayacı</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="servis_sayaci" ValidationGroup="cihazGrup" CssClass="form-control" />
                                        <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="servis_sayaci" Type="Currency" ValidationGroup="cihazGrup" CssClass="text-danger" MaximumValue="1000000" MinimumValue="0" ErrorMessage="Ondalık sayılar için virgül kullanınız."></asp:RangeValidator>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnCihazKaydet" runat="server" Text="Kaydet"
                                CssClass="btn btn-info" OnClick="btnCihazKaydet_Click" ValidationGroup="cihazGrup" />
                            <button class="btn btn-info" data-dismiss="modal"
                                aria-hidden="true">
                                Kapat</button>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnYeni" EventName="Click" />

                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <%-- yeni ürün bitiyor --%>

        <div id="updateModal" class="modal  fade" tabindex="-1" role="dialog"
            aria-labelledby="updateModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-content modal-md">
                <div class="modal-header modal-header-primary">
                    <button type="button" class="close" data-dismiss="modal"
                        aria-hidden="true">
                        ×</button>
                    <h4 id="updateModalLabel" class="baslik">Makine Bilgisi</h4>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>

                        <div class="modal-body">


                            <div class="form-horizontal">

                                <asp:HiddenField ID="hdnCihazID" runat="server" />
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="dadi" CssClass="col-md-2 control-label">Makine Adı</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="dadi" ValidationGroup="cihazGrupdd" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="dadi" ValidationGroup="cihazGrupd" ErrorMessage="Lütfen ürün cinsi giriniz"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="daciklama" CssClass="col-md-2 control-label">Açıklama</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="daciklama" ValidationGroup="cihazGrupd" CssClass="form-control" />
                                     </div>
                                </div>

                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="dplaka" CssClass="col-md-2 control-label">Plaka</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="dplaka" ValidationGroup="cihazGrupd" CssClass="form-control" />

                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="dson_sayac" CssClass="col-md-2 control-label">Son Sayaç</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="dson_sayac" ValidationGroup="cihazGrupd" TextMode="Number" CssClass="form-control" />
                                        <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="dson_sayac" Type="Currency" ValidationGroup="cihazGrupd" CssClass="text-danger" MaximumValue="1000000" MinimumValue="0" ErrorMessage="Ondalık sayılar için virgül kullanınız."></asp:RangeValidator>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="dtoplam_calisma_saat" CssClass="col-md-2 control-label">Top. Çalışma Saat</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="dtoplam_calisma_saat" ValidationGroup="cihazGrupd" CssClass="form-control" />
                                     </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="dtoplam_calisma_gun" CssClass="col-md-2 control-label">Top. Çalışma Hafta</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="dtoplam_calisma_gun" ValidationGroup="cihazGrupd" CssClass="form-control" />
                                     </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="dtoplam_calisma_hafta" CssClass="col-md-2 control-label">Top. Çalışma Hafta</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="dtoplam_calisma_hafta" ValidationGroup="cihazGrupd" CssClass="form-control" />
                                     </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="dtoplam_calisma_ay" CssClass="col-md-2 control-label">Top. Çalışma Ay</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="dtoplam_calisma_ay" ValidationGroup="cihazGrupd" CssClass="form-control" />
                                     </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="dtoplam_masraf_teorik" CssClass="col-md-2 control-label">Top. Masraf</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="dtoplam_masraf_teorik" ValidationGroup="cihazGrupd" CssClass="form-control" />
                                     </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="dtoplam_masraf_teorik" CssClass="col-md-2 control-label">Top. Gerçek</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="dtoplam_masraf_gercek" ValidationGroup="cihazGrupd" CssClass="form-control" />
                                     </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="dtoplam_gelir" CssClass="col-md-2 control-label">Top. Gelir</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="dtoplam_gelir" ValidationGroup="cihazGrupd" CssClass="form-control" />
                                     </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="dservis_sayaci" CssClass="col-md-2 control-label">Servis Sayacı</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="dservis_sayaci" ValidationGroup="cihazGrupd" TextMode="Number" CssClass="form-control" />
                                        <asp:RangeValidator ID="RangeValidator4" runat="server" ControlToValidate="dservis_sayaci" Type="Currency" ValidationGroup="cihazGrupd" CssClass="text-danger" MaximumValue="1000000" MinimumValue="0" ErrorMessage="Ondalık sayılar için virgül kullanınız."></asp:RangeValidator>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnCihazUpdate" runat="server" Text="Kaydet"
                                CssClass="btn btn-info" OnClick="btnCihazUpdate_Click" ValidationGroup="cihazGrup_up" />
                            <button class="btn btn-info" data-dismiss="modal"
                                aria-hidden="true">
                                Kapat</button>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="grdAlimlar" EventName="RowCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

    </div>


</asp:Content>
