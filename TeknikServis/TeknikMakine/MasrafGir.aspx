<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="MasrafGir.aspx.cs" Inherits="TeknikServis.TeknikMakine.MasrafGir" %>

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

    <div class="row kaydir">
        <asp:UpdateProgress ID="UpdateProgress2" runat="server">
            <ProgressTemplate>
                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <div class="panel panel-info">
            <div class="panel-heading">
                <h1 id="baslik" runat="server"></h1>
            </div>
            <div class="panel-body">
                <div class="panel-group" id="accordion">

                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseTwo">Sarf Malzeme Kalemleri</a>
                            </h4>
                        </div>
                        <div id="collapseTwo" class="panel-collapse in" style="height: auto;">
                            <div class="panel-body">

                                <!-- Ürün seçim alanı başlıyor -->

                                <div class="table-responsive">

                                    <asp:UpdatePanel ID="upCrudGrid2" runat="server">
                                        <ContentTemplate>

                                            <asp:GridView ID="grdDetay" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="masraf_id"
                                                EmptyDataText="Detay bilgileri" EnablePersistedSelection="true" OnRowCommand="grdDetay_RowCommand" OnSelectedIndexChanged="grdDetay_SelectedIndexChanged">

                                                <SelectedRowStyle CssClass="danger" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>


                                                            <asp:LinkButton ID="delLink"
                                                                runat="server"
                                                                CssClass="btn btn-danger btn-xs"
                                                                CommandName="del" CommandArgument='<%#Eval("masraf_id") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />


                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                        <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="masraf_id" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="masraf_adi" HeaderText="Masraf Adı" />
                                                    <asp:BoundField DataField="miktar" HeaderText="Miktar" />
                                                    <asp:BoundField DataField="birim" HeaderText="Birim" />
                                                    <asp:BoundField DataField="tutar" HeaderText="Tutar" />
                                                    <asp:BoundField DataField="tarih" HeaderText="Tarih" />

                                                    <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg"></asp:BoundField>
                                                </Columns>
                                            </asp:GridView>
                                            <asp:Button Text="Sarf Ekle" ID="btnDetayEkleMasraf" OnClick="btnDetayEkleMasraf_Click" CssClass="btn btn-primary" runat="server" />
                                            <asp:Button ID="btnAlimKaydet" CssClass="btn btn-info" runat="server" CausesValidation="true" ValidationGroup="valGrup" Text="Kaydet" OnClick="btnAlimKaydet_Click" />

                                        </ContentTemplate>
                                        <%--   <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnDetayKaydet" EventName="Click" />
                                        </Triggers>--%>
                                    </asp:UpdatePanel>


                                </div>

                                <!-- Ürün seçimalanı bitiyor-->
                            </div>
                        </div>
                    </div>
                    <!-- detay Modal Starts here-->


                    <div id="detayModalMasraf" class="modal  fade" tabindex="-1" role="dialog"
                        aria-labelledby="detayModalLabelMasraf" aria-hidden="true">
                        <div class="modal-dialog modal-content modal-lg">
                            <div class="modal-header modal-header-info">
                                <button type="button" class="close" data-dismiss="modal"
                                    aria-hidden="true">
                                    ×</button>
                                <h3 id="detayModalLabelMasraf" class="baslik">Sarf Kalemi Ekleyin</h3>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>

                                    <div class="modal-body">

                                        <asp:Panel runat="server" ID="panel1" DefaultButton="btnMasrafAra">
                                            <div class="input-group custom-search-form col-md-12">
                                                <input runat="server" type="text" id="txtMasrafAra" class="form-control" placeholder="Ara..." />
                                                <span class="input-group-btn">
                                                    <asp:LinkButton ID="btnMasrafAra"
                                                        runat="server"
                                                        CssClass="btn btn-info" OnClick="MasrafAra"
                                                        Text="<i class='fa fa-search icon-2x'></i>" />
                                                </span>
                                            </div>
                                        </asp:Panel>

                                        <div class="form-horizontal" id="masraf">
                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="grdMasraf" CssClass="col-md-12 control-label">Masraf Seçimi</asp:Label>
                                                <div class="col-md-12">
                                                    <asp:GridView ID="grdMasraf" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="MasrafID"
                                                        EmptyDataText="Masraf kalemi seç" EnablePersistedSelection="true" OnRowCommand="grdMasraf_RowCommand" OnSelectedIndexChanged="grdMasraf_SelectedIndexChanged">

                                                        <SelectedRowStyle CssClass="danger" />
                                                        <Columns>

                                                            <asp:ButtonField CommandName="Select" ControlStyle-CssClass="btn btn-info" ButtonType="Button" Text="Seç" HeaderText="Seçim">
                                                                <ControlStyle CssClass="btn btn-primary"></ControlStyle>
                                                            </asp:ButtonField>
                                                            <asp:BoundField DataField="MasrafID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="adi" HeaderText="Masraf" />

                                                            <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="giris" HeaderText="Toplam Giriş" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="cikis" HeaderText="Toplam Çıkış" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="bakiye" HeaderText="Stok Miktarı" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="birim_maliyet" HeaderText="Birim Maliyet" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="birim" HeaderText="Birim" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <%--    <asp:Button ID="btnYeniMasraf" runat="server" Text="Yeni Tanımla"
                                                        CssClass="btn btn-info" OnClick="btnYeniMasraf_Click" />--%>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="txtAdetMasraf" CssClass="col-md-2 control-label">Miktar</asp:Label>
                                                <div class="col-md-8">
                                                    <asp:TextBox runat="server" ID="txtAdetMasraf" ValidationGroup="detayGrupMasraf" CssClass="form-control" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAdetMasraf" ValidationGroup="detayGrupMasraf" ErrorMessage="Lütfen ürün cinsi giriniz"></asp:RequiredFieldValidator>
                                                    <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="txtAdetMasraf" Type="Currency" ValidationGroup="detayGrupMasraf" CssClass="text-danger" MaximumValue="1000000" MinimumValue="0" ErrorMessage="Ondalık sayılar için virgül kullanınız."></asp:RangeValidator>

                                                </div>
                                                <asp:Label ID="lblBirimMasraf" AssociatedControlID="txtAdetMasraf" CssClass="col-md-1 control-label" runat="server" />
                                            </div>


                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="txtDetayAciklamaMasraf" CssClass="col-md-2 control-label">Açıklama</asp:Label>
                                                <div class="col-md-8">
                                                    <asp:TextBox runat="server" ID="txtDetayAciklamaMasraf" ValidationGroup="detayGrupMasraf" CssClass="form-control" />
                                                </div>

                                            </div>
                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="chcSayacSifirla" CssClass="col-md-2 control-label">Servis Sayacı</asp:Label>
                                                <div class="col-md-8">
                                                    <div class=" form-control">
                                                        <asp:CheckBox ID="chcSayacSifirla" Text="Sıfırla" runat="server" />
                                                    </div>
                                                </div>

                                            </div>

                                        </div>

                                    </div>

                                    <div class="modal-footer">
                                        <asp:Button ID="btnDetayKaydetMasraf" runat="server" Text="Detay Kaydet"
                                            CssClass="btn btn-info" OnClick="btnDetayKaydetMasraf_Click" ValidationGroup="detayGrupMasraf" />
                                        <button class="btn btn-info" data-dismiss="modal"
                                            aria-hidden="true">
                                            Kapat</button>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnDetayEkleMasraf" EventName="Click" />
                                    <%--<asp:AsyncPostBackTrigger ControlID="btnMasrafKaydet" EventName="Click" />--%>
                                    <asp:AsyncPostBackTrigger ControlID="btnMasrafAra" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>


                </div>
            </div>
        </div>

    </div>
    <script type="text/javascript">
        function pageLoad(sender, args) {

            $('#ContentPlaceHolder1_tarih2').datetimepicker({
                format: 'L',

                locale: 'tr'
            });

        }
    </script>
</asp:Content>
