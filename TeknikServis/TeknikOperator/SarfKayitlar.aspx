<%@ Page Title="" Language="C#" MasterPageFile="~/TeknikOperator/Operator.Master" AutoEventWireup="true" CodeBehind="SarfKayitlar.aspx.cs" Inherits="TeknikServis.TeknikOperator.SarfKayitlar" %>

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
        <div id="Div5" runat="server" class="panel panel-info ">
            <div class="panel-heading">
                <h2 class="panel-title">
                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseGiris" class="collapsed">Tarih Aralığındaki Sarf Tüketimleri</a>
                </h2>
            </div>
            <div id="collapseGiris" class="panel-collapse in" style="height: 0px;">
                <div class="table-responsive">
                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                        <ContentTemplate>
                            <div id="cariOzetG" runat="server" class="pull-right">
                                <asp:Button Text="Sarf Kaydet" ID="btnSarfKaydet" CssClass="btn btn-danger" OnClick="btnSarfKaydet_Click" runat="server" />
                            </div>
                            <asp:GridView ID="grdAlimlarGirisler" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover" DataKeyNames="id"
                                EmptyDataText="Masraf girişi bulunmuyor" OnRowCommand="grdAlimlarGirisler_RowCommand"
                                AllowPaging="true" PageSize="10">

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
                                    <asp:BoundField DataField="masraf_adi" HeaderText="Masraf"></asp:BoundField>
                                    <asp:BoundField DataField="miktar" HeaderText="Miktar" />
                                    <asp:BoundField DataField="tarih" HeaderText="Tarih" />

                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="grdAlimlarGirisler" EventName="RowCommand" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

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
                                            EmptyDataText="Masraf kalemi seç" EnablePersistedSelection="true" OnSelectedIndexChanged="grdMasraf_SelectedIndexChanged">

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
                                            </Columns>
                                        </asp:GridView>

                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="txtAdetMasraf" CssClass="col-md-2 control-label">Adet/Miktar</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="txtAdetMasraf" TextMode="Number" ValidationGroup="detayGrupMasraf" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAdetMasraf" ValidationGroup="detayGrupMasraf" ErrorMessage="Lütfen ürün cinsi giriniz"></asp:RequiredFieldValidator>

                                    </div>
                                </div>


                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="txtDetayAciklamaMasraf" CssClass="col-md-2 control-label">Açıklama</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="txtDetayAciklamaMasraf" ValidationGroup="detayGrupMasraf" CssClass="form-control" />

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
                        <asp:AsyncPostBackTrigger ControlID="btnSarfKaydet" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnMasrafAra" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

    </div>
</asp:Content>
