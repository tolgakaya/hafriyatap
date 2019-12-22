<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="HizliKirala2.aspx.cs" Inherits="TeknikServis.TeknikMakine.HizliKirala2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="kaydir">

        <asp:UpdateProgress ID="UpdateProgress2" runat="server">
            <ProgressTemplate>
                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>


        <div class="panel panel-info">
            <div class="panel-heading">
                HIZLI KİRALAMA KAYDI
            </div>
            <div class="panel-body">

                <div class="panel panel-info">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            <a data-toggle="collapse" data-parent="#accordion" href="#collapseYeniMakine" class="collapsed">Makine Seçimi</a>
                        </h4>
                    </div>
                    <div id="collapseYeniMakine" class="panel-collapse collapse" style="height: 0px;">
                        <div class="panel-body">
                            <!-- Müşteri seçim alanı başlıyor -->

                            <div class="table-responsive">
                                <div class="input-group custom-search-form ">
                                    <span class="input-group-btn">
                                        <button id="btnMakine" runat="server" class="btn btn-default" type="submit" onserverclick="MakineAra">
                                            <i class="fa fa-search"></i>
                                        </button>
                                    </span>
                                    <input runat="server" type="text" id="txtMakineAra" class="form-control" placeholder="Ara..." />

                                </div>

                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="grdMakine" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="makine_id"
                                            EmptyDataText="Ürün/Parça girilmemiş" OnSelectedIndexChanged="grdMakine_SelectedIndexChanged">
                                            <SelectedRowStyle CssClass="danger" />
                                            <Columns>
                                                <asp:ButtonField CommandName="Select" ControlStyle-CssClass="btn btn-danger" ButtonType="Button" Text="Seç" HeaderText="Seçim">
                                                    <ControlStyle CssClass="btn btn-danger"></ControlStyle>
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="makine_id" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                    <HeaderStyle CssClass="visible-lg" />
                                                    <ItemStyle CssClass="visible-lg" />
                                                </asp:BoundField>

                                                <asp:TemplateField HeaderText="Makine Adı" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">

                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnRandom"
                                                            runat="server"
                                                            CssClass="btn btn-primary"
                                                            CommandName="detail" CommandArgument='<%#Eval("makine_id") %>' Text=' <%#Eval("adi") %> '>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>

                                                </asp:TemplateField>
                                                <asp:BoundField DataField="plaka" HeaderText="Plaka" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                    <HeaderStyle CssClass="visible-lg" />
                                                    <ItemStyle CssClass="visible-lg" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                    <HeaderStyle CssClass="visible-lg" />
                                                    <ItemStyle CssClass="visible-lg" />
                                                </asp:BoundField>

                                            </Columns>

                                        </asp:GridView>

                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnMakine" EventName="ServerClick" />
                                        <asp:AsyncPostBackTrigger ControlID="grdMakine" EventName="RowCommand" />
                                
                                    </Triggers>
                                </asp:UpdatePanel>


                            </div>

                            <!-- Müşteri seç,malanı bitiyor-->
                        </div>
                    </div>
                </div>

                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                    <ContentTemplate>

                        <div class="panel panel-info">
                            <div class="panel-heading">
                                <h4 class="panel-title">
                                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseKararilMakine">Makine Çalışma Bilgileri</a>
                                </h4>
                            </div>
                            <div id="collapseKararilMakine" class="panel-collapse in" style="height: auto;">
                                <div class="panel-body">
                                    <div class="form-horizontal">

                                        <div class="form-group">
                                            <label class="col-sm-2 control-label" for="drdTarife">Tarife</label>
                                            <div class="col-sm-4">
                                                <asp:DropDownList ID="drdTarife" runat="server" AutoPostBack="true" class="form-control" OnSelectedIndexChanged="drdTarife_SelectedIndexChanged">
                                                    <asp:ListItem Text="Saat" Value="saat" Selected="True" />
                                                    <asp:ListItem Text="Gün" Value="gun" />
                                                    <asp:ListItem Text="Hafta" Value="hafta" />
                                                    <asp:ListItem Text="Ay" Value="ay" />
                                                </asp:DropDownList>

                                            </div>
                                            <label class="col-sm-2 control-label" for="txtFiyat">Birim Ücret</label>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" ID="txtFiyat" Enabled="false" Text="10" class="form-control" />
                                            </div>

                                        </div>

                                        <div runat="server" id="tarih_aralik">
                                            <div class="form-group">
                                                <label class="col-sm-2 control-label" for="datetimepicker6">Başlama</label>
                                                <div class="col-sm-4">
                                                    <%--<input type='text' runat="server" class="form-control" id="datetimepicker6" />--%>
                                                    <asp:TextBox runat="server" ID="datetimepicker6" class="form-control" AutoPostBack="true" />

                                                </div>
                                                <label class="col-sm-2 control-label" for="datetimepicker7">Bitiş</label>
                                                <div class="col-sm-4">
                                                    <%--<input type='text' runat="server" class="form-control" id="datetimepicker7" />--%>
                                                    <asp:TextBox runat="server" ID="datetimepicker7" class="form-control" AutoPostBack="true" />
                                                </div>

                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-2 control-label" for="txtSure">Süre</label>
                                                <div class="input-group col-sm-6 ">
                                                    <asp:TextBox runat="server" ID="txtSure" class="form-control" AutoPostBack="true" OnTextChanged="txtSure_TextChanged" />
                                                    <span class="input-group-btn">
                                                        <asp:Button ID="btnSureHesapla" runat="server" Text="Hesapla" CssClass="btn btn-info" OnClick="btnSureHesapla_Click" />
                                                    </span>

                                                </div>
                                            </div>

                                        </div>

                                        <div runat="server" id="numara_aralik">
                                            <div class="form-group">
                                                <label for="txtSonNumara" class="col-sm-2 control-label">Başlama</label>
                                                <div class="col-sm-4">

                                                    <asp:TextBox runat="server" ID="txtSonNumara"  AutoPostBack="true" OnTextChanged="txtBaslangicChanged" class="form-control" />
                                                </div>
                                                <label for="txtYeniNumara" class="col-sm-2 control-label">Bitiş</label>
                                                <div class="col-sm-4">

                                                    <asp:TextBox runat="server" ID="txtYeniNumara"  class="form-control" AutoPostBack="true" OnTextChanged="txtBaslangicChanged" />
                                                </div>
                                                <asp:HiddenField ID="hdnSaatlik" runat="server" />
                                                <asp:HiddenField ID="hdnTarifeTipi" runat="server" />

                                            </div>
                                            <div class="form-group">
                                                <label for="txtDakika" class="col-sm-2 control-label">Dakika</label>
                                                <div class="col-sm-4">

                                                    <asp:TextBox runat="server" ID="txtDakika" AutoPostBack="true" class="form-control" OnTextChanged="txtSaat_TextChanged" />
                                                </div>
                                                <label for="txtSaatBilgi" class="col-sm-2 control-label">Saat</label>
                                                <div class="col-sm-4">

                                                    <asp:TextBox runat="server" ID="txtSaatBilgi" Enabled="false" class="form-control" />
                                                </div>
                                            </div>
                                        </div>



                                        <div class="form-group">
                                            <label class="col-sm-2 control-label" for="txtMakineAdiGoster">Makine</label>
                                            <div class="col-sm-10">
                                                <input id="txtMakineAdiGoster" runat="server" type="text" class="form-control" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label" for="txtIslemParcaMakine">Yapılacak İşlem</label>
                                            <div class="col-sm-10">
                                                <input id="txtIslemParcaMakine" runat="server" type="text" class="form-control" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" EnableClientScript="true" CssClass="text-danger" ControlToValidate="txtIslemParcaMakine" ErrorMessage="Lütfen yapılacak işlemi giriniz" ValidationGroup="valGrupMakine"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>

                                        <div class="form-group">

                                            <label class="col-sm-2 control-label" for="txtKDVOraniDuzenleMakine">KDV Oranı</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="txtKDVOraniDuzenleMakine" runat="server" Text="18" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div hidden class="form-group">

                                            <label class="col-sm-2 control-label" for="txtKDVDuzenleMakine">KDV Tutarı</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="txtKDVDuzenleMakine" Enabled="false" runat="server" Text="18" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">

                                            <label class="col-sm-2 control-label" for="txtYekunMakine">Tutar(KDV Dahil)</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="txtYekunMakine" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" EnableClientScript="true" ControlToValidate="txtYekunMakine" ErrorMessage="Lütfen toplam tutar giriniz" CssClass="text-danger" ValidationGroup="valGrupMakine"></asp:RequiredFieldValidator>
                                                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtYekunMakine" Type="Currency" ValidationGroup="valGrupMakine" CssClass="text-danger" MaximumValue="1000000" MinimumValue="0" ErrorMessage="Ondalık sayılar için virgül kullanınız."></asp:RangeValidator>
                                                <asp:HiddenField ID="hdnHesapIDDuzenMakine" runat="server" />
                                            </div>
                                        </div>
                                        <div class="form-group">

                                            <label class="col-sm-2 control-label" for="txtAciklamaMakine">Açıklama</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="txtAciklamaMakine" runat="server" TextMode="MultiLine" CssClass="form-control" ValidationGroup="valGrupMakine"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group">

                                            <label for="txtTarihMakine" class="col-md-2 control-label">İşlem Tarihi</label>
                                            <div class="col-md-10">

                                                <input type='text' id="txtTarihMakine" runat="server" class="form-control" />

                                            </div>
                                        </div>
                                               <div class="form-group">
                                            <label class="col-sm-2 control-label" for="txtCariID">Kişi/Firma Seç</label>
                                            <div class="col-sm-10">
                                    <div class="input-group custom-search-form ">
                                    <span class="input-group-btn">
                                        <button id="Button1" runat="server" class="btn btn-default" type="submit" onserverclick="btnCari_Click">
                                            <i class="fa fa-search">Seç</i>
                                        </button>
                                    </span>
                                    <input runat="server" type="text" id="txtCariID" class="form-control" placeholder="Cari seç..." />
                                    </div
                                            </div>
                                
                                        </div>
                                               <asp:HiddenField ID="hdnCari" runat="server" />

                                        <div class="form-group">
                                            <asp:Button ID="btnKaydetMakine" runat="server" Text="Kaydet" CausesValidation="true" ValidationGroup="valGrupMakine" CssClass="btn btn-info btn-block" OnClick="btnKaydetMakine_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="grdMakine" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>

            </div>
        </div>
        <div id="musteriModal" class="modal  fade" tabindex="-1" role="dialog" aria-labelledby="tamirciModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-content modal-lg">
                <div class="modal-header modal-header-info">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="tamirciModalLabel" class="baslik">Müşteri Seçimi</h3>
                </div>

                <div class="modal-body">

                    <%--  <div class="panel panel-info">
                                <div class="panel-heading">
                                    SERVİS HESAP VE KARARI
                                </div>--%>
                    <%-- %> <div class="panel-body"> --%>

                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseMusteri" class="collapsed">Müşteri Seçimi</a>
                            </h4>
                        </div>


                        <%-- müşteri aramayı ekleyecez --%>
                        <div id="collapseMusteri" class="panel-collapse in" style="height: auto;">
                            <div class="panel-body">
                                <!-- Müşteri seçim alanı başlıyor -->

                                <div class="table-responsive">
                                    <asp:Panel runat="server" ID="musteriPanel" DefaultButton="btnMusteriAra">
                                        <div class="input-group custom-search-form">

                                            <input runat="server" type="text" id="txtMusteriSorgu" class="form-control" placeholder="Ara..." />
                                            <span class="input-group-btn">
                                                <asp:Button Text="Ara" runat="server" ID="btnMusteriAra" class="btn btn-default" OnClick="MusteriAra" />
                                            </span>



                                        </div>
                                    </asp:Panel>
                                </div>

                                <!-- Müşteri seç,malanı bitiyor-->
                            </div>
                        </div>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanelKarar" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="grdMusteri" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="CustID"
                                EmptyDataText="Kayıt girilmemiş" EnablePersistedSelection="false"
                                OnSelectedIndexChanged="grdMusteri_SelectedIndexChanged">
                                <SelectedRowStyle CssClass="danger" />

                                <Columns>
                                    <asp:ButtonField CommandName="Select" ControlStyle-CssClass="btn btn-danger" ButtonType="Button" Text="Seç">
                                        <ControlStyle CssClass="btn btn-danger"></ControlStyle>
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="CustID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                        <HeaderStyle CssClass="visible-lg" />
                                        <ItemStyle CssClass="visible-lg" />
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="Müşteri Adı" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnRandom"
                                                runat="server"
                                                CssClass="btn btn-primary"
                                                CommandName="detail" CommandArgument='<%#Eval("CustID") %>' Text=' <%#Eval("Ad") %> '>  </asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                        <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Adres" HeaderText="Adres" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                        <HeaderStyle CssClass="visible-lg" />
                                        <ItemStyle CssClass="visible-lg" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Telefon" HeaderText="Telefon" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                        <HeaderStyle CssClass="visible-lg" />
                                        <ItemStyle CssClass="visible-lg" />
                                    </asp:BoundField>

                                </Columns>

                            </asp:GridView>

                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="grdMusteri" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="btnMusteriAra" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <%--</div>--%>
                    <%--</div>--%>

                    <%--  <div class="modal-footer">
                                
                            </div>--%>
                </div>


            </div>

        </div>

    </div>
    <script type="text/javascript">
        function pageLoad(sender, args) {

            $('#ContentPlaceHolder1_txtTarihMakine').datetimepicker({
                format: 'L',

                locale: 'tr'
            });

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
        }

    </script>
</asp:Content>
