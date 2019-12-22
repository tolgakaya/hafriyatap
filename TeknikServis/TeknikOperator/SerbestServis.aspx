<%@ Page Title="" Language="C#" MasterPageFile="~/TeknikOperator/Operator.Master" AutoEventWireup="true" CodeBehind="SerbestServis.aspx.cs" Inherits="TeknikServis.TeknikOperator.SerbestServis" %>

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
        <div class="panel-group" id="accordionik">
        
            <div id="Div2" runat="server" class="panel panel-info">
                <div class="panel-heading">
                    <h2 class="panel-title">
                        <a data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" class="collapsed">Serbest Makine Hesapları</a>
                    </h2>
                </div>
                <div id="collapseTwo" class="panel-collapse in" style="height: 0px;">
                    <div class="panel-body">
                     
                        <div class="table-responsive">
                            <%-- servis hesapları başlıyor --%>
                            <asp:UpdatePanel ID="upCrudGrid" runat="server" ChildrenAsTriggers="true">
                                <ContentTemplate>

                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                                        DataKeyNames="hesapID"
                                        EmptyDataText="Kayıt girilmemiş" OnRowCommand="GridView1_RowCommand"
                                        OnPageIndexChanging="GridView1_PageIndexChanging" AllowPaging="true" PageSize="10">
                                        <PagerStyle CssClass="pagination-ys" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" HeaderStyle-Width="100">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="delLink"
                                                        runat="server"
                                                        CssClass="btn btn-danger btn-xs"
                                                        CommandName="del" CommandArgument='<%#Eval("hesapID") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm" ItemStyle-CssClass="visible-xs visible-sm">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="delLinkk"
                                                        runat="server"
                                                        CssClass="btn btn-danger btn-md"
                                                        CommandName="del" CommandArgument='<%#Eval("hesapID") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />
                                                </ItemTemplate>

                                            </asp:TemplateField>

                                            <asp:BoundField DataField="hesapID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                <HeaderStyle CssClass="visible-lg" />
                                                <ItemStyle CssClass="visible-lg" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="islemParca" HeaderText="İşlem" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="cihaz" HeaderText="Makine/Malzeme" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg "></asp:BoundField>
                                            <asp:BoundField DataField="dakika" HeaderText="Dakika" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg "></asp:BoundField>
                                            <asp:BoundField DataField="tarife" HeaderText="Tarife"></asp:BoundField>
                                            <asp:BoundField DataField="sure" HeaderText="Süre"></asp:BoundField>
                                            <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg "></asp:BoundField>

                                        </Columns>

                                    </asp:GridView>

                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCommand" />
                                                                    </Triggers>
                            </asp:UpdatePanel>

                            <%-- servishesapları bitiyor --%>

                            <%-- yeni makine hesabı seçimi başlıyor --%>
                            <div id="yeniMakineModal" class="modal  fade" tabindex="-1" role="dialog" aria-labelledby="yeniMakineModalLabel" aria-hidden="true">
                                <div class="modal-dialog modal-content modal-lg">
                                    <%--  <div class="modal-header modal-header-info">
                                        <a class="close" data-dismiss="modal" aria-hidden="true">×</a>
                                        <h3 id="yeniMakineModalLabel" class="baslik">Yeni Makine Çalışma Hesabı</h3>
                                    </div>--%>
                                    <%-- <div class="modal-body">--%>
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
                                                    <asp:Panel runat="server" ID="panelMakine" DefaultButton="btnMakine">


                                                        <div class="input-group custom-search-form ">
                                                            <span class="input-group-btn">

                                                                <asp:LinkButton ID="btnMakine"
                                                                    runat="server"
                                                                    CssClass="btn btn-info" OnClick="MakineAra"
                                                                    Text="<i class='fa fa-search icon-2x'></i>" />
                                                            </span>
                                                            <input runat="server" type="text" id="txtMakineAra" class="form-control" placeholder="Ara..." />

                                                        </div>
                                                    </asp:Panel>
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
                                                            <asp:AsyncPostBackTrigger ControlID="btnMakine" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="grdMakine" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnEkleHMakine" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnEkleHKMakine" EventName="Click" />

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
                                                        <asp:Panel runat="server" ID="panelMakine2" DefaultButton="btnKaydetMakine">

                                                            <div class="form-horizontal">

                                                                <div class="form-group">
                                                                    <label class="col-sm-2 control-label" for="drdTarife">Tarife</label>
                                                                    <div class="col-sm-4">
                                                                        <asp:DropDownList ID="drdTarife" runat="server" AutoPostBack="true" class="form-control" OnSelectedIndexChanged="drdTarife_SelectedIndexChanged">
                                                                        </asp:DropDownList>

                                                                    </div>
                                                                    <label hidden class="col-sm-2 control-label" for="txtFiyat">Birim Ücret</label>
                                                                    <div hidden class="col-sm-4">
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
                                                                <asp:HiddenField ID="hdnSaatlik" runat="server" />
                                                                <asp:HiddenField ID="hdnTarifeTipi" runat="server" />

                                                                <div runat="server" id="numara_aralik">
                                                                    <div class="form-group">
                                                                        <label for="txtSonNumara" class="col-sm-2 control-label">Başlama</label>
                                                                        <div class="col-sm-4">
                                                                            <asp:TextBox runat="server" ID="txtSonNumara" AutoPostBack="true" OnTextChanged="txtBaslangicChanged" class="form-control" />
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" EnableClientScript="true" ControlToValidate="txtSonNumara" ErrorMessage="Lütfen baslangic saati" CssClass="text-danger" ValidationGroup="valGrupMakine"></asp:RequiredFieldValidator>
                                                                            <asp:RangeValidator ID="RangeValidator5" runat="server" ControlToValidate="txtSonNumara" Type="Currency" ValidationGroup="valGrupMakine" CssClass="text-danger" MaximumValue="1000000" MinimumValue="0" ErrorMessage="Ondalık sayılar için virgül kullanınız."></asp:RangeValidator>
                                                                        </div>
                                                                        <label for="txtYeniNumara" class="col-sm-2 control-label">Bitiş</label>
                                                                        <div class="col-sm-4">
                                                                            <asp:TextBox runat="server" ID="txtYeniNumara" AutoPostBack="true" OnTextChanged="txtBaslangicChanged" class="form-control" />
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" EnableClientScript="true" ControlToValidate="txtYeniNumara" ErrorMessage="Lütfen baslangic saati" CssClass="text-danger" ValidationGroup="valGrupMakine"></asp:RequiredFieldValidator>
                                                                            <asp:RangeValidator ID="RangeValidator6" runat="server" ControlToValidate="txtYeniNumara" Type="Currency" ValidationGroup="valGrupMakine" CssClass="text-danger" MaximumValue="1000000" MinimumValue="0" ErrorMessage="Ondalık sayılar için virgül kullanınız."></asp:RangeValidator>
                                                                        </div>

                                                                    </div>
                                                                    <div hidden class="form-group">
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

                                                                <div hidden class="form-group">
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

                                                                <div hidden class="form-group">

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
                                                                <div hidden class="form-group">

                                                                    <label class="col-sm-2 control-label" for="txtYekunMakine">Tutar(KDV Dahil)</label>
                                                                    <div class="col-sm-10">
                                                                        <asp:TextBox ID="txtYekunMakine" runat="server" CssClass="form-control"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" EnableClientScript="true" ControlToValidate="txtYekunMakine" ErrorMessage="Lütfen toplam tutar giriniz" CssClass="text-danger" ValidationGroup="valGrupMakine"></asp:RequiredFieldValidator>
                                                                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtYekunMakine" Type="Currency" ValidationGroup="valGrupMakine" CssClass="text-danger" MaximumValue="1000000" MinimumValue="0" ErrorMessage="Ondalık sayılar için virgül kullanınız."></asp:RangeValidator>
                                                                        <asp:HiddenField ID="hdnHesapIDDuzenMakine" runat="server" />
                                                                    </div>
                                                                </div>
                                                                <div hidden class="form-group">

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
                                                                    <asp:Button ID="btnKaydetMakine" runat="server" Text="Kaydet" CausesValidation="true" ValidationGroup="valGrupMakine" CssClass="btn btn-info btn-block" OnClick="btnKaydetMakine_Click" />
                                                                </div>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="grdMakine" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>



                                    <div class="modal-footer">
                                        <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Kapat</button>
                                    </div>
                                </div>

                                <%-- buraaaa --%>
                                <%-- </div>--%>
                            </div>
                            <%-- makine hesabı seçimi bitiyor --%>
                        </div>

                        <div class=" btn-group pull-right visible-lg">

                            <asp:LinkButton ID="btnEkleHMakine"
                                runat="server"
                                CssClass="btn btn-danger" OnClick="btnEkleMakine_ClickH"
                                Text="<i class='fa fa-plus icon-3x'>Yeni Hesap Kaydet</i>" />


                        </div>

                        <div class=" btn-group pull-right visible-xs visible-sm">

                            <asp:LinkButton ID="btnEkleHKMakine"
                                runat="server"
                                CssClass="btn btn-danger" OnClick="btnEkleMakine_ClickH"
                                Text="<i class='fa fa-plus icon-3x'>Yeni Hesap Kaydet</i>" />


                        </div>
                    </div>

                    <%--   <div class="panel-footer pull-right">
                    </div>--%>
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
            $('#ContentPlaceHolder1_txtTarihMakine').datetimepicker({
                format: 'L',

                locale: 'tr'
            });

            $('#ContentPlaceHolder1_tarihtamirci').datetimepicker({
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
